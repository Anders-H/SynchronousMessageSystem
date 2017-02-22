using System;
using System.Linq;

namespace SynchronousMessageSystem
{
    public abstract class Actor
    {
        public ActorMatchList MatchList { get; } = new ActorMatchList();
        protected internal ActorSystem ActorSystem { get; set; }
        public abstract void Other(Actor sender, ActorMatch address, object message);
        public void Talk(Actor receiver, object message) => ActorSystem.Talk(this, receiver, receiver.GetType(), message);
        public void Talk(Type receiverType, object message)
        {
            var receivers = ActorSystem.GetActors(receiverType);
            var enumerable = receivers as Actor[] ?? receivers.ToArray();
            if (enumerable.Any())
            {
                foreach (var receiver in enumerable)
                    Talk(receiver, message);
                return;
            }
            ActorSystem.Undelivered.Add(new Envelope(this, receiverType, message));
        }
        public void Talk(object message) => ActorSystem.Talk(this, message);
    }
}
