using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;

namespace MayoSolutions.Storage.AWS.S3
{
    public class S3StorageClient : IStorageClient
    {
        private readonly S3StorageClientConfig _config;

        public S3StorageClient(
            S3StorageClientConfig config
            )
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async ValueTask<IBucket> GetBucketAsync(string bucketIdentifier, CancellationToken cancellationToken = default)
        {
            BasicAWSCredentials basicCredentials =
                new BasicAWSCredentials(
                    _config.AccessKey,
                    _config.SecretKey);
            //BasicAWSCredentials basicCredentials =
            //    new BasicAWSCredentials("AKIAIIYG27E27PLQ6EWQ",
            //        "hr9+5JrS95zA5U9C6OmNji+ZOTR+w3vIXbWr3/td");

            var client = new AmazonS3Client(basicCredentials);
            var buckets = await client.ListBucketsAsync(cancellationToken);
            var bucket = buckets.Buckets.Find(b => b.BucketName == bucketIdentifier);
            var wrapper = new S3BucketWrapper(client, bucket);
            return wrapper;
        }
    }
}
