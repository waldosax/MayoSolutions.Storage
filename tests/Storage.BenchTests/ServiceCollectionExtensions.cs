using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MayoSolutions.Storage;
using MayoSolutions.Storage.AWS.S3;
using MayoSolutions.Storage.Google;
using MayoSolutions.Storage.Local;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Storage.BenchTests
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection Clone(this IServiceCollection services)
        {
            var clone = new ServiceCollection();
            foreach (var svcDescriptor in services)
            {
                ((ICollection<ServiceDescriptor>)clone).Add(svcDescriptor);
            }
            return clone;
        }
    }

    internal static class BootstrapperExtensions
    {
        public static IServiceCollection AddGoogleStorage(this IServiceCollection services)
        {
            services
                .AddTransient<IGoogleStorageClient, GoogleStorageClient>()
                .TryAddSingleton<StorageClientFactory>();
            return services;
        }
        public static IServiceCollection AddLocalStorage(this IServiceCollection services)
        {
            services
                .AddTransient<ILocalStorageClient, LocalStorageClient>()
                .TryAddSingleton<StorageClientFactory>();
            return services;
        }
        public static IServiceCollection AddAmazonS3Storage(this IServiceCollection services)
        {
            services
                .AddTransient<IS3StorageClient, S3StorageClient>()
                .TryAddSingleton<StorageClientFactory>();
            return services;
        }
    }

    internal static class TestBootstrapperExtensions
    {
        public static IStorageClient GetClient(this IServiceCollection services, string configSectionName)
        {
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<StorageClientFactory>()!.GetClient(configSectionName);
        }
    }

    internal class StorageClientFactory
    {

        public static class StorageProviders
        {
            public const string AmazonS3 = "AmazonS3";
            public const string GoogleStorage = "GoogleStorage";
            public const string LocalStorage = "LocalFileStorage";
        }


        private readonly IServiceCollection _serviceCollection;

        public StorageClientFactory(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }


        internal IStorageClient GetClient(string configSectionName, string provider)
        {
            try
            {
                var serviceProvider = _serviceCollection.BuildServiceProvider();
                var config = serviceProvider.GetService<IConfiguration>();
                var storageConfig = config!.GetSection(configSectionName);

                var cloned = _serviceCollection.Clone();

                switch (provider)
                {
                    case StorageProviders.GoogleStorage:
                        cloned.AddSingleton(storageConfig.GetSection(provider).Get<GoogleStorageClientConfig>());
                        serviceProvider = cloned.BuildServiceProvider();
                        return serviceProvider.CreateScope().ServiceProvider.GetService<IGoogleStorageClient>()!;
                    case StorageProviders.LocalStorage:
                        cloned.AddSingleton(storageConfig.GetSection(provider).Get<LocalStorageClientConfig>());
                        serviceProvider = cloned.BuildServiceProvider();
                        return serviceProvider.CreateScope().ServiceProvider.GetService<ILocalStorageClient>()!;
                    case StorageProviders.AmazonS3:
                        cloned.AddSingleton(storageConfig.GetSection(provider).Get<S3StorageClientConfig>());
                        serviceProvider = cloned.BuildServiceProvider();
                        return serviceProvider.CreateScope().ServiceProvider.GetService<IS3StorageClient>()!;
                    default:
                        throw new ArgumentException($"No {provider} storage provider configured at {configSectionName}.", nameof(configSectionName));
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Invalid storage provider configured at {configSectionName}.", nameof(configSectionName), e);
            }
        }

        public IStorageClient GetClient(string configSectionName)
        {
                var serviceProvider = _serviceCollection.BuildServiceProvider();
                var config = serviceProvider.GetService<IConfiguration>();
                var storageConfig = config!.GetSection(configSectionName);
                var provider = storageConfig!.GetValue<string>("StorageProvider");

                return GetClient(configSectionName, provider);
        }
    }
}
