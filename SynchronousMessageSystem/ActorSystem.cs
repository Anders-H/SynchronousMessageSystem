﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SynchronousMessageSystem;

public class ActorSystem
{
    private List<Actor> Actors { get; } = [];
    public EnvelopeList Undelivered { get; } = [];

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

            foreach (var env in toDeliver.Where(env => env.IsActorAddressUsable))
                Talk(env.Sender, null, env.ActorAddress!, env.Message);
        }
    }

    public void RemoveActor(Actor actor) =>
        Actors.Remove(actor);

    internal void RemoveActor(Type actorType)
    {
        lock (Actors)
        {
            bool again;

            do
            {
                again = false;

                foreach (var x in Actors.Where(x => x.GetType() == actorType))
                {
                    again = true;
                    RemoveActor(x);
                    break;
                }

            } while (again);
        }
    }

    internal void Talk(Actor sender, Actor? receiver, ActorAddress receiverAddress, object message)
    {
        if (receiver == null)
        {
            if (receiver!.GetAddress().IsUsable)
                Undelivered.Add(new Envelope(sender, receiverAddress, message));

            return;
        }

        lock (receiver.MatchList)
        {
            //Deliver to all subscribing functions.
            var delivered = false;

            if (receiver.MatchList.Count > 0)
            {
                //Any function may change the list, so a copy is needed.
                var ml = receiver.MatchList.ToArray();

                foreach (var t in ml)
                {
                    if (!t.IsMatch(message))
                        continue;

                    var receiveProcess = t.GetReceiveProcess(receiver);

                    if (receiveProcess == null)
                        continue;

                    receiveProcess(sender, t, message);
                    delivered = true;
                }
            }

            if (!delivered)
                receiver.Other(sender, null, message);
        }
    }

    internal void Talk(Actor sender, object message)
    {
        lock (Actors)
        {
            var delivered = false;
            var al = Actors.ToArray();

            foreach (var actor in al)
            {
                if (actor.MatchList.Count <= 0)
                    continue;

                var ml = actor.MatchList.ToArray();

                foreach (var t in ml)
                {
                    if (!t.IsMatch(message))
                        continue;

                    var receiveProcess = t.GetReceiveProcess(actor);

                    if (receiveProcess == null)
                        continue;

                    receiveProcess(sender, t, message);
                    delivered = true;
                }
            }

            if (!delivered)
                Undelivered.Add(new Envelope(sender, message));
        }
    }

    public Actor? GetActor(string actorName) =>
        Actors.FirstOrDefault(x => x.ActorName == actorName);

    public Actor? GetActor(Type actorType) =>
        Actors.FirstOrDefault(x => x.GetType() == actorType);

    public IEnumerable<Actor> GetActors(string actorName) =>
        Actors.Where(x => x.ActorName == actorName);

    public IEnumerable<Actor> GetActors(Type actorType) =>
        Actors.Where(x => x.GetType() == actorType);

    public T? GetActor<T>() where T : Actor =>
        (T?)Actors.FirstOrDefault(x => x.GetType() == typeof(T));

    public IEnumerable<T> GetActors<T>() where T : Actor =>
        (IEnumerable<T>)Actors.Where(x => x.GetType() == typeof(T));

    public int TalkToAll(ActorMatch actorMatch, object message) =>
        TalkToAll(null, actorMatch, message);

    public int TalkToAll(Actor? sender, ActorMatch actorMatch, object message)
    {
        var matchCount = 0;

        foreach (var actor in Actors)
        {
            if (!actorMatch.IsMatch(actor))
                continue;
                
            var receiveProcess = actorMatch.GetReceiveProcess(actor);
                
            if (receiveProcess == null)
                actor.Talk(message);
            else
                receiveProcess(sender!, actorMatch, message);

            matchCount++;
        }

        return matchCount;
    }

    public bool Has(string actorName) =>
        GetActor(actorName) != null;

    public void DeleteFirst(string actorName)
    {
        var actor = GetActor(actorName);

        if (actor != null)
            Actors.Remove(actor);
    }

    public void DeleteAll(string actorName)
    {
        while (Has(actorName))
            DeleteFirst(actorName);
    }
}