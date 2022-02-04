namespace MayoSolutions.Storage
{
    public interface IStorageItem
    {
        string BucketName { get; }
        string Identifier { get; }
        string Name { get; }
    }
}