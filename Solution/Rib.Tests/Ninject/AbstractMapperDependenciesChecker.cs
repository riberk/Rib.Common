namespace Rib.Tests.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Ninject;
    using global::Ninject.Activation;
    using global::Ninject.Activation.Caching;
    using global::Ninject.Modules;
    using global::Ninject.Parameters;
    using global::Ninject.Planning;
    using JetBrains.Annotations;
    using TsSoft.AbstractMapper;
    using TsSoft.AbstractMapper.Engine;
    using TsSoft.AbstractMapper.Rules;
    using TsSoft.Expressions.Helpers.Reflection;
    using TsSoft.Expressions.Models.AbstractMapper;
    using TsSoft.Expressions.SelectBuilder.Models;

    /// <summary>
    ///     Проверяет разрешимость зависимостей абстрактных мапперов
    /// </summary>
    public static class AbstractMapperDependenciesChecker
    {
        [NotNull] private static readonly MethodInfo CheckMapperDef;

        static AbstractMapperDependenciesChecker()
        {
            CheckMapperDef = new MemberInfoHelper().GetGenericDefinitionMethodInfo(() => CheckMapperIfAlreadyRequested<object, object>(null, null));
        }

        /// <summary>
        ///     Является ли тип абстрактным маппером
        /// </summary>
        public static bool IsMapperAbstract([NotNull] Type t)
        {
            if (t == typeof (object))
            {
                return false;
            }
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof (Mapper<,>) ||
                   t.BaseType != null && IsMapperAbstract(t.BaseType);
        }

        /// <summary>
        ///     Имеется ли маппер в контексте ядра. Он там имеется, если к такому типу уже был запрос
        /// </summary>
        /// <param name="kernel">Нинжект-ядро</param>
        /// <param name="t">Тип маппера</param>
        public static bool HasInContext([NotNull] IKernel kernel, [NotNull] Type t)
        {
            var request = kernel.CreateRequest(t, null, Enumerable.Empty<IParameter>(), false, false);
            if (request == null)
            {
                throw new NullReferenceException("request");
            }
            if (!kernel.CanResolve(request))
            {
                return false;
            }
            var bindings = kernel.GetBindings(request.Service);
            if (bindings == null)
            {
                throw new NullReferenceException("bindings");
            }
            var components = kernel.Components;
            if (components == null)
            {
                throw new NullReferenceException("components");
            }
            var cache = components.Get<ICache>();
            if (cache == null)
            {
                throw new NullReferenceException("cache");
            }
            var context = new Context(kernel, request, bindings.First(), cache, components.Get<IPlanner>(), components.Get<IPipeline>());
            return cache.TryGet(context) != null;
        }

        [NotNull]
        private static PropertyInfo GetMapperProperty<TFrom, TTo>([NotNull] string name)
            where TFrom : class
            where TTo : class, new()
        {
            var result = typeof (Mapper<TFrom, TTo>).GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (result == null)
            {
                throw new InvalidOperationException($"Property {name} not found in {typeof (Mapper<TFrom, TTo>)}");
            }
            return result;
        }

        /// <summary>
        ///     Проверить разрешимость зависимостей маппера, если он уже запрашивался из ядра
        /// </summary>
        /// <typeparam name="TFrom">Тип, из которого преобразует маппер</typeparam>
        /// <typeparam name="TTo">Тип, в который преобразует маппер</typeparam>
        /// <param name="mapperType">Тип маппера</param>
        /// <param name="kernel">Ядро</param>
        public static IEnumerable<string> CheckMapperIfAlreadyRequested<TFrom, TTo>([NotNull] Type mapperType, [NotNull] IKernel kernel)
            where TFrom : class
            where TTo : class, new()
        {
            if (mapperType.GetInterfaces().All(i => i != null && !HasInContext(kernel, i)))
            {
                return new string[0];
            }
            object mapperInstance;
            IMapExpressionCreator expressionCreator;
            var result = new List<string>();
            try
            {
                mapperInstance = kernel.Get(mapperType);
                expressionCreator = kernel.Get<IMapExpressionCreator>();
            }
            catch (ActivationException)
            {
                return new string[0]; // if this mapper is injected somewhere, let the test fail on those parts
            }
            if (expressionCreator == null)
            {
                throw new NullReferenceException("expressionCreator");
            }
            var mapRules = GetMapperProperty<TFrom, TTo>("MapRules").GetValue(mapperInstance) as IMapRules<TTo, TFrom>;
            var ignoreRules = GetMapperProperty<TFrom, TTo>("IgnoreRules").GetValue(mapperInstance) as IIgnoreRules<TTo>;
            var autoProps = (AutoPropertiesBehavior) GetMapperProperty<TFrom, TTo>("AutoPropertiesBehavior").GetValue(mapperInstance);
            var innerMapper = (InnerMapperStrategy) GetMapperProperty<TFrom, TTo>("InnerMapperStrategy").GetValue(mapperInstance);
            IReadOnlyCollection<MappedPathDescription> paths;
            try
            {
                paths = expressionCreator.GetExpression(mapRules, ignoreRules, autoProps, innerMapper).MappedPaths;
            }
            catch (Exception ex)
            {
                return new[] {$"Construction of expression for {mapperType} failed: {ex}"};
            }
            foreach (var path in paths)
            {
                if (path?.MapperDescription != null)
                {
                    try
                    {
                        kernel.Get(typeof (IMapper<,>).MakeGenericType(path.MapperDescription.MapperFromType, path.MapperDescription.MapperToType));
                    }
                    catch (ActivationException ex)
                    {
                        result.Add($"Error resolving inner mappers for {mapperType} : {ex}");
                    }
                }
            }
            try
            {
                var selectProvider = mapperInstance as ISelectProvider<TFrom>;
                if (selectProvider != null)
                {
                    var select = selectProvider.Select;
                }
            }
            catch (Exception e)
            {
                result.Add($"Не удалось построить селект для маппера {mapperType} : {e}");
            }
            return result;
        }

        /// <summary>
        ///     Проверить разрешимость зависимостей маппера, если он уже запрашивался из ядра
        /// </summary>
        /// <param name="mapperType">Тип маппера</param>
        /// <param name="kernel">Ядро</param>
        public static IEnumerable<string> CheckMapperIfAlreadyRequested([NotNull] Type mapperType, [NotNull] IKernel kernel)
        {
            var mapperInterface =
                mapperType.GetInterfaces().Single(i => i != null && i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IMapper<,>));
            if (mapperInterface?.GenericTypeArguments == null)
            {
                throw new NullReferenceException("mapperInterface");
            }
            var fromType = mapperInterface.GenericTypeArguments[0];
            var toType = mapperInterface.GenericTypeArguments[1];
            if (fromType == null)
            {
                throw new NullReferenceException("fromType");
            }
            if (toType == null)
            {
                throw new NullReferenceException("toType");
            }
            if (mapperType.IsGenericType)
            {
                if (fromType.IsGenericParameter)
                {
                    return null;
                }
                var constraints = toType.GetGenericParameterConstraints();
                if (constraints.Length < 1)
                {
                    return null;
                }
                if (constraints[0] == null)
                {
                    return null;
                }
                if (constraints[0].IsClass)
                {
                    return
                        CheckMapperDef.MakeGenericMethod(fromType, constraints[0])
                            .Invoke(null, new object[] {mapperType.MakeGenericType(constraints[0]), kernel})
                            as IEnumerable<string>;
                }
                return null;
            }
            return CheckMapperDef.MakeGenericMethod(fromType, toType).Invoke(null, new object[] {mapperType, kernel})
                as IEnumerable<string>;
        }

        /// <summary>
        ///     Проверить разрешимость зависимостей мапперов, которые уже были запрошены из ядра ранее
        /// </summary>
        /// <param name="mapperTypes">Типы мапперов</param>
        /// <param name="kernel">Ядро</param>
        public static IEnumerable<string> CheckAlreadyRequestedMappers([NotNull] IEnumerable<Type> mapperTypes, [NotNull] IKernel kernel)
        {
            return mapperTypes.Where(m => m != null).SelectMany(m => CheckMapperIfAlreadyRequested(m, kernel) ?? Enumerable.Empty<string>());
        }

        /// <summary>
        ///     Проверить разрешимость зависимостей мапперов из заданной модулем сборки, которые уже были запрошены из ядра ранее
        /// </summary>
        /// <typeparam name="T">Тип модуля, определяющий сборку</typeparam>
        /// <param name="kernel">Ядро</param>
        public static IEnumerable<string> CheckAlreadyRequestedMappersFromAssemblyWithModule<T>([NotNull] IKernel kernel) where T : INinjectModule
        {
            if (kernel == null)
            {
                throw new ArgumentNullException(nameof(kernel));
            }
            var assembly = Assembly.GetAssembly(typeof (T));
            if (assembly == null)
            {
                throw new NullReferenceException("assembly");
            }
            var mapperTypes = assembly
                .GetTypes()
                .Where(t => t != null && !t.IsAbstract && t.GetInterfaces().Any(
                    i => i != null
                         && i.IsGenericType
                         && i.GetGenericTypeDefinition() == typeof (IMapper<,>))
                            && IsMapperAbstract(t))
                .ToList();
            return CheckAlreadyRequestedMappers(mapperTypes, kernel);
        }
    }
}