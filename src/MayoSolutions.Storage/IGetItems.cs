using System.Threading;
using System.Threading.Tasks;

namespace MayoSolutions.Storage
{
    public interface IGetItems
    {
        ValueTask<IFolder[]> GetFoldersAsync(string bucketName, string path, CancellationToken cancellationToken = default);
        ValueTask<IFile> GetFileAsync(string bucketName, string path, CancellationToken cancellationToken = default);
        ValueTask<IFile[]> GetFilesAsync(string bucketName, string path, CancellationToken cancellationToken = default);
    }
}