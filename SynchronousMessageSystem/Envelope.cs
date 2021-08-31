using System;

namespace SynchronousMessageSystem
{
    public class Envelope
    {
        public Actor Sender { get; }
        public ActorAddress? ActorAddress { get; }
        public object Message { get; }

        internal Envelope(Actor sender, object message)
        {
            Sender = sender;
            ActorAddress = null;
            Message = message;
        }

        internal Envelope(Actor sender, string receiverName, object message)
        {
            Sender = sender;
            ActorAddress = new ActorAddress(receiverName);
            Message = message;
        }

        internal Envelope(Actor sender, Type receiverType, object message)
        {
            Sender = sender;
            ActorAddress = new ActorAddress(receiverType);
            Message = message;
        }

        internal Envelope(Actor sender, Type receiverType, string receiverName, object message)
        {
            Sender = sender;
            ActorAddress = new ActorAddress(receiverType, receiverName);
            Message = message;
        }

        internal Envelope(Actor sender, ActorAddress actorAddress, object message)
        {
            Sender = sender;
            ActorAddress = actorAddress;
            Message = message;
        }
    }
}