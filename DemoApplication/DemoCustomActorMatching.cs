using System;
using System.Windows.Forms;
using SynchronousMessageSystem;

namespace DemoApplication;

public partial class DemoCustomActorMatching : Form
{
    private ActorSystem ActorSystem { get; }
    private CustomActorMatch? MatchIfValueIsOne { get; set; }
    private CustomActorMatch? MatchIfValueIsTwo { get; set; }

    public DemoCustomActorMatching()
    {
        InitializeComponent();
        // 1. Create the actor system.
        ActorSystem = new ActorSystem();
    }

    private void DemoCustomActorMatching_Load(object sender, EventArgs e)
    {
        // 2. Create the actors.
        var hasOne = new Actor1E(1);
        var hasTwo = new Actor1E(2);

        // 3. Create match conditions.
        MatchIfValueIsOne = new CustomActorMatch(nameof(Actor1E.MyReceiver), 1);
        MatchIfValueIsTwo = new CustomActorMatch(nameof(Actor1E.MyReceiver), 2);
            
        // 4. Add actors to actorsystem.
        ActorSystem.AddActor(hasOne);
        ActorSystem.AddActor(hasTwo);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        // 5. Talk to all who matches the criteria.
        ActorSystem.TalkToAll(MatchIfValueIsOne!, "One"); // Hits both receivers.
        ActorSystem.TalkToAll(MatchIfValueIsTwo!, "Two"); // Hits only the second receiver.
    }
}

public class CustomActorMatch : ActorMatch
{
    public int SearchForValue { get; }

    public CustomActorMatch(string receiveProcess, int searchForValue) : base(null, receiveProcess)
    {
        SearchForValue = searchForValue;
    }

    public override bool IsMatch(object message) =>
        (message as Actor1E)?.Value >= SearchForValue;
}

public class Actor1E : Actor
{
    public int Value { get; }

    public Actor1E(int value)
    {
        Value = value;
    }

    public override void Other(Actor sender, ActorMatch? address, object message)
    {
    }

    public void MyReceiver(Actor sender, ActorMatch address, object message)
    {
        MessageBox.Show($@"Found instance with value {Value}, got message: {message}");
    }
}