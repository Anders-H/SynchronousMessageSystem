using System;
using System.Windows.Forms;

namespace DemoApplication
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using var x = new DemoSimpleMessageWithResponse();
            x.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using var x = new DemoSpecifiedTargetFunctions();
            x.ShowDialog(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using var x = new DemoMutateReceiverList();
            x.ShowDialog(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using var x = new DemoMultipleReceivers();
            x.ShowDialog(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using var x = new DemoCustomActorMatching();
            x.ShowDialog(this);
        }
    }
}