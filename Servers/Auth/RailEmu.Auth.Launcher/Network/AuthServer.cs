using Microsoft.Extensions.Logging;
using RailEmu.Network.Interfaces;
using RailEmu.Network.Network;

namespace RailEmu.Auth.Launcher.Network
{
    public class AuthServer : TcpServer
    {
        public AuthServer(IMessageManager messageManager,
            ILogger<AuthServer> logger) : base(logger, messageManager)
        {

        }

        protected override IClientData CreateNewClientData()
        {
            return null;
        }
    }
}
