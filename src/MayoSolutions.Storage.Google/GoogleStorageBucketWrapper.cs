using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

namespace MayoSolutions.Storage.Google
{
    internal class GoogleStorageBucketWrapper : GoogleStorageClientWrapper, IBucket
    {
        public string Identifier => Bucket.Id;
        public string Name => Bucket.Name;

        public GoogleStorageBucketWrapper(
            StorageClient client,
            Bucket bucket
        )
            : base(client, bucket, null)
        {
        }
    }
}