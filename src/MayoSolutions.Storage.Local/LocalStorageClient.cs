using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MayoSolutions.Storage.Local
{
    public class LocalStorageClient : IStorageClient
    {

        public string ColdStoragePath = @"F:\Code\Plex\ColdStorage";

        public LocalStorageClient()
        {
            // TODO: Config
        }

        public async ValueTask<IBucket> GetBucketAsync(string bucketIdentifier, CancellationToken cancellationToken = default)
        {
            string fullPath = Path.Combine(ColdStoragePath, bucketIdentifier);
            if (Directory.Exists(fullPath))
                return new LocalStorageBucketWrapper(ColdStoragePath, bucketIdentifier);
            throw new IOException("Directory not found.");
        }
    }

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

    internal class LocalStorageBucketWrapper : LocalStorageFolderWrapper, IBucket
    {
        public string StoragePath { get; }

        public LocalStorageBucketWrapper(string storagePath, string bucketName)
            : base(Path.Combine(storagePath, bucketName), bucketName)
        {
            StoragePath = storagePath;
        }
    }

    internal class LocalStorageFolderWrapper : IFolder
    {
        public string FullPath { get; protected set; }
        public string BucketName { get; }
        public string Identifier => FullPath;
        public string Name => new DirectoryInfo(FullPath).Name;

        public LocalStorageFolderWrapper(
            string fullPath,
            string bucketName
        )
        {
            FullPath = fullPath;
            BucketName = bucketName;
        }




        public async ValueTask<IFolder> GetFolderAsync(string name, CancellationToken cancellationToken = default)
        {
            var fullPath = Path.Combine(FullPath, name);
            if (Directory.Exists(fullPath))
                return new LocalStorageFolderWrapper(fullPath, BucketName);
            throw new IOException("Directory not found.");
        }

        public async ValueTask<IFolder[]> GetFoldersAsync(string name, CancellationToken cancellationToken = default)
        {
            var fullPath = Path.Combine(FullPath, name);
            if (Directory.Exists(fullPath))
                return Directory.GetDirectories(fullPath)
                    .Select(child => new LocalStorageFolderWrapper(child, BucketName))
                    .Cast<IFolder>()
                    .ToArray();
            throw new IOException("Directory not found.");
        }

        public async ValueTask<IFile> GetFileAsync(string name, CancellationToken cancellationToken = default)
        {
            var fullPath = Path.Combine(FullPath, name);
            if (Directory.Exists(fullPath))
                return Directory.GetFiles(fullPath)
                    .Select(child => new LocalStorageFileWrapper(child, BucketName))
                    .Cast<IFile>()
                    .FirstOrDefault();
            throw new IOException("Directory not found.");
        }

        public async ValueTask<IFile[]> GetFilesAsync(string name, CancellationToken cancellationToken = default)
        {
            var fullPath = Path.Combine(FullPath, name);
            if (Directory.Exists(fullPath))
                return Directory.GetFiles(fullPath)
                    .Select(child => new LocalStorageFileWrapper(child, BucketName))
                    .Cast<IFile>()
                    .ToArray();
            throw new IOException("Directory not found.");
        }


    }
}
