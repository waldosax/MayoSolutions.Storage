using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

namespace MayoSolutions.Storage.Google
{
    internal class GoogleStorageClientWrapper : IGetItems
    {
        public StorageClient Client { get; }
        public Bucket Bucket { get; }

        public string BucketName => Bucket.Name;
        public string Prefix { get; }

        public GoogleStorageClientWrapper(
            StorageClient client,
            Bucket bucket,
            string prefix
        )
        {
            Client = client;
            Bucket = bucket;
            Prefix = prefix;
        }

        public async ValueTask<IFolder> GetFolderAsync(string name, CancellationToken cancellationToken = default)
        {
            var obj = await Client.GetObjectAsync(BucketName, name, new GetObjectOptions(), cancellationToken);
            var wrapper = new GoogleStorageFolderWrapper(Client, Bucket, obj);
            return wrapper;
        }

        public async ValueTask<IFolder[]> GetFoldersAsync(string name, CancellationToken cancellationToken = default)
        {
            var prefix = Prefix ?? "";
            if (prefix.Length > 0 && !prefix.EndsWith("/"))
                prefix += "/";
            prefix += name;
            var objects = Client.ListObjectsAsync(
                    BucketName, prefix,
                    new ListObjectsOptions
                    {
                        Delimiter = "/"
                    })
                .WithCancellation(cancellationToken);

            List<IFolder> items = new List<IFolder>();
            var enumerator = objects.GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                var obj = enumerator.Current;
                var wrapper = new GoogleStorageFolderWrapper(Client, Bucket, obj);
                items.Add(wrapper);
            }

            return items.ToArray();
        }

        public async ValueTask<IFile> GetFileAsync(string name, CancellationToken cancellationToken = default)
        {
            var obj = await Client.GetObjectAsync(BucketName, name, new GetObjectOptions(), cancellationToken);
            var wrapper = new GoogleStorageFileWrapper(obj);
            return wrapper;
        }

        public async ValueTask<IFile[]> GetFilesAsync(string name, CancellationToken cancellationToken = default)
        {
            var prefix = Prefix ?? "";
            if (prefix.Length > 0 && !prefix.EndsWith("/"))
                prefix += "/";
            prefix += name;
            var objects = Client.ListObjectsAsync(
                    BucketName, prefix,
                    new ListObjectsOptions
                    {
                        Delimiter = "/"
                    })
                .WithCancellation(cancellationToken);

            List<IFile> items = new List<IFile>();
            var enumerator = objects.GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                var obj = enumerator.Current;
                var wrapper = new GoogleStorageFileWrapper(obj);
                items.Add(wrapper);
            }

            return items.ToArray();
        }
    }
}