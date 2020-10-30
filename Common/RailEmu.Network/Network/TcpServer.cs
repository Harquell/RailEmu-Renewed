using Microsoft.Extensions.Logging;
using RailEmu.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace RailEmu.Network.Network
{
    public abstract class TcpServer : IDisposable
    {
        private Socket _socket;
        private IPAddress _iPAddress;
        public readonly List<TcpClient> Clients;
        private readonly int _port;
        private readonly ILogger<TcpServer> logger;
        private readonly IMessageManager messageManager;

        public bool IsRunning { get; private set; }

        protected TcpServer(ILogger<TcpServer> logger,
            IMessageManager messageManager)
        {
            //TODO: Pass ipaddress & port to configuration
            string ipAddress = "127.0.0.1";
            int port = 443;

            IPAddress.TryParse(ipAddress, out _iPAddress);
            _port = port;
            Clients = new List<TcpClient>();
            this.logger = logger;
            this.messageManager = messageManager;
        }

        public void Init()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(_iPAddress, _port));

            messageManager.Init();
        }

        public void Start()
        {
            _socket.Listen(0);
            BeginAccept();
            IsRunning = true;
            logger.LogInformation("Server started on {0}:{1}", _iPAddress, _port);
        }

        public void Stop()
        {
            IsRunning = false;
            _socket.Close();
        }

        private void BeginAccept()
        {
            _socket.BeginAccept(new AsyncCallback(AcceptCallBack), _socket);
        }

        private void AcceptCallBack(IAsyncResult result)
        {
            Socket socket = _socket.EndAccept(result);

            //TODO: Faire une méthode abstraite qui retourne un IClientData pour un nouvel utilisateur
            IClientData clientData = CreateNewClientData();

            //TODO: Créer l'objet client
            TcpClient client = null;
            logger.LogDebug("Nouveau client");
            Clients.Add(client);
            client.Init();

            BeginAccept();
        }

        protected abstract IClientData CreateNewClientData();

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            IsRunning = false;
            if (!disposedValue)
            {
                if (disposing)
                {
                    _socket?.Dispose();
                }

                _iPAddress = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TcpServer()
        {
            Dispose(false);
        }

        #endregion IDisposable Support
    }
}
