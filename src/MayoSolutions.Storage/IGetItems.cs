using System.Threading;
using System.Threading.Tasks;

namespace MayoSolutions.Storage
{
    public interface IGetItems
    {
        ValueTask<IFolder> GetFolderAsync(string name, CancellationToken cancellationToken = default);
        ValueTask<IFolder[]> GetFoldersAsync(string name, CancellationToken cancellationToken = default);
        ValueTask<IFile> GetFileAsync(string name, CancellationToken cancellationToken = default);
        ValueTask<IFile[]> GetFilesAsync(string name, CancellationToken cancellationToken = default);
    }
}