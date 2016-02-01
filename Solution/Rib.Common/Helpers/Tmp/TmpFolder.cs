namespace Rib.Common.Helpers.Tmp
{
    using System;
    using System.IO;
    using JetBrains.Annotations;

    public class TmpFolder : ITmpFolder
    {
        [NotNull] private readonly DirectoryInfo _di;
        [NotNull] private readonly FileStream _lock;

        /// <summary>
        ///     »нициализирует новый экземпл€р класса <see cref="T:System.Object" />.
        /// </summary>
        private TmpFolder(string root)
        {
            Id = Guid.NewGuid();
            _di = new DirectoryInfo(System.IO.Path.Combine(root, Id.ToString()));
            if (!_di.Exists)
            {
                _di.Create();
            }
            _lock = new FileStream(System.IO.Path.Combine(Path, Id.ToString()), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
        }

        public void Dispose()
        {
            _lock.Dispose();
            _di.Delete(true);
        }

        public string Path => _di.FullName;
        public Guid Id { get; }

        [NotNull]
        public static ITmpFolder Create(string root)
        {
            return new TmpFolder(root);
        }
    }
}