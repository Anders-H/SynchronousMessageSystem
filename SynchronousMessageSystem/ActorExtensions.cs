namespace SynchronousMessageSystem;

public static class ActorExtensions
{
    public static ActorAddress GetAddress(this Actor? me)
    {
        if (me == null)
            return new ActorAddress();

        return string.IsNullOrEmpty(me.ActorName)
            ? new ActorAddress(me.GetType())
            : new ActorAddress(me.GetType(), me.ActorName!);
    }
}