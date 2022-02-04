using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MayoSolutions.Storage
{
    public interface IStorageClient
    {
        ValueTask<IBucket> GetBucketAsync(string bucketIdentifier, CancellationToken cancellationToken = default);
    }
}
