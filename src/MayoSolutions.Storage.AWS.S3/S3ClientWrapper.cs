using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace MayoSolutions.Storage.AWS.S3
{
    internal class S3ClientWrapper : IGetItems
    {
        public AmazonS3Client Client { get; }
        public S3Bucket Bucket { get; }
        public string BucketName => Bucket.BucketName;
        public string Prefix { get; }

        public S3ClientWrapper(
            AmazonS3Client client,
            S3Bucket bucket,
            string prefix
        )
        {
            Client = client;
            Bucket = bucket;
            Prefix = prefix;
        }



        public async ValueTask<IFolder> GetFolderAsync(string name, CancellationToken cancellationToken = default)
        {
            var prefix = Prefix ?? "";
            if (prefix.Length > 0 && !prefix.EndsWith("/"))
                prefix += "/";

            var listRequest = new ListObjectsV2Request
            {
                BucketName = BucketName,
                Prefix = prefix + name,
                Delimiter = "/"
            };

            var response = await Client.ListObjectsV2Async(
                listRequest,
                cancellationToken);
            var folders = response.S3Objects;
            return folders
                .Where(o => o.Key.EndsWith("/"))
                .Select(o => new S3ObjectFolderWrapper(Client, Bucket, o))
                .Cast<IFolder>()
                .FirstOrDefault();
        }

        public async ValueTask<IFolder[]> GetFoldersAsync(string name, CancellationToken cancellationToken = default)
        {
            var prefix = Prefix ?? "";
            if (prefix.Length > 0 && !prefix.EndsWith("/"))
                prefix += "/";
            prefix += name;

            var listRequest = new ListObjectsV2Request
            {
                BucketName = BucketName,
                Prefix = prefix,
                Delimiter = "/"
            };

            var response = await Client.ListObjectsV2Async(
                listRequest,
                cancellationToken);
            var folders = response.S3Objects;
            return folders
                .Where(o => o.Key.EndsWith("/"))
                .Select(o => new S3ObjectFolderWrapper(Client, Bucket, o))
                .Cast<IFolder>()
                .ToArray();
        }

        public async ValueTask<IFile> GetFileAsync(string name, CancellationToken cancellationToken = default)
        {
            var prefix = Prefix ?? "";
            if (prefix.Length > 0 && !prefix.EndsWith("/"))
                prefix += "/";

            var listRequest = new ListObjectsV2Request
            {
                BucketName = BucketName,
                Prefix = prefix + name,
                Delimiter = "/"
            };

            var response = await Client.ListObjectsV2Async(
                listRequest,
                cancellationToken);
            var files = response.S3Objects;
            return files
                .Where(o => !o.Key.EndsWith("/"))
                .Select(o => new S3ObjectFileWrapper(o))
                .Cast<IFile>()
                .FirstOrDefault();
        }

        public async ValueTask<IFile[]> GetFilesAsync(string name, CancellationToken cancellationToken = default)
        {
            var prefix = Prefix ?? "";
            if (prefix.Length > 0 && !prefix.EndsWith("/"))
                prefix += "/";
            prefix += name;

            var listRequest = new ListObjectsV2Request
            {
                BucketName = BucketName,
                Prefix = prefix,
                Delimiter = "/"
            };

            var response = await Client.ListObjectsV2Async(
                listRequest,
                cancellationToken);
            var files = response.S3Objects;
            return files
                .Where(o => !o.Key.EndsWith("/"))
                .Select(o => new S3ObjectFileWrapper(o))
                .Cast<IFile>()
                .ToArray();
        }
    }
}