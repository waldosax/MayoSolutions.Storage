using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Newtonsoft.Json;

namespace MayoSolutions.Storage.Google
{
    // TODO: IDisposable
    public class GoogleStorageClient : IGoogleStorageClient
    {
        private readonly GoogleStorageClientConfig _config;

        private StorageClient _storageClient;

        public GoogleStorageClient(
            GoogleStorageClientConfig config
            )
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }


        private async Task EnsureStorageClient()
        {
            if (_storageClient != null) return;
            var json = JsonConvert.SerializeObject(_config);
            var credentials = GoogleCredential.FromJson(json);
            _storageClient = await StorageClient.CreateAsync(credentials);
        }

        

        public async ValueTask<IFolder[]> GetFoldersAsync(string bucketName, string path, CancellationToken cancellationToken = default)
        {
            await EnsureStorageClient();
            var cleanPath = path ?? "";
            if (cleanPath.Length > 0 && !cleanPath.EndsWith("/"))
                cleanPath += "/";

            var objects = _storageClient.ListObjects(
                    bucketName, cleanPath,
                    new ListObjectsOptions
                    {
                        Delimiter = "/",
                        IncludeTrailingDelimiter = true,
                        PageSize = _config.PageSize,
                    });

            List<IFolder> items = new List<IFolder>();
            using (var enumerator = objects.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var obj = enumerator.Current;
                    if (!obj.Name.EndsWith("/")) continue;
                    var absolutePath = cleanPath + obj.Name;
                    var wrapper = new GoogleStorageFolderWrapper(absolutePath, obj);
                    items.Add(wrapper);
                }
            }

            return items.ToArray();
        }

        public async ValueTask<IFile> GetFileAsync(string bucketName, string path, CancellationToken cancellationToken = default)
        {
            await EnsureStorageClient();
            var cleanPath = path ?? throw new ArgumentNullException(nameof(path));
            var obj = await _storageClient.GetObjectAsync(bucketName, cleanPath, new GetObjectOptions(), cancellationToken);
            var wrapper = new GoogleStorageFileWrapper(obj);
            return wrapper;
        }

        public async ValueTask<IFile[]> GetFilesAsync(string bucketName, string path, CancellationToken cancellationToken = default)
        {
            await EnsureStorageClient();
            var cleanPath = path ?? "";
            if (cleanPath.Length > 0 && !cleanPath.EndsWith("/"))
                cleanPath += "/";
            var objects = _storageClient.ListObjectsAsync(
                    bucketName, cleanPath,
                    new ListObjectsOptions
                    {
                        Delimiter = "/",
                        IncludeTrailingDelimiter = false,
                        PageSize = _config.PageSize,
                    })
                .WithCancellation(cancellationToken);

            List<IFile> items = new List<IFile>();
            var enumerator = objects.GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                var obj = enumerator.Current;
                if (obj.Name.EndsWith("/")) continue;
                var wrapper = new GoogleStorageFileWrapper(obj);
                items.Add(wrapper);
            }

            return items.ToArray();
        }
    }
}
