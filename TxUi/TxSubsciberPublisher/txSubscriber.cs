/* * Author: Ahmad ash-Sharkwi
 * * Year: 2024
 * * This class is used to subscribe to a String message and process it.
 * */

using RosSharp.RosBridgeClient;
using System;
using TxROS_Playground.Services;

public class txSubscriber : TxSubscriber<RosSharp.RosBridgeClient.MessageTypes.Std.String>
{
    public event Action<string> MessageReceived;

    public txSubscriber(RosConnector rosConnector) : base(rosConnector)
    {
    }

    protected override void ReceiveMessage(RosSharp.RosBridgeClient.MessageTypes.Std.String message)
    {
        string msg = "Received message: " + message.data.ToString();
        System.Diagnostics.Debug.WriteLine(msg);
        MessageReceived?.Invoke(msg);
    }
}
