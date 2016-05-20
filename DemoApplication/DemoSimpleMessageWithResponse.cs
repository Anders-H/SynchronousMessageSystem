using System;
using System.Windows.Forms;
using SynchronousMessageSystem;

namespace DemoApplication
{
    public partial class DemoSimpleMessageWithResponse : Form
    {
        // 1. Create the actor system.
        private ActorSystem ActorSystem { get; } = new ActorSystem();

        public DemoSimpleMessageWithResponse()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 3. Tell the first actor to talk to the second actor. You could keep a reference in the form.
            ActorSystem.GetActor<Actor1a>().SendMessage();
        }

        private void DemoSimpleMessageWithResponse_Load(object sender, EventArgs e)
        {
            // 2. Create the actors.
            ActorSystem.AddActor(new Actor1a());
            ActorSystem.AddActor(new Actor2a());
        }
    }

    public class Actor1a : Actor
    {
        public void SendMessage()
        {
            // 4. Greet actor 2.
            Talk(typeof(Actor2a), "Hello!");
        }
        public override void Other(Actor sender, ActorMatch address, object message)
        {
            // 6. Display response.
            MessageBox.Show((string) message);
        }
    }

    public class Actor2a : Actor
    {
        public override void Other(Actor sender, ActorMatch address, object message)
        {
            // 5. Display incomming message and send a response.
            MessageBox.Show((string) message);
            Talk(sender, "Hello to you!");
        }
    }
}
