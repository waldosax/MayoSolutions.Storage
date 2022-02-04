using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

namespace MayoSolutions.Storage.Google
{
    internal class GoogleStorageFolderWrapper : GoogleStorageClientWrapper, IFolder
    {
        public Object FolderObject { get; }

        public string Identifier => FolderObject.Id;
        public string Name => FolderObject.Name;

        public GoogleStorageFolderWrapper(
            StorageClient client, 
            Bucket bucket, 
            Object folderObject
        )
            : base(client, bucket, folderObject.Name) // TODO: Is this just the top-level name or the relative multi-level path?
        {
            FolderObject = folderObject;
        }
    }
}