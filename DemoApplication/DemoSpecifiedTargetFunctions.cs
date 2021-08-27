using System;
using System.Windows.Forms;
using SynchronousMessageSystem;

namespace DemoApplication
{
    public partial class DemoSpecifiedTargetFunctions : Form
    {
        private ActorSystem ActorSystem { get; }

        public DemoSpecifiedTargetFunctions()
        {
            InitializeComponent();
            // 1. Create the actor system.
            ActorSystem = new ActorSystem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 3. Tell the first actor to talk to the second actor. You could keep a reference in the form.
            ActorSystem.GetActor<Actor1B>().SendMessage();
        }

        private void DemoSpecifiedTargetFunctions_Load(object sender, EventArgs e)
        {
            // 2. Create the actors.
            ActorSystem.AddActor(new Actor1B());
            ActorSystem.AddActor(new Actor2B());
        }
    }

    public class Actor1B : Actor
    {
        public void SendMessage()
        {
            // 4. Send a string and an int.
            Talk(typeof(Actor2B), "Hello!");
            Talk(typeof(Actor2B), 55);
        }

        public override void Other(Actor sender, ActorMatch address, object message)
        {
        }
    }

    public class Actor2B : Actor
    {
        public Actor2B()
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