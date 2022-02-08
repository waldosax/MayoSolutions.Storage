using System.Diagnostics;
using System.Linq;
using Google.Apis.Storage.v1.Data;

namespace MayoSolutions.Storage.Google
{
    [DebuggerDisplay(nameof(Name) + ": {" + nameof(Name) + ",nq}, " + nameof(Path) + ": {" + nameof(Path) + ",nq}")]
    internal class GoogleStorageFileWrapper : IFile
    {
        public Object FileObject { get; }

        //public string Identifier => FileObject.Id;
        public string Path => FileObject.Name;
        public string Name => FileObject.Name.Split('/').Last();
        public long? Size => (long?)FileObject.Size;

        public GoogleStorageFileWrapper(
            Object fileObject
            )
        {
            FileObject = fileObject;
        }
        
    }
}