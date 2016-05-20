using System;

namespace SynchronousMessageSystem
{
    public class ActorMatch
    {
        public Type MessageType { get; }
        public ReceiveProcess RreceiveProcess { get; }
        public ActorMatch(Type messageType, ReceiveProcess receiveProcess)
        {
            MessageType = messageType;
            RreceiveProcess = receiveProcess;
        }
        public bool IsMatch(object message) => MessageType == message.GetType();
    }
}
