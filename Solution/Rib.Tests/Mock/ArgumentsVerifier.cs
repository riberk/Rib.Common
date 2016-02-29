namespace Rib.Tests.Mock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Moq;

    public class ArgumentsVerifier
    {
        [NotNull] private readonly Dictionary<Type, ConstructorInfo[]> _constructors;
        [NotNull] private readonly MethodInfo _createMockMethod;
        [NotNull] private readonly List<string> _errors;
        [NotNull] private readonly MockRepository _mockFactory;
        [NotNull] private readonly List<ConstructorInfo> _voidConstructors;

        public ArgumentsVerifier(IEnumerable<Type> types)
        {
            types = types.Where(x => !x.Name.StartsWith("<>")).Where(x => !x.IsNestedPrivate).Where(x => !x.IsGenericTypeDefinition);
            _mockFactory = new MockRepository(MockBehavior.Loose);
            _errors = new List<string>();
            _constructors = types
                    .Select(x => new
                    {
                        Type = x,
                        Constructors = x.GetConstructors()
                                        .Where(c => c.GetParameters().All(p => ((p.ParameterType.IsClass && !p.ParameterType.IsSealed) || p.ParameterType.IsInterface) && p.ParameterType != typeof (string)))
                                        .ToArray()
                    }).Where(x => x.Constructors.Any())
                    .ToDictionary(x => x.Type, x => x.Constructors);
            _voidConstructors = types.SelectMany(x => x.GetConstructors()).Where(x => !x.GetParameters().Any()).ToList();
            _createMockMethod = typeof (MockRepository).GetMethod("Create", new Type[0]);
        }

        [NotNull]
        public IReadOnlyCollection<string> Errors => _errors;

        [NotNull]
        public ArgumentsVerifier CheckAllVoidCtorsCreateObject()
        {
            foreach (var voidConstructor in _voidConstructors)
            {
                try
                {
                    voidConstructor.Invoke(new object[0]);
                }
                catch (Exception e)
                {
                    _errors.Add(e.ToString());
                }
            }
            return this;
        }

        [NotNull]
        public ArgumentsVerifier CheckNullArgumentsOnConstructors<T>()
        {
            return CheckNullArgumentsOnConstructors(typeof (T));
        }

        [NotNull]
        public ArgumentsVerifier CheckNullArgumentsOnConstructors([NotNull] Type t)
        {
            return CheckNullArgumentOnConstructors(t.GetConstructors());
        }

        [NotNull]
        private ArgumentsVerifier CheckNullArgumentOnConstructors([NotNull] IEnumerable<ConstructorInfo> infos)
        {
            foreach (var mi in infos)
            {
                var args = mi.GetParameters();
                var dict = args.ToDictionary(x => x,
                                             parameterInfo =>
                                             _createMockMethod.MakeGenericMethod(parameterInfo.ParameterType).Invoke(_mockFactory, new object[0]));
                foreach (var source in dict)
                {
                    var p = args.Select(x =>
                    {
                        if (source.Key == x) return null;
                        var s = dict[x];
                        try
                        {
                            return s.GetType()
                                    .GetProperties()
                                    .Single(pp => pp.Name == "Object" && pp.DeclaringType == s.GetType())
                                    .GetValue(s);
                        }
                        catch (Exception e)
                        {
                            throw new AggregateException($"Exception on argument {x.Name} o type {x.Member.DeclaringType}", e);
                        }
                    }).ToArray();
                    try
                    {
                        mi.Invoke(p);
                        _errors.Add($"Not throw exception constructor {mi} on type {mi.DeclaringType} where {source.Key} is null");
                    }
                    catch (TargetInvocationException tie) when (tie.InnerException is ArgumentNullException)
                    {
                    }
                    catch (Exception ex)
                    {
                        _errors.Add(ex.ToString());
                    }
                }
            }
            return this;
        }

        [NotNull]
        public ArgumentsVerifier CheckNullArgumentsOnConstructors()
        {
            foreach (var info in _constructors)
            {
                CheckNullArgumentOnConstructors(info.Value);
            }
            return this;
        }

        [NotNull]
        public static ArgumentsVerifierBuilder Builder(Type t)
        {
            return new ArgumentsVerifierBuilder(t);
        }

        public class ArgumentsVerifierBuilder
        {
            [NotNull] private readonly HashSet<Type> _allTypes;

            public ArgumentsVerifierBuilder(Type assemblyType)
            {
                var types = assemblyType.Assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface);
                _allTypes = new HashSet<Type>(types);
            }

            [NotNull]
            public ArgumentsVerifierBuilder Exclude<T>()
            {
                _allTypes.Remove(typeof (T));
                return this;
            }

            [NotNull]
            public ArgumentsVerifier ToVerifier()
            {
                return new ArgumentsVerifier(_allTypes);
            }
        }
    }
}