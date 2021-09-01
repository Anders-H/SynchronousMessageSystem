using System.ComponentModel.Design;

namespace SynchronousMessageSystem
{
    public static class ActorExtensions
    {
        public static ActorAddress GetAddress(this Actor? me)
        {
            if (me == null)
                return new ActorAddress();

            if (string.IsNullOrEmpty(me.ActorName))
                return new ActorAddress(me.GetType());

            return new ActorAddress(me.GetType(), me.ActorName!);
        }
    }
}