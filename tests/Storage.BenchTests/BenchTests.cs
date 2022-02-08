using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MayoSolutions.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Storage.BenchTests
{
    internal class TestContext
    {
        public IConfiguration Configuration { get; }
        public IServiceCollection Services { get; }
        public StorageClientFactory Factory { get; }

        public TestContext()
        {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<TestContext>()
                .Build();

            Services = new ServiceCollection();
            Services.AddSingleton<IConfiguration>(Configuration);
            Services
                .AddGoogleStorage()
                .AddAmazonS3Storage()
                .AddLocalStorage();

            Factory = new StorageClientFactory(Services);
        }
    }
    public class Tests
    {
        private const string ConfigKey = "PlexSportsNetImageAssets";

        [Test]
        public async Task Test1()
        {
            // Arrange
            CancellationToken cancellationToken = new CancellationToken();
            var provider = StorageClientFactory.StorageProviders.LocalStorage;
            var ctx = new TestContext();
            var client = ctx.Factory.GetClient(ConfigKey, provider);
            var bucketName = ctx.Configuration.GetValue<string>($"{ConfigKey}:BucketIdentifier");
            
            var childFolders = await client.GetFoldersAsync(bucketName, "", cancellationToken);
            var files = await client.GetFilesAsync(bucketName, "team-logos/NHL/winnipeg-jets", cancellationToken);
            var file = await client.GetFileAsync(bucketName, "posters/NBA/2021/nba-poster-2021.png", cancellationToken);
            using (var ms = new MemoryStream())
            {
                await client.DownloadObjectAsync(bucketName, file.Path, ms, cancellationToken);
                ms.Position = 0L;
                var signature = new byte[4];
                await ms.ReadAsync(signature, 0, 4, cancellationToken);
                var pngSig = new byte[] { 0x89, 0x50, 0x4e, 0x47 };
                Assert.That(signature, Is.EqualTo(pngSig));
            }
        }

        [Test]
        public async Task Test2()
        {
            // Arrange
            CancellationToken cancellationToken = new CancellationToken();
            var provider = StorageClientFactory.StorageProviders.GoogleStorage;
            var ctx = new TestContext();
            var client = ctx.Factory.GetClient(ConfigKey, provider);
            var bucketName = ctx.Configuration.GetValue<string>($"{ConfigKey}:BucketIdentifier");
            
            var childFolders = await client.GetFoldersAsync(bucketName, "", cancellationToken);
            var files = await client.GetFilesAsync(bucketName, "team-logos/NHL/winnipeg-jets", cancellationToken);
            var file = await client.GetFileAsync(bucketName, "posters/NBA/2021/nba-poster-2021.png", cancellationToken);
            using (var ms = new MemoryStream())
            {
                await client.DownloadObjectAsync(bucketName, file.Path, ms, cancellationToken);
                ms.Position = 0L;
                var signature = new byte[4];
                await ms.ReadAsync(signature, 0, 4, cancellationToken);
                var pngSig = new byte[] { 0x89, 0x50, 0x4e, 0x47 };
                Assert.That(signature, Is.EqualTo(pngSig));
            }
        }

        [Test]
        public async Task Test3()
        {
            // Arrange
            CancellationToken cancellationToken = new CancellationToken();
            var provider = StorageClientFactory.StorageProviders.AmazonS3;
            var ctx = new TestContext();
            var client = ctx.Factory.GetClient(ConfigKey, provider);
            var bucketName = ctx.Configuration.GetValue<string>($"{ConfigKey}:BucketIdentifier");
            
            var childFolders = await client.GetFoldersAsync(bucketName, "", cancellationToken);
            var files = await client.GetFilesAsync(bucketName, "team-logos/NHL/winnipeg-jets", cancellationToken);
            var file = await client.GetFileAsync(bucketName, "posters/NBA/2021/nba-poster-2021.png", cancellationToken);
            using (var ms = new MemoryStream())
            {
                await client.DownloadObjectAsync(bucketName, file.Path, ms, cancellationToken);
                ms.Position = 0L;
                var signature = new byte[4];
                await ms.ReadAsync(signature, 0, 4, cancellationToken);
                var pngSig = new byte[] { 0x89, 0x50, 0x4e, 0x47 };
                Assert.That(signature, Is.EqualTo(pngSig));
            }
        }
    }
}