using System.IO;

namespace MayoSolutions.Storage.Local
{
    internal class LocalStorageBucketWrapper : LocalStorageFolderWrapper, IBucket
    {
        public string StoragePath { get; }

        public LocalStorageBucketWrapper(string storagePath, string bucketName)
            : base(Path.Combine(storagePath, bucketName), bucketName)
        {
            StoragePath = storagePath;
        }
    }
}