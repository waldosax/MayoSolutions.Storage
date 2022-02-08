namespace MayoSolutions.Storage.AWS.S3
{
    internal class S3FolderShim : IFolder
    {
        public string Name { get; protected set; }
        public string Path { get; protected set; }

        public S3FolderShim(
            string path,
            string name
        )
        {
            Path = path;
            Name = name;
        }
    }
}