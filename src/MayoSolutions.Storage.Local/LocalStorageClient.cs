using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MayoSolutions.Storage.Local
{
    public class LocalStorageClient : IStorageClient
    {
        private readonly LocalStorageClientConfig _config;


        public LocalStorageClient(
            LocalStorageClientConfig config
            )
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async ValueTask<IBucket> GetBucketAsync(string bucketIdentifier, CancellationToken cancellationToken = default)
        {
            string fullPath = Path.Combine(_config.StoragePath, bucketIdentifier);
            if (Directory.Exists(fullPath))
                return new LocalStorageBucketWrapper(_config.StoragePath, bucketIdentifier);
            throw new IOException("Directory not found.");
        }
    }
}
