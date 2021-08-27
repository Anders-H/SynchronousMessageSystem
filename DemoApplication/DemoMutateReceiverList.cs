using System;
using System.Windows.Forms;
using SynchronousMessageSystem;

namespace DemoApplication
{
    public partial class DemoMutateReceiverList : Form
    {
        private ActorSystem ActorSystem { get; }

        public DemoMutateReceiverList()
        {
            InitializeComponent();
            // 1. Create the actor system.
            ActorSystem = new ActorSystem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 5. Send a string to Actor2c. Click this button twice.
            ActorSystem.GetActor<Actor1C>().SendMessage();
        }

        private void DemoMutateReceiverList_Load(object sender, EventArgs e)
        {
            // 2. Create the actors.
            ActorSystem.AddActor(new Actor1C());
            ActorSystem.AddActor(new Actor2C());
        }

        public class Actor1C : Actor
        {
            public void SendMessage()
            {
                Talk(typeof(Actor2C), "Hello!");
            }

            public override void Other(Actor sender, ActorMatch address, object message)
            {
            }
        }

        public class Actor2C : Actor
        {
            public Actor2C()
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