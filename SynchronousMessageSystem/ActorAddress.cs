using System;

namespace SynchronousMessageSystem
{
    public class ActorAddress
    {
        public Type? ReceiverType { get; }
        public string? ReceiverName { get; }

        public ActorAddress()
        {
            ReceiverType = null;
            ReceiverName = null;
        }

        public ActorAddress(Type receiverType)
        {
            ReceiverType = receiverType;
            ReceiverName = null;
        }

        public ActorAddress(string receiverName)
        {
            ReceiverType = null;
            ReceiverName = receiverName;
        }

        public ActorAddress(Type receiverType, string receiverName)
        {
            ReceiverType = receiverType;
            ReceiverName = receiverName;
        }

        public bool IsUsable =>
            !(ReceiverType == null && string.IsNullOrEmpty(ReceiverName));
    }
}