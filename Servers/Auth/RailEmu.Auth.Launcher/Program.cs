using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RailEmu.Auth.Database.Interfaces;
using RailEmu.Auth.Database.Repositories;
using RailEmu.Auth.Launcher.Network;
using RailEmu.Core.Providers;
using RailEmu.Database.Services;
using RailEmu.Network.Interfaces;
using RailEmu.Network.Managers;
using RailEmu.Network.Network;
using System;

namespace RailEmu.Auth.Launcher
{
    public static class Program
    {
        private static ServiceProvider _serviceProvider;
            
        public static void Main(string[] args)
        {
            Console.WriteLine("Initializing IOC...");
            InitIoc();
            IServiceScope scope = _serviceProvider.CreateScope();
            try
            {
                scope.ServiceProvider.GetRequiredService<AuthApplication>().Run();
            }
            catch (Exception e)
            {
                ILogger<AuthApplication> logger = _serviceProvider.GetService<ILogger<AuthApplication>>();
                logger.LogCritical(-1, e, e.ToString());
            }

            DisposeServices();
        }

        private static void InitIoc()
        {
            var collection = new ServiceCollection();

            InitLogger(collection, LogLevel.Trace);
            InitRepositories(collection);
            InitConcretes(collection);
            InitManagers(collection);

            _serviceProvider = collection.BuildServiceProvider(true);
        }

        private static void InitConcretes(ServiceCollection collection)
        {
            collection
                .AddSingleton<MongoDatabaseManager>()
                .AddSingleton<AuthApplication>()
                .AddSingleton<TcpServer, AuthServer>();
        }

        private static void InitLogger(ServiceCollection collection, LogLevel minLogLevel)
        {
            collection
                .AddLogging(x => x.AddProvider(new LoggerProvider(minLogLevel))
                .SetMinimumLevel(minLogLevel));
        }

        private static void InitRepositories(IServiceCollection collection)
        {
            collection
                .AddSingleton<IAccountRepository, MongoAccountRepository>();
        }

        private static void InitManagers(IServiceCollection collection)
        {
            collection
                .AddSingleton<IMessageManager, MessageManager>();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
