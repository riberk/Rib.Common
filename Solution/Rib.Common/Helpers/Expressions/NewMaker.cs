namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq.Expressions;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Models.Exceptions;
    using JetBrains.Annotations;

    internal class NewMaker : INewMaker
    {
        [NotNull] private readonly ICacherFactory _cacherFactory;

        public NewMaker([NotNull] ICacherFactory cacherFactory)
        {
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _cacherFactory = cacherFactory;
        }

        public Func<T> Create<T>()
        {
            var fullName = typeof (T).FullName;
            return _cacherFactory.Create<Func<T>>($"{GetType().FullName}").GetOrAdd(fullName, Create<T>);
        }

        private static Func<T> Create<T>(string arg)
        {
            var type = typeof (T);
            var constructorInfo = type.GetConstructor(new Type[0]);
            if (constructorInfo == null)
            {
                throw new MetadataException($"Parameterless constructor could not be found for {type}");
            }
            return Expression.Lambda<Func<T>>(Expression.New(constructorInfo)).Compile();
        }
    }
}