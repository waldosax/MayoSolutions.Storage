using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#pragma warning disable CS1998

namespace MayoSolutions.Storage.Local
{
    public class LocalStorageClient : ILocalStorageClient
    {
        private readonly LocalStorageClientConfig _config;


        public LocalStorageClient(
            LocalStorageClientConfig config
            )
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
        

        public async ValueTask<IFolder[]> GetFoldersAsync(string bucketName, string path, CancellationToken cancellationToken = default)
        {
            var cleanPath = path.SanitizeSimulatedPath(); // TODO: Null check
            if (cleanPath.Length > 0 && !cleanPath.EndsWith("/"))
                cleanPath += "/";
            var fullPath = Path.Combine(_config.StoragePath, bucketName, path.SanitizeLocalPath());
            
            if (Directory.Exists(fullPath))
                return Directory.GetDirectories(fullPath)
                    .Select(child => new LocalStorageFolderWrapper(child, cleanPath + new DirectoryInfo(child).Name + "/"))
                    .Cast<IFolder>()
                    .ToArray();
            
            throw new IOException($"Directory {cleanPath} not found.");
        }

        public async ValueTask<IFile> GetFileAsync(string bucketName, string path, CancellationToken cancellationToken = default)
        {
            var cleanPath = path.SanitizeSimulatedPath(); // TODO: Null check
            var fullPath = Path.Combine(_config.StoragePath, bucketName, path.SanitizeLocalPath());
            
            if (File.Exists(fullPath))
                return new LocalStorageFileWrapper(fullPath, cleanPath);
            
            throw new IOException("File {cleanPath} not found.");
        }

        public async ValueTask<IFile[]> GetFilesAsync(string bucketName, string path, CancellationToken cancellationToken = default)
        {
            var cleanPath = path.SanitizeSimulatedPath(); // TODO: Null check
            if (cleanPath.Length > 0 && !cleanPath.EndsWith("/"))
                cleanPath += "/";
            var fullPath = Path.Combine(_config.StoragePath, bucketName, path.SanitizeLocalPath());
            
            if (Directory.Exists(fullPath))
                return Directory.GetDirectories(fullPath)
                    .Select(child => new LocalStorageFileWrapper(child, cleanPath))
                    .Cast<IFile>()
                    .ToArray();
            
            throw new IOException($"Directory {cleanPath} not found.");
        }
    }
}
