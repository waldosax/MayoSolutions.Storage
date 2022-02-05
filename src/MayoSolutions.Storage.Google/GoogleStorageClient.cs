using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Newtonsoft.Json;

namespace MayoSolutions.Storage.Google
{
    public class GoogleStorageClient : IStorageClient
    {
        private readonly GoogleStorageClientConfig _config;

        public GoogleStorageClient(
            GoogleStorageClientConfig config
            )
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }



        public async ValueTask<IBucket> GetBucketAsync(string bucketIdentifier, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(_config);
            GoogleCredential creds = GoogleCredential.FromJson(json);
            var storage = await StorageClient.CreateAsync(creds);
            var bucket = await storage.GetBucketAsync(bucketIdentifier, null, cancellationToken);
            var wrapper = new GoogleStorageBucketWrapper(storage, bucket);
            return wrapper;
        }
        


    }
}
