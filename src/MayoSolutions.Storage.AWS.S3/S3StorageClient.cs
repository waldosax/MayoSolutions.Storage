using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace MayoSolutions.Storage.AWS.S3
{
    public class S3StorageClient : IS3StorageClient
    {
        private readonly S3StorageClientConfig _config;
        private AmazonS3Client _storageClient;

        public S3StorageClient(
            S3StorageClientConfig config
            )
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        private void EnsureStorageClient()
        {
            if (_storageClient != null) return;
            BasicAWSCredentials basicCredentials =
                new BasicAWSCredentials(
                    _config.AccessKey,
                    _config.SecretKey);

            var endpoint = RegionEndpoint.GetBySystemName(_config.RegionEndpoint);
            _storageClient = new AmazonS3Client(basicCredentials, endpoint);
        }


        //public async ValueTask<IBucket> GetBucketAsync(string bucketIdentifier, CancellationToken cancellationToken = default)
        //{
        //    BasicAWSCredentials basicCredentials =
        //        new BasicAWSCredentials(
        //            _config.AccessKey,
        //            _config.SecretKey);

        //    var client = new AmazonS3Client(basicCredentials);
        //    var buckets = await client.ListBucketsAsync(cancellationToken);
        //    var bucket = buckets.Buckets.Find(b => b.BucketName == bucketIdentifier);
        //    var wrapper = new S3BucketWrapper(client, bucket);
        //    return wrapper;
        //}

        

        public async ValueTask<IFolder[]> GetFoldersAsync(string bucketName, string path, CancellationToken cancellationToken = default)
        {
            EnsureStorageClient();
            var cleanPath = path ?? "";
            if (cleanPath.Length > 0 && !cleanPath.EndsWith("/"))
                cleanPath += "/";

            var allFolders = new HashSet<string>();
            string continuationToken = null;

            // NOTE: This is amongst the most egregious things I've ever had to code to
            //   shoehorn something into an interface/concept.
            //
            //   This lists ALL the objects (files) at or below prefix in order to
            //   SIMULATE a directory structure .
request_chunk:
            var listRequest = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = cleanPath,
                ContinuationToken = continuationToken,
                MaxKeys = _config.PageSize ?? 1000,
            };

            var response = await _storageClient.ListObjectsV2Async(
                listRequest,
                cancellationToken);
            var folders = response.S3Objects;
            folders
                .Select(obj => GetFirstLevelKey(obj))
                .Distinct()
                .ToList()
                .ForEach(parent => allFolders.Add(parent));
            if (response.IsTruncated)
            {
                continuationToken = response.NextContinuationToken;
                goto request_chunk;
            }

            return allFolders
                .OrderBy(folder => folder)
                .Select(folder => new S3FolderShim(cleanPath + folder, folder))
                .Cast<IFolder>()
                .ToArray();
        }

        public async ValueTask<IFile> GetFileAsync(string bucketName, string path, CancellationToken cancellationToken = default)
        {
            EnsureStorageClient();
            var cleanPath = path ?? throw new ArgumentNullException(nameof(path));

            var response = await _storageClient.GetObjectAsync(bucketName, cleanPath, cancellationToken);
            return new S3ObjectFileWrapper(cleanPath, response);
        }

        public async ValueTask<IFile[]> GetFilesAsync(string bucketName, string path, CancellationToken cancellationToken = default)
        {
            EnsureStorageClient();
            var cleanPath = path ?? "";
            if (cleanPath.Length > 0 && !cleanPath.EndsWith("/"))
                cleanPath += "/";

            var allFiles = new List<S3Object>();
            string continuationToken = null;

request_chunk:
            var listRequest = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = cleanPath,
                Delimiter = "/",
                ContinuationToken = continuationToken,
                MaxKeys = _config.PageSize ?? 1000,
            };

            var response = await _storageClient.ListObjectsV2Async(
                listRequest,
                cancellationToken);
            var files = response.S3Objects;
            allFiles.AddRange(files
                .Where(obj => GetParentKey(obj) == cleanPath)
                );
            if (response.IsTruncated)
            {
                continuationToken = response.NextContinuationToken;
                goto request_chunk;
            }

            return allFiles
                .OrderBy(obj => obj.Key)
                .Select(o => new S3ObjectFileWrapper(o.Key, o))
                .Cast<IFile>()
                .ToArray();
        }

        public async Task DownloadObjectAsync(
            string bucketName, string path,
            Stream downloadInto,
            CancellationToken cancellationToken = default)
        {
            EnsureStorageClient();

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = path
            };
            GetObjectResponse response = await _storageClient.GetObjectAsync(request, cancellationToken);
            await response.ResponseStream.CopyToAsync(downloadInto);
        }




        private static string GetParentKey(S3Object obj, string delimiter = "/")
        {
            if (string.IsNullOrEmpty(delimiter)) delimiter = "/";
            string path = obj.Key;
            int lastIndexOfDelimiter = path.LastIndexOf(delimiter, StringComparison.Ordinal);
            if (lastIndexOfDelimiter == -1) return string.Empty;
            return path.Substring(0, lastIndexOfDelimiter + delimiter.Length);
        }

        private static string GetFirstLevelKey(S3Object obj, string delimiter = "/")
        {
            if (string.IsNullOrEmpty(delimiter)) delimiter = "/";
            string path = obj.Key;
            int indexOfDelimiter = path.IndexOf(delimiter, StringComparison.Ordinal);
            if (indexOfDelimiter == -1) return string.Empty;
            return path.Substring(0, indexOfDelimiter + delimiter.Length);
        }
    }
}
