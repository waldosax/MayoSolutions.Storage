using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;

namespace MayoSolutions.Storage.Google
{
    public class GoogleStorageClient: IStorageClient
    {
        

        public async ValueTask<IBucket> GetBucketAsync(string bucketIdentifier, CancellationToken cancellationToken = default)
        {
            // Split out project and bucket.
            var pieces = bucketIdentifier.Split(new[] { "::" }, StringSplitOptions.None);
            var projectId = pieces[0];
            var bucketName = pieces[1];
            return await _GetBucket(projectId, bucketName, cancellationToken);
        }

        private async ValueTask<IBucket> _GetBucket(
            string projectId,
            string bucketName,
            CancellationToken cancellationToken
            )
        {
            var storage = await StorageClient.CreateAsync();
            var bucket = await storage.GetBucketAsync(bucketName, null, cancellationToken);
            var wrapper = new GoogleStorageBucketWrapper(storage, bucket);
            return wrapper;
        }


    }
}
