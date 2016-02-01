namespace Rib.Common.Models.Helpers
{
    using System;

    /// <summary>
    ///     Идентификатор корреляции
    ///     Используется для определения текущего контекста запроса/потока
    /// </summary>
    [Serializable]
    public class CorrelationId :
            IEquatable<CorrelationId>,
            IEquatable<Guid>,
            IComparable<CorrelationId>,
            IComparable<Guid>
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="CorrelationId" />.
        /// </summary>
        public CorrelationId() : this(Guid.NewGuid())
        {
        }

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="CorrelationId" />.
        /// </summary>
        public CorrelationId(Guid value)
        {
            Value = value;
        }

        private Guid Value { get; }

        public int CompareTo(CorrelationId other)
        {
            if (other == null)
            {
                throw new NullReferenceException("CorelationId can not be null");
            }
            return Value.CompareTo(other);
        }

        public int CompareTo(Guid value)
        {
            return Value.CompareTo(value);
        }

        public bool Equals(CorrelationId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public bool Equals(Guid g)
        {
            return Value.Equals(g);
        }

        public byte[] ToByteArray()
        {
            return Value.ToByteArray();
        }

        public int CompareTo(object value)
        {
            return Value.CompareTo(value);
        }

        public string ToString(string format)
        {
            return Value.ToString(format);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return Value.ToString(format, provider);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CorrelationId) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CorrelationId left, CorrelationId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CorrelationId left, CorrelationId right)
        {
            return !Equals(left, right);
        }

        public static implicit operator CorrelationId(Guid id)
        {
            return new CorrelationId(id);
        }

        public static implicit operator Guid(CorrelationId id)
        {
            return id?.Value ?? default(Guid);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}