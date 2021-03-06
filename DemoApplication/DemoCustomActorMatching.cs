﻿using System;
using System.Windows.Forms;
using SynchronousMessageSystem;

namespace DemoApplication
{
    public partial class DemoCustomActorMatching : Form
    {
        // 1. Create the actor system.
        private ActorSystem ActorSystem { get; } = new ActorSystem();
        private CustomActorMatch MatchIfValueIsOne { get; set; }
        private CustomActorMatch MatchIfValueIsTwo { get; set; }
        public DemoCustomActorMatching()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // 5. Talk to all who matches the criteria.
            ActorSystem.TalkToAll(MatchIfValueIsOne, "One"); // Hits both receivers.
            ActorSystem.TalkToAll(MatchIfValueIsTwo, "Two"); // Hits only the second receiver.
        }

        private void DemoCustomActorMatching_Load(object sender, EventArgs e)
        {
            // 2. Create the actors.
            var hasOne = new Actor1e(1);
            var hasTwo = new Actor1e(2);
            // 3. Create match conditions.
            MatchIfValueIsOne = new CustomActorMatch(nameof(Actor1e.MyReceiver), 1);
            MatchIfValueIsTwo = new CustomActorMatch(nameof(Actor1e.MyReceiver), 2);
            // 4. Add actors to actorsystem.
            ActorSystem.AddActor(hasOne);
            ActorSystem.AddActor(hasTwo);
        }
    }
    public class CustomActorMatch : ActorMatch
    {
        public int SearchForValue { get; }
        public CustomActorMatch(string receiveProcess, int searchForValue) : base(null, receiveProcess)
        {
            SearchForValue = searchForValue;
        }
        public override bool IsMatch(object message) => (message as Actor1e)?.Value >= SearchForValue;
    }
    public class Actor1e : Actor
    {
        public int Value { get; }
        public Actor1e(int value)
        {
            Value = value;
        }
        public override void Other(Actor sender, ActorMatch address, object message)
        {
        }
        public void MyReceiver(Actor sender, ActorMatch address, object message)
        {
            MessageBox.Show($@"Found instance with value {Value}, got message: {message}");
        }
    }
}
