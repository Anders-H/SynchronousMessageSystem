using System;
using System.Collections.Generic;
using System.Linq;

namespace SynchronousMessageSystem
{
    public class ActorSystem
    {
        private List<Actor> Actors { get; } = new List<Actor>();
        internal EnvelopeList Undelivered { get; } = new EnvelopeList();
        public void AddActor(Actor actor)
        {
            actor.ActorSystem = this;
            Actors.Add(actor);
        }

        public void ResendAllUndelivered()
        {
            lock (Undelivered)
            {
                var toDeliver = new EnvelopeList();
                toDeliver.AddRange(Undelivered);
                Undelivered.Clear();
                toDeliver.ForEach(x => Talk(x.Sender, GetActor(x.ReceiverType), x.ReceiverType, x.Message));
            }
        }

        public void RemoveActor(Actor actor) => Actors.Remove(actor);

        internal void RemoveActor(Type actorType)
        {
            lock (Actors)
            {
                bool again;
                do
                {
                    again = false;
                    foreach (var x in Actors)
                    {
                        if (x.GetType() != actorType)
                            continue;
                        again = true;
                        RemoveActor(x);
                        break;
                    }
                } while (again);
            }
        }

        internal void Talk(Actor sender, Actor receiver, Type receiverType, object message)
        {
            if (receiver == null)
            {
                Undelivered.Add(new Envelope(sender, receiverType, message));
                return;
            }
            lock (receiver.MatchList)
            {
                //Deliver to all subscribing functions.
                var delivered = false;
                if (receiver.MatchList.Count > 0)
                    for (var i = 0; i < receiver.MatchList.Count; i++)
                        if (receiver.MatchList[i].IsMatch(message))
                        {
                            receiver.MatchList[i].RreceiveProcess(sender, receiver.MatchList[i], message);
                            delivered = true;
                        }
                if (!delivered)
                    receiver.Other(sender, null, message);
            }
        }

        public Actor GetActor(Type actorType) => Actors.FirstOrDefault(x => x.GetType() == actorType);
        public T GetActor<T>() where T : Actor => (T) Actors.FirstOrDefault(x => x.GetType() == typeof(T)); 
    }
}
