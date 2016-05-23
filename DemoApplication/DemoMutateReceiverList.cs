using System;
using System.Windows.Forms;
using SynchronousMessageSystem;

namespace DemoApplication
{
    public partial class DemoMutateReceiverList : Form
    {
        // 1. Create the actor system.
        private ActorSystem ActorSystem { get; } = new ActorSystem();

        public DemoMutateReceiverList()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 5. Send a string to Actor2c. Click this button twice.
            ActorSystem.GetActor<Actor1c>().SendMessage();
        }

        private void DemoMutateReceiverList_Load(object sender, EventArgs e)
        {
            // 2. Create the actors.
            ActorSystem.AddActor(new Actor1c());
            ActorSystem.AddActor(new Actor2c());
        }

        public class Actor1c : Actor
        {
            public void SendMessage()
            {
                Talk(typeof(Actor2c), "Hello!");
            }
            public override void Other(Actor sender, ActorMatch address, object message)
            {
            }
        }

        public class Actor2c : Actor
        {
            public Actor2c()
            {
                // 3. Configure this actor to have the FirstTimeAStringArrives function to respond when strings are sent.
                MatchList.Add(new ActorMatch(typeof(string), FirstTimeAStringArrives));
            }
            public void FirstTimeAStringArrives(Actor sender, ActorMatch address, object message)
            {
                // 4. reconfigure this actor to have the SecondTimeAStringArrives function to respond when strings are sent.
                MessageBox.Show((string)message, @"First time");
                MatchList.Clear();
                MatchList.Add(new ActorMatch(typeof(string), SecondTimeAStringArrives));
            }
            public void SecondTimeAStringArrives(Actor sender, ActorMatch address, object message)
            {
                MessageBox.Show((string)message, @"Second time");
            }
            public override void Other(Actor sender, ActorMatch address, object message)
            {
            }
        }
    }
}
