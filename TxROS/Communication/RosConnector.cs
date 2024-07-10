using System;
using System.Diagnostics;
using System.Threading;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Protocols;
using Tecnomatix.Engineering;

namespace TxROS_Playground.Services
{
    public class RosConnector
    {
        public event Action<string> ConnectionStatusChanged;
        public RosConnector()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            TxApplicationEvents appEvents = TxApplication.ApplicationEvents;
            appEvents.Exiting += new TxApplication_ExitingEventHandler(ApplicationExiting);
        }

        public int SecondsTimeout = 10;

        public RosSocket RosSocket { get; private set; }
        public RosSocket.SerializerEnum Serializer;
        public Protocol protocol;
        public string RosBridgeServerUrl = "ws://192.168.0.1:9090";

        public ManualResetEvent IsConnected { get; private set; }

        public virtual void Awake()
        {
            IsConnected = new ManualResetEvent(false);
            new Thread(ConnectAndWait).Start();
        }

        protected void ConnectAndWait()
        {
            RosSocket = ConnectToRos(protocol, RosBridgeServerUrl, OnConnected, OnClosed, Serializer);

            if (!IsConnected.WaitOne(SecondsTimeout * 1000))
                Debug.WriteLine("WARNING: Failed to connect to RosBridge at: " + RosBridgeServerUrl);
        }

        public static RosSocket ConnectToRos(Protocol protocolType, string serverUrl, EventHandler onConnected = null, EventHandler onClosed = null, RosSocket.SerializerEnum serializer = RosSocket.SerializerEnum.Microsoft)
        {
            IProtocol protocol = ProtocolInitializer.GetProtocol(protocolType, serverUrl);
            protocol.OnConnected += onConnected;
            protocol.OnClosed += onClosed;

            return new RosSocket(protocol, serializer);
        }

        private void ApplicationExiting(object sender, TxApplication_ExitingEventArgs args)
        {
            TxApplicationEvents appEvents = TxApplication.ApplicationEvents;
            appEvents.Exiting -= new TxApplication_ExitingEventHandler(ApplicationExiting);
            RosSocket.Close();
        }

        private void OnConnected(object sender, EventArgs e)
        {
            IsConnected.Set();
            string message = "INFO: Connected to RosBridge: " + RosBridgeServerUrl;
            Debug.WriteLine(message);
            ConnectionStatusChanged?.Invoke(message);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            IsConnected.Reset();
            string message = "INFO: Disconnected from RosBridge: " + RosBridgeServerUrl;
            Debug.WriteLine(message);
            ConnectionStatusChanged?.Invoke(message);
        }

    }
}
