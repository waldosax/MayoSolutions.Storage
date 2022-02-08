namespace MayoSolutions.Storage
{
    public interface IStorageClient : IGetItems, IStorageItemStream
    {
        // TODO: Re-implement
        //ValueTask<IBucket> GetBucketAsync(string bucketIdentifier, CancellationToken cancellationToken = default);
    }
}
