namespace Rib.Common.Models.Metadata
{
    using System;
    using JetBrains.Annotations;

    public abstract class EnumModel : IEnumModel
    {
        [NotNull]
        public abstract object Value { get; }

        [NotNull]
        public abstract string Name { get; }

        public abstract string Description { get; }
    }

    public class EnumModel<T> : EnumModel, IEnumModel<T> 
        where T : struct
    {
        private string _name;

        public EnumModel(T enumValue, string description, [NotNull] object val)
        {
            if (val == null) throw new ArgumentNullException(nameof(val));
            EnumValue = enumValue;
            Description = description;
            Value = val;
        }

        public T EnumValue { get; }
        public override object Value { get; }
        public override string Name => _name ?? (_name = EnumValue.ToString());
        public override string Description { get; }
    }
}