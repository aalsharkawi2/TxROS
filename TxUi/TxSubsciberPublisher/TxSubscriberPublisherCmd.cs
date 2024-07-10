using System;
using System.Windows.Forms;
using Tecnomatix.Engineering;
using TxROS_Playground.TxUi;

namespace TxHelloROS
{
    public class TxSubscriberPublisherCmd : TxButtonCommand
    {
        public override string Category
        {
            get { return "Test"; }
        }

        public override void Execute(object cmdParams)
        {
            try
            {
                TxSubscriberPublisherForm txform = new TxSubscriberPublisherForm();
                txform.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override string Name
        {
            get { return "ROS Subscriber/Publisher"; }
        }
    }
}
