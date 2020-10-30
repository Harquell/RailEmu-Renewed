using System;

namespace RailEmu.Network
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    sealed class MessageHandlerAttribute : Attribute
    {
        public Type MessageType { get; }

        public MessageHandlerAttribute(Type messageType)
        {
            this.MessageType = messageType;
        }
    }
}
