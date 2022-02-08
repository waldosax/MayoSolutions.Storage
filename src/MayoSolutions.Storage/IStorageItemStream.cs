using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MayoSolutions.Storage
{
    public interface IStorageItemStream
    {
        Task DownloadObjectAsync(string bucketName, string path, Stream downloadInto, CancellationToken cancellationToken = default);
        //Task UploadObjectAsync(string bucketName, string path, Stream contents, CancellationToken cancellationToken = default);
    }
}