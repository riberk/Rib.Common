namespace Rib.Common.Models.Interfaces
{
    using System.IO;

    public interface IUploadedFile
    {
        string FileName { get; }
        Stream File { get; }
    }
}