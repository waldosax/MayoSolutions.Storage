using Google.Apis.Storage.v1.Data;

namespace MayoSolutions.Storage.Google
{
    internal class GoogleStorageFileWrapper : IFile
    {
        public Object FileObject { get; }

        public string BucketName => FileObject.Bucket;
        public string Identifier => FileObject.Id;
        public string Name => FileObject.Name;
        public long? Size => (long?)FileObject.Size;

        public GoogleStorageFileWrapper(Object fileObject)
        {
            FileObject = fileObject;
        }
    }
}