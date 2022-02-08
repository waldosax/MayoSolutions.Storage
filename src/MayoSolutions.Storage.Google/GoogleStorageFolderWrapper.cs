using System.Diagnostics;
using System.Linq;
using Google.Apis.Storage.v1.Data;

namespace MayoSolutions.Storage.Google
{
    [DebuggerDisplay(nameof(Name) + ": {" + nameof(Name) + ",nq}, " + nameof(Path) + ": {" + nameof(Path) + ",nq}")]
    internal class GoogleStorageFolderWrapper : IFolder
    {
        public Object FolderObject { get; }

        //public string Identifier => FolderObject.Id;
        public string Path { get; protected set; }
        public string Name => FolderObject.Name;

        public GoogleStorageFolderWrapper(
            string path,
            Object folderObject
        )
        {
            Path = path;
            FolderObject = folderObject;
        }
    }
}