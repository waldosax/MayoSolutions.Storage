using System.Diagnostics;
using System.IO;

namespace MayoSolutions.Storage.Local
{
    [DebuggerDisplay(nameof(Name) + ": {" + nameof(Name) + ",nq}, " + nameof(Path) + ": {" + nameof(Path) + ",nq}")]
    internal class LocalStorageFileWrapper : IFile
    {
        public string FullPath { get; protected set; }
        public string Path { get; protected set; }
        public string Name => new FileInfo(FullPath).Name;
        public long? Size => new FileInfo(FullPath).Length;

        public LocalStorageFileWrapper(
            string fullPath,
            string path
        )
        {
            FullPath = fullPath;
            Path = path;
        }

    }
}