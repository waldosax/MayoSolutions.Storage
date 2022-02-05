using System.IO;

namespace MayoSolutions.Storage.Local
{
    internal class LocalStorageFileWrapper : IFile
    {
        public string FullPath { get; protected set; }
        public string BucketName { get; }
        public string Identifier => FullPath;
        public string Name => new FileInfo(FullPath).Name;
        public long? Size => new FileInfo(FullPath).Length;

        public LocalStorageFileWrapper(
            string fullPath,
            string bucketName
        )
        {
            FullPath = fullPath;
            BucketName = bucketName;
        }

    }
}