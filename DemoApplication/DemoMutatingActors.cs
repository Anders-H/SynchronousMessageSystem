using System;
using System.Windows.Forms;
using SynchronousMessageSystem;

namespace DemoApplication;

public partial class DemoMutatingActors : Form
{
    private ActorSystem ActorSystem { get; }

    public DemoMutatingActors()
    {
        InitializeComponent();
        // 1. Create the actor system.
        ActorSystem = new ActorSystem();
    }

    private void DemoMutatingActors_Load(object sender, EventArgs e)
    {
        // 2. Create the actors.
        ActorSystem.AddActor(new Actor1F("First Actor"));
        ActorSystem.AddActor(new Actor2Fa("Second Actor"));
    }

    private void button1_Click(object sender, EventArgs e)
    {
        // 3. Tell the first actor to talk to the second actor. You could keep a reference in the form.

        // Less type safe way to do this:
        // ((Actor1F?)ActorSystem.GetActor("First Actor"))!.SendMessage();

        // More type safe way to do this:
        ActorSystem.GetActor<Actor1F>()!.SendMessage();
    }
}

public class Actor1F : Actor
{
    public Actor1F(string actorName) : base(actorName, null)
    {
    }

    public void SendMessage()
    {
        // 4. Talk to aktor 2 by name.
        Talk("Second Actor", "Hello!");
    }

    public override void Other(Actor sender, ActorMatch? address, object message)
    {
    }
}

public class Actor2Fa : Actor
{
    public Actor2Fa(string actorName) : base(actorName, null)
    {
    }

    public override void Other(Actor sender, ActorMatch? address, object message)
    {
        // 5. Confirm message and mutate.
        MessageBox.Show(@"Yes, I got the message!");
        Become(new Actor2Fb());
    }
}

public class Actor2Fb : Actor
{
    public override void Other(Actor sender, ActorMatch? address, object message)
    {
        // 6. The second time, this object will receive the message.
        MessageBox.Show(@"Now I got the message!");
    }
}