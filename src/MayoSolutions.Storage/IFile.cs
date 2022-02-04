namespace MayoSolutions.Storage
{
    public interface IFile : IStorageItem
    {
        long? Size { get; }
    }
}