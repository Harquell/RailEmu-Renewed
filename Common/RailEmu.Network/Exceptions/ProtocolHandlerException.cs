using System;

namespace RailEmu.Network.Exceptions
{
    [Serializable]
    public class ProtocolHandlerException : Exception
    {
        public uint MessageId { get; set; }

        public ProtocolHandlerException() { }
        public ProtocolHandlerException(string message) : base(message) { }
        public ProtocolHandlerException(string message, Exception inner) : base(message, inner) { }
        public ProtocolHandlerException(string message, uint messageId) : base(message)
        {
            this.MessageId = messageId;
        }
        protected ProtocolHandlerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
