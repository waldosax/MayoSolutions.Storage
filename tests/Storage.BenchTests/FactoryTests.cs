using MayoSolutions.Storage.AWS.S3;
using MayoSolutions.Storage.Google;
using MayoSolutions.Storage.Local;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Storage.BenchTests;

public class FactoryTests
{
    [Test]
    public void TestRegisterGoogleStorageClient()
    {
        var services = new ServiceCollection();
        services.AddGoogleStorage();
    }

    [Test]
    public void TestRegisterLocalStorageClient()
    {
        var services = new ServiceCollection();
        services.AddLocalStorage();
    }

    [Test]
    public void TestRegisterAmazonS3Client()
    {
        var services = new ServiceCollection();
        services.AddAmazonS3Storage();
    }

    [Test]
    public void TestRegisterMultipleClients()
    {
        var services = new ServiceCollection();
        services
            .AddAmazonS3Storage()
            .AddLocalStorage();
    }

    [Test]
    public void TestGetFactory()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<FactoryTests>()
            .Build();
            
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services
            .AddGoogleStorage()
            .AddAmazonS3Storage()
            .AddLocalStorage();
        var factory = new StorageClientFactory(services);

        var client = factory.GetClient("PlexSportsNetImageAssets", StorageClientFactory.StorageProviders.LocalStorage);
        Assert.That(client, Is.Not.Null);
        Assert.That(client.GetType(), Is.EqualTo(typeof(LocalStorageClient)));

        client = factory.GetClient("PlexSportsNetImageAssets", StorageClientFactory.StorageProviders.GoogleStorage);
        Assert.That(client, Is.Not.Null);
        Assert.That(client.GetType(), Is.EqualTo(typeof(GoogleStorageClient)));

        client = factory.GetClient("PlexSportsNetImageAssets", StorageClientFactory.StorageProviders.AmazonS3);
        Assert.That(client, Is.Not.Null);
        Assert.That(client.GetType(), Is.EqualTo(typeof(S3StorageClient)));
    }
}