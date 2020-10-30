using Microsoft.Extensions.Logging;
using RailEmu.Network.Interfaces;
using RailEmu.Protocol.IO;
using RailEmu.Protocol.Messages;
using System;
using System.Net;
using System.Net.Sockets;

namespace RailEmu.Network.Network
{
    public abstract class TcpClient
    {
        private readonly Socket _socket;
        private readonly IMessageManager messageManager;
        private readonly ILogger<TcpClient> logger;
        private byte[] _buffer;

        public IClientData ClientData { get; private set; }
        public EndPoint EndPoint => _socket.RemoteEndPoint;

        protected TcpClient(IMessageManager messageManager,
            ILogger<TcpClient> logger)
        {
            //_socket = socket;
            //ClientData = data;
            this.messageManager = messageManager;
            this.logger = logger;
        }

        public void Init()
        {
            BeginReceive();
        }

        private void BeginReceive()
        {
            _buffer = new byte[Constants.BUFFER_SIZE];
            _socket.BeginReceive(_buffer, 0,  Constants.BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveCallback), this);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int size = _socket.EndReceive(result);
                byte[] buffer = new byte[size];
                Array.Copy(_buffer, buffer, size);

                messageManager.HandleMessage(buffer, this);

                BeginReceive();
            }
            catch (Exception)
            {
                Dispose();
            }
        }

        public void SendMessage(Message message)
        {
            using BigEndianWriter writer = new BigEndianWriter();
            message.Serialize(writer);
            var buffer = writer.Data;
            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendMessageCallBack), _socket);
        }

        private void SendMessageCallBack(IAsyncResult result)
        {
            try
            {
                Socket socket = (Socket)result.AsyncState;
                socket.EndSend(result);
            }
            catch (Exception)
            {
                _socket.Dispose();
            }
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _socket.Dispose();
                }

                logger.LogDebug("Client il est mort");
                _buffer = null;

                disposedValue = true;
            }
        }

        ~TcpClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}
