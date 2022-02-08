using Amazon.S3.Model;

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
        public S3FolderShim(
            string path,
            S3Object folderObject
        )
        {
            Path = path;
            Name = folderObject.Key;
        }
        public S3FolderShim(
            string path,
            GetObjectResponse response
        )
        {
            Path = path;
            Name = response.Key;
        }
    }
}