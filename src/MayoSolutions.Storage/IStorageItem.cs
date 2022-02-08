namespace MayoSolutions.Storage
{
    public interface IStorageItem
    {
        string Name { get; }
        string Path { get; }
    }
}