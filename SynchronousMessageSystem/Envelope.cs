using System;

namespace SynchronousMessageSystem
{
    public class Envelope
    {
        public Actor Sender { get; }
        public Type ReceiverType { get; }
        public object Message { get; }

        internal Envelope(Actor sender, Type receiverType, object message)
        {
            Sender = sender;
            ReceiverType = receiverType;
            Message = message;
        }
    }
}