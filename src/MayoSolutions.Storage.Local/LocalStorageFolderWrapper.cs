using System.Diagnostics;
using System.IO;

namespace MayoSolutions.Storage.Local
{
    [DebuggerDisplay(nameof(Name) + ": {" + nameof(Name) + ",nq}, " + nameof(Path) + ": {" + nameof(Path) + ",nq}")]
    internal class LocalStorageFolderWrapper : IFolder
    {
        public string FullPath { get; protected set; }
        public string Path { get; protected set; }
        public string Name => new DirectoryInfo(FullPath).Name + "/";

        public LocalStorageFolderWrapper(
            string fullPath,
            string path
        )
        {
            FullPath = fullPath;
            Path = path;
        }



        


    }
}