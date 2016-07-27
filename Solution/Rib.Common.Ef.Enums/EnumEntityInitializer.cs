namespace Rib.Common.Ef.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Expressions;
    using Rib.Common.Helpers.Metadata.Enums;

    public class EnumEntityInitializer : IEnumEntityInitializer
    {
        [NotNull] private readonly IEnumAttributeReader _enumAttributeReader;
        [NotNull] private readonly INewMaker _newMaker;
        [NotNull] private readonly MethodInfo _method;

        public EnumEntityInitializer([NotNull] IEnumAttributeReader enumAttributeReader, [NotNull] INewMaker newMaker)
        {
            if (enumAttributeReader == null) throw new ArgumentNullException(nameof(enumAttributeReader));
            if (newMaker == null) throw new ArgumentNullException(nameof(newMaker));
            _enumAttributeReader = enumAttributeReader;
            _newMaker = newMaker;
            _method = GetType().GetMethod("FillTable", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>Fill enum tables</summary>
        public async Task FillEnumTablesAsync(DbContext ctx, IEnumerable<Type> entityTypes)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            if (entityTypes == null) throw new ArgumentNullException(nameof(entityTypes));
            Func<Type, bool> predicate = i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumEntity<>);
            var dbEnumTypes = entityTypes.Where(x => x.GetInterfaces().Any(predicate));

            foreach (var dbEnumType in dbEnumTypes)
            {
                var @interface = dbEnumType.GetInterfaces().First(predicate);
                await (Task) _method.MakeGenericMethod(dbEnumType, @interface.GetGenericArguments()[0])
                                    .Invoke(this, new object[] {ctx});
            }
            await ctx.SaveChangesAsync();
        }

        [UsedImplicitly]
        private async Task FillTable<T, TEnum>([NotNull] DbContext ctx) where T : class, IEnumEntity<TEnum>
                where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var factory = _newMaker.Create<T>();
            var dbSet = ctx.Set<T>();
            var entities = await dbSet.ToDictionaryAsync(x => x.Id, x => x);
            var enums = typeof(TEnum).GetEnumValues().Cast<TEnum>();
            var deletedSet = new HashSet<T>(entities.Values);
            foreach (var @enum in enums)
            {
                T dbEnum;
                if (entities.TryGetValue(@enum, out dbEnum))
                {
                    deletedSet.Remove(dbEnum);
                    continue;
                }
                var val = factory();
                val.Id = @enum;
                val.IsDeleted = false;
                val.Name = @enum.ToString();
                val.Title = _enumAttributeReader.Description(@enum);
                dbSet.Add(val);
            }
            foreach (var deletingEntity in deletedSet)
            {
                deletingEntity.IsDeleted = true;
            }
        }
    }
}