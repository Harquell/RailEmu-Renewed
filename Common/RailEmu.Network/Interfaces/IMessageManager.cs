using RailEmu.Network.Network;

namespace RailEmu.Network.Interfaces
{
    public interface IMessageManager
    {
        void HandleMessage(byte[] message, TcpClient client);
        void Init();
    }
}