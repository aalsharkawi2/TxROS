using System.Threading;
using TxROS_Playground.Services;

namespace RosSharp.RosBridgeClient
{
    public abstract class TxSubscriber<T> where T : Message
    {
        public string Topic;
        public float TimeStep;

        private RosConnector rosConnector;
        private string subscriptionId;
        private readonly int SecondsTimeout = 1;

        protected TxSubscriber(RosConnector rosConnector)
        {
            this.rosConnector = rosConnector;
        }
        public void Start()
        {
            new Thread(Subscribe).Start();
        }

        public void Subscribe()
        {
            if (!rosConnector.IsConnected.WaitOne(SecondsTimeout * 1000))
                System.Diagnostics.Debug.WriteLine("Failed to subscribe: RosConnector not connected");

            subscriptionId = rosConnector.RosSocket.Subscribe<T>(Topic, ReceiveMessage, (int)(TimeStep * 1000));             // the rate(in ms in between messages) at which to throttle the topics
        }
        public void Unsubscribe()
        {
            if (subscriptionId != null)
            {
                rosConnector.RosSocket.Unsubscribe(subscriptionId);
                subscriptionId = null;
            }
        }
        protected abstract void ReceiveMessage(T message);
        public bool IsConnected()
        {
            return rosConnector.IsConnected.WaitOne(0);
        }
    }
}