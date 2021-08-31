using System;
using System.Windows.Forms;
using SynchronousMessageSystem;

namespace DemoApplication
{
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
            ActorSystem.AddActor(new Actor1Fa());
            ActorSystem.AddActor(new Actor2F());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 3. Tell the first actor to talk to the second actor. You could keep a reference in the form.
            ActorSystem.GetActor<Actor1F>()!.SendMessage();
        }
    }

    public class Actor1Fa : Actor
    {
        public Actor1Fa(string actorName) : base(actorName, null)
        {
        }

        public override void Other(Actor sender, ActorMatch? address, object message)
        {
        }
    }
}