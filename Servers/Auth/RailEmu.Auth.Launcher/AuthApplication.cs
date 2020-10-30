using Microsoft.Extensions.Logging;
using RailEmu.Auth.Database.Interfaces;
using RailEmu.Network.Network;
using System;

namespace RailEmu.Auth.Launcher
{
    public class AuthApplication
    {
        private readonly IAccountRepository repository;
        private readonly ILogger<AuthApplication> logger;

        public AuthApplication(IAccountRepository repository,
            ILogger<AuthApplication> logger,
            TcpServer tcpServer)
        {
            this.repository = repository;
            this.logger = logger;
            TcpServer = tcpServer;
            logger.LogDebug("Initializing AuthApplication");
            logger.LogInformation("AuthApplication initialized");
        }

        public TcpServer TcpServer { get; }

        public void Run()
        {
            TcpServer.Init();
            TcpServer.Start();
            Console.ReadKey();
        }
    }
}
