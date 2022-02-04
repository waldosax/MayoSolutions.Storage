using Amazon.S3;
using Amazon.S3.Model;

namespace MayoSolutions.Storage.AWS.S3
{
    internal class S3BucketWrapper : S3ClientWrapper, IBucket
    {
        public string Identifier => BucketName;
        public string Name => BucketName;

        public S3BucketWrapper(
            AmazonS3Client client,
            S3Bucket bucket
        )
            : base(client, bucket, null)
        {
        }
    }
}