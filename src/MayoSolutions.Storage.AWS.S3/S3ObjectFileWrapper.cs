using Amazon.S3.Model;

namespace MayoSolutions.Storage.AWS.S3
{
    internal class S3ObjectFileWrapper : IFile
    {
        public S3Object FileObject { get; }

        public string BucketName => FileObject.BucketName;
        public string Identifier => FileObject.Key;
        public string Name => FileObject.Key;
        public long? Size => FileObject.Size;

        public S3ObjectFileWrapper(S3Object fileObject)
        {
            FileObject = fileObject;
        }
    }
}