using System;
using System.Windows.Forms;
using SynchronousMessageSystem;

namespace DemoApplication
{
    public partial class DemoSpecifiedTargetFunctions : Form
    {
        // 1. Create the actor system.
        private ActorSystem ActorSystem { get; } = new ActorSystem();

        public DemoSpecifiedTargetFunctions()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 3. Tell the first actor to talk to the second actor. You could keep a reference in the form.
            ActorSystem.GetActor<Actor1b>().SendMessage();
        }

        private void DemoSpecifiedTargetFunctions_Load(object sender, EventArgs e)
        {
            // 2. Create the actors.
            ActorSystem.AddActor(new Actor1b());
            ActorSystem.AddActor(new Actor2b());
        }
    }

    public class Actor1b : Actor
    {
        public void SendMessage()
        {
            // 4. Send a string and an int.
            Talk(typeof(Actor2b), "Hello!");
            Talk(typeof(Actor2b), 55);
        }
        public override void Other(Actor sender, ActorMatch address, object message)
        {
        }
    }

    public class Actor2b : Actor
    {
        public Actor2b()
        {
            //Configure to send strings to one function and ints to another.
            MatchList.Add(new ActorMatch(typeof(string), I_accept_strings));
            MatchList.Add(new ActorMatch(typeof(int), I_accept_ints));
        }

        // 5. Accept different types of messages.
        public void I_accept_strings(Actor sender, ActorMatch address, object message) =>
            MessageBox.Show((string)message, @"Got a string");

        public void I_accept_ints(Actor sender, ActorMatch address, object message) =>
            MessageBox.Show(((int)message).ToString(), @"Got an int");

        public override void Other(Actor sender, ActorMatch address, object message)
        {
        }
    }
}
