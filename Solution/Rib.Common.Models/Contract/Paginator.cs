namespace Rib.Common.Models.Contract
{
    using System;
    using JetBrains.Annotations;

    public class Paginator : IPaginator
    {
        [NotNull] private static readonly Paginator FullPaginator = new Paginator(1, int.MaxValue);

        private int _pageNumber;
        private int _pageSize;

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="Paginator" />.
        /// </summary>
        public Paginator(int pageNumber, int pageSize) : this()
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public Paginator()
        {
        }

        [NotNull]
        public static IPaginator Full => FullPaginator;

        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Номер страницы должен быть положительным числом");
                }
                _pageNumber = value;
            }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Размер страницы должен быть положительным числом");
                }
                _pageSize = value;
            }
        }

        public int Skip => (PageNumber - 1)*PageSize;
    }
}