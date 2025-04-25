namespace SynchronousMessageSystem;

public delegate void ReceiveProcess(Actor sender, ActorMatch address, object message);