using Amazon.S3;
using Amazon.S3.Model;

namespace MayoSolutions.Storage.AWS.S3
{
    internal class S3ObjectFolderWrapper : S3ClientWrapper, IFolder
    {
        public S3Object FolderObject { get; }

        public string Identifier => FolderObject.Key;
        public string Name => FolderObject.Key;

        public S3ObjectFolderWrapper(
            AmazonS3Client client,
            S3Bucket bucket,
            S3Object folderObject
        )
            : base(client, bucket, folderObject.Key) // TODO: Is this just the top-level name or the relative multi-level path?
        {
            FolderObject = folderObject;
        }
    }
}