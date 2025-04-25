using System;
using System.Linq;

namespace SynchronousMessageSystem;

public abstract class Actor
{
    public string? ActorName { get; set; }
    public object? ActorState { get; protected set; }
    public ActorMatchList MatchList { get; }
    protected internal ActorSystem? ActorSystem { get; set; }
    public abstract void Other(Actor sender, ActorMatch? address, object message);

    protected Actor() : this(null, null)
    {
    }

    protected Actor(string? actorName, object? actorState)
    {
        MatchList = new ActorMatchList();
        ActorName = actorName;
        ActorState = actorState;
    }

    public ActorAddress GetAddress() =>
        string.IsNullOrEmpty(ActorName)
            ? new ActorAddress(GetType())
            : new ActorAddress(GetType(), ActorName);

    public void Talk(Actor receiver, object message) =>
        ActorSystem!.Talk(
            this, // Who is talking?
            receiver, // Talking to who?
            receiver.GetAddress(), // If object does not exist, try to figure out the address for later use.
            message // What is being said?
        );
        
    public void Talk(Type receiverType, object message)
    {
        var receivers = ActorSystem!.GetActors(receiverType);
        var enumerable = receivers as Actor[] ?? receivers.ToArray();
            
        if (enumerable.Any())
        {
            foreach (var receiver in enumerable)
                Talk(receiver, message);
                
            return;
        }

        ActorSystem.Undelivered.Add(new Envelope(this, receiverType, message));
    }

    public void Talk(string actorName, object message)
    {
        var receivers = ActorSystem!.GetActors(actorName);
        var enumerable = receivers as Actor[] ?? receivers.ToArray();

        if (enumerable.Any())
        {
            foreach (var receiver in enumerable)
                Talk(receiver, message);

            return;
        }

        ActorSystem.Undelivered.Add(new Envelope(this, actorName, message));
    }

    public void Talk(object message) =>
        ActorSystem!.Talk(this, message);

    internal ReceiveProcess GetReceiveProcess(string instanceMember)
    {
        var method = GetType().GetMethod(instanceMember);
            
        if (method == null)
            throw new MissingMethodException($@"Method ""{instanceMember}"" not found.");
        var d = (ReceiveProcess)Delegate.CreateDelegate(typeof(ReceiveProcess), this, method);
            
        if (d == null)
            throw new SystemException($@"Method ""{instanceMember}"" is not of type ""ReceiveProcess"".");
            
        return d;
    }

    internal ReceiveProcess? TryGetReceiveProcess(string instanceMember)
    {
        var method = GetType().GetMethod(instanceMember);
            
        if (method == null)
            return null;
            
        try
        {
            return (ReceiveProcess)Delegate.CreateDelegate(typeof(ReceiveProcess), this, method);
        }
        catch
        {
            return null;
        }
    }

    public bool Become(Actor newObject)
    {
        if (string.IsNullOrWhiteSpace(ActorName))
            throw new SystemException("Actor has no name.");

        if (ActorSystem!.Has(ActorName))
            ActorSystem!.DeleteAll(ActorName);
        else
            return false;

        newObject.ActorName = ActorName;
        newObject.ActorState = ActorState;
        ActorSystem!.AddActor(newObject);
        return true;
    }
}