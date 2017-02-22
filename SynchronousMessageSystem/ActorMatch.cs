using System;

namespace SynchronousMessageSystem
{
    public class ActorMatch
    {
        public Type MessageType { get; }
        public ReceiveProcess ReceiveProcess { get; }
        public ActorMatch(Type messageType, ReceiveProcess receiveProcess)
        {
            MessageType = messageType;
            ReceiveProcess = receiveProcess;
        }
        public bool IsMatch(object message) => MessageType == message.GetType();
    }
}
