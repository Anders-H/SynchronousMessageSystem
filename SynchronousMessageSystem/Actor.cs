using System;

namespace SynchronousMessageSystem
{
    public abstract class Actor
    {
        public ActorMatchList MatchList { get; } = new ActorMatchList();
        internal ActorSystem ActorSystem { get; set; }
        public abstract void Other(Actor sender, ActorMatch address, object message);
        public void Talk(Actor receiver, object message) => ActorSystem.Talk(this, receiver, receiver.GetType(), message);
        public void Talk(Type receiverType, object message)
        {
            var receiver = ActorSystem.GetActor(receiverType);
            if (receiver != null)
                Talk(receiver, message);
            else
                ActorSystem.Undelivered.Add(new Envelope(this, receiverType, message));
        }
    }
}
