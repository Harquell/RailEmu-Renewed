using Microsoft.Extensions.Logging;
using RailEmu.Network.Interfaces;
using RailEmu.Network.Network;
using RailEmu.Protocol.IO;
using RailEmu.Protocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RailEmu.Network.Managers
{
    public class MessageManager : IMessageManager
    {
        public delegate void HandleMessageDelegate(Message message, IClientData client);

        private Dictionary<uint, Type> _messageTypes;
        private Dictionary<Type, HandleMessageDelegate> _messageHandlers;
        private readonly ILogger<MessageManager> logger;

        public MessageManager(ILogger<MessageManager> logger)
        {
            _messageTypes = new Dictionary<uint, Type>();
            _messageHandlers = new Dictionary<Type, HandleMessageDelegate>();
            this.logger = logger;
        }

        public void Init()
        {
            try
            {
                logger.LogInformation("Init MessageManager");
                InitMessageTypes();
                InitMessageHandlers();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        private void InitMessageTypes()
        {
            logger.LogInformation("Initializing message types");
            Assembly messageAssembly = typeof(Message).Assembly;
            _messageTypes = messageAssembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(Message)))
                .ToDictionary(t => (uint)t.GetField("Id").GetValue(null), t => t);
            logger.LogDebug("{0} messages founded", _messageTypes.Count);
        }

        private void InitMessageHandlers()
        {
            logger.LogInformation("Initializing message handlers");
            Assembly assembly = Assembly.GetEntryAssembly();

            _messageHandlers = assembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes<MessageHandlerAttribute>().Any())
                .ToDictionary(
                    m => m.GetCustomAttribute<MessageHandlerAttribute>().MessageType,
                    m => (HandleMessageDelegate)Delegate.CreateDelegate(typeof(HandleMessageDelegate), m)
                );
            logger.LogDebug("{0} handlers founded", _messageHandlers.Count);
        }

        public void HandleMessage(byte[] message, TcpClient client)
        {
            using BigEndianReader reader = new BigEndianReader(message);
            ushort packetId = reader.ReadUShort();
            if (!_messageTypes.ContainsKey(packetId))
            {
                logger.LogError("Received PacketId({0}) from {1} doesn't correspond to a message type", packetId, client.EndPoint);
                return;
            }
            Type packetType = _messageTypes[packetId];
            if (!_messageHandlers.ContainsKey(packetType))
            {
                logger.LogError("Received PacketId({0}) from {1} not handled", packetId, client.EndPoint);
                return;
            }
            Message msg = Activator.CreateInstance(_messageTypes[packetId]) as Message;
            msg.Deserialize(reader);

            logger.LogDebug("new packet data [{0}]{1}", packetId, msg.GetType().Name);

            _messageHandlers[packetType](msg, client.ClientData);
        }
    }
}
