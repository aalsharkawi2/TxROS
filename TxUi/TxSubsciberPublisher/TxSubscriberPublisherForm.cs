/* * Author: Ahmad ash-Sharkawi
 * * Year: 2024
 * * This form is used to connect to a ROS server and subscribe/publish to a topic.
 * * BUG: a lot of bad code here.
 * * BUG: You should edit the the websocekt url even it does not have to change
 * * BUG: You should connect to server before subscriping. Otherwise, the program crashes
 * * BUG: some unexpected behaviour after disconnecting and reconnecting.
 * * BUG: Logs are not in good format
 * * TODO: Publishing is not implemented
 * */

using RosSharp.RosBridgeClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;
using TxROS_Playground.Services;
using TxROS_Playground.TxROS.Messages.Twist;
using System.Xml.Linq;

namespace TxROS_Playground.TxUi
{
    public partial class TxSubscriberPublisherForm : TxForm
    {
        private RosConnector rosConnector;
        private TxTwistSubscriber txTwistSubscriber;
        TxDocument txDocument = TxApplication.ActiveDocument;

        public TxSubscriberPublisherForm()
        {
            InitializeComponent();
            rosConnector = new RosConnector();
            rosConnector.ConnectionStatusChanged += RosConnector_ConnectionStatusChanged;
            txTwistSubscriber = new TxTwistSubscriber(rosConnector);
            txTwistSubscriber.MessageReceived += txTwistSubscriber_MessageReceived;
        }

        private void RosConnector_ConnectionStatusChanged(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => RosConnector_ConnectionStatusChanged(message)));
            }
            else
            {
                LogTB.Text += message + Environment.NewLine;
            }
        }

        private void txTwistSubscriber_MessageReceived(string message)
        {
            if (LogTB.InvokeRequired)
            {
               LogTB.Invoke(new Action(() => LogTB.Text += message + Environment.NewLine));
            }
            else
            {
               LogTB.Text += message + Environment.NewLine;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void WebSocketTB_TextChanged(object sender, EventArgs e)
        {
            rosConnector.RosBridgeServerUrl = "ws://" + WebSocketTB.Text;
        }

        private void MSGTypeTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void PublishMSGTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void TopicNameTB_TextChanged(object sender, EventArgs e)
        {
            PublishCheckBox.Enabled = true;
            SubscribeCheckBox.Enabled = true;
            txTwistSubscriber.Topic = TopicNameTB.Text;
        }

        private void PublishCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = PublishCheckBox.Checked;
            PublishMSGTB.Enabled = isChecked;
            MSGTypeTB.Enabled = isChecked;
        }

        private void SubscribeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SubscribeCheckBox.Checked)
            {
                txTwistSubscriber.Start();
                PlaySimulation();
            }
            else
            {
                txTwistSubscriber.Unsubscribe();
                PlaySimulation();
            }
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            if (ConnectBtn.Text == "Connect")
            {
                rosConnector.Awake();
                ConnectBtn.Text = "Disconnect";
                TopicNameTB.Enabled = true;
            }
            else
            {
                PublishCheckBox.Enabled = false;
                SubscribeCheckBox.Enabled = false;
                PublishCheckBox.Checked = false;
                SubscribeCheckBox.Checked = false;
                TopicNameTB.Enabled = false;
                rosConnector.RosSocket.Close();
                ConnectBtn.Text = "Connect";
            }

        }

        private void LogTB_TextChanged(object sender, EventArgs e)
        {
            LogTB.SelectionStart = LogTB.Text.Length;
            LogTB.ScrollToCaret();
        }

        private void TxSubscriberPublisherForm_Load(object sender, EventArgs e)
        {

        }

        private void PlaySimulation()
        {
            if (txDocument.SimulationPlayer.IsSimulationRunning())
            {
               // txDocument.SimulationPlayer.Rewind();
            }
            else
            {
              //  txDocument.SimulationPlayer.Play();
            }
        }
    }
}
