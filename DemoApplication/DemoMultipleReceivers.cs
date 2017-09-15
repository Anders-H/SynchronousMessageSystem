using System;
using System.Windows.Forms;
using SynchronousMessageSystem;

namespace DemoApplication
{
    public partial class DemoMultipleReceivers : Form
    {
        // 1. Create the actor system.
        private ActorSystem ActorSystem { get; } = new ActorSystem();

        public DemoMultipleReceivers()
        {
            InitializeComponent();
        }

        private void DemoMultipleReceivers_Load(object sender, EventArgs e)
        {
            // 2. Create the actors.
            ActorSystem.AddActor(new Actor1d());
            ActorSystem.AddActor(new Actor2d());
            ActorSystem.AddActor(new Actor3d());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 5. Send a string to Actor2d.
            ActorSystem.GetActor<Actor1d>().SendMessage();
        }
    }
    public class Actor1d : Actor
    {
        public void SendMessage()
        {
            Talk("Hello!");
        }
        public override void Other(Actor sender, ActorMatch address, object message)
        {
        }
    }
    public class Actor2d : Actor
    {
        public Actor2d()
        {
            // 3. Register for string messages.
            MatchList.Add(new ActorMatch(typeof(string), StringReceiver1));
            MatchList.Add(new ActorMatch(typeof(string), StringReceiver2));
        }
        public override void Other(Actor sender, ActorMatch address, object message)
        {
        }
        private void StringReceiver1(Actor sender, ActorMatch address, object message)
        {
            MessageBox.Show($@"String receiver 1 in {GetType().Name} got: {message}");
        }
        private void StringReceiver2(Actor sender, ActorMatch address, object message)
        {
            MessageBox.Show($@"String receiver 2 in {GetType().Name} got: {message}");
        }
    }
    public class Actor3d : Actor
    {
        public Actor3d()
        {
            // 4. Also register for string messages.
            MatchList.Add(new ActorMatch(typeof(string), StringReceiver));
        }
        public override void Other(Actor sender, ActorMatch address, object message)
        {
        }
        private void StringReceiver(Actor sender, ActorMatch address, object message)
        {
            MessageBox.Show($@"String receiver in {GetType().Name} got: {message}");
        }
    }
}
