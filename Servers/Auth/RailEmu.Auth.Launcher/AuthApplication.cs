using Microsoft.Extensions.Logging;
using RailEmu.Auth.Database.Interfaces;
using RailEmu.Network.Protocol;
using System;
using System.Net;
using System.Net.Sockets;

namespace RailEmu.Auth.Launcher
{
    public class AuthApplication
    {
        private readonly IAccountRepository repository;
        private readonly ILogger<AuthApplication> logger;

        public AuthApplication(IAccountRepository repository,
            ILogger<AuthApplication> logger)
        {
            this.repository = repository;
            this.logger = logger;
            logger.LogDebug("Initializing AuthApplication");
            logger.LogInformation("AuthApplication initialized");
        }

        public void Run()
        {
            //Main function of auth server
            throw new NotImplementedException();
        }
    }
}
