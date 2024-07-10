using TxROS_Playground.Services;

namespace RosSharp.RosBridgeClient
{
    public abstract class TxPublisher<T> where T : Message
    {
        public string Topic;
        private string publicationId;

        private RosConnector rosConnector;

        protected virtual void Start()
        {
            rosConnector = new RosConnector();
            publicationId = rosConnector.RosSocket.Advertise<T>(Topic);
        }

        protected void Publish(T message)
        {
            rosConnector.RosSocket.Publish(publicationId, message);
        }
    }
}