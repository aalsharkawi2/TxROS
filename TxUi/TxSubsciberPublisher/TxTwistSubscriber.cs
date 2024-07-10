/* * Author: Ahmad ash-Sharkawi
 * * Year: 2024
 * * 
 * * This class is used to subscribe to a Twist message and process it.
 * * The Twist message contains two Vector3 messages, one for linear velocity and one for angular velocity.
 * * The linear velocity is used to move the robot in the x and y direction, while the angular velocity is used to rotate the robot around the z axis.
 * * The precision of the movement is set by the precision variable.
 * * The precision_angular variable is used to set the precision of the rotation.
 * * The distanceZPresent variable is used to check if the robot is rotating.
 * * The linearVelocity and angularVelocity variables are used to store the linear and angular velocity of the robot.
 * * The ProcessMessage method is used to calculate the distance the robot should move and rotate based on the linear and angular velocity.
 * * The distanceX, distanceY and distanceZ variables are used to store the distance the robot should move in the x, y and z direction.
 * * The txAMR_DrivingJointsUtilities object is used to update the driving joints of the robot.
 * * The MoveRobot method is used to move the robot based on the distanceX, distanceY and distanceZ variables.
 * * The UpdateDrivingJointsAxes method is used to update the driving joints of the robot.
 * * The MessageReceived event is used to notify the user that a message has been received.
 * * The ReceiveMessage method is used to receive the Twist message and call the ProcessMessage method.
 * * The ToTxVector method is used to convert a Vector3 message to a TxVector object.
 * *
 * * BUG: The code had bug after a few orientation changes of the robot the, axes is updated successfully but robot location change unexpectedly
 * */

using RosSharp.RosBridgeClient;
using System;
using System.Diagnostics;
using Tecnomatix.Engineering;
using TxROS_Playground.Services;
using TxROS_Playground.Tx;

namespace TxROS_Playground.TxROS.Messages.Twist
{
    public class TxTwistSubscriber : TxSubscriber<RosSharp.RosBridgeClient.MessageTypes.Geometry.Twist>
    {
        public event Action<string> MessageReceived;

        private double precision;
        private double precision_angular;
        private TxVector linearVelocity;
        private TxVector angularVelocity;
        private bool distanceZPresent;
        TxAMR_DrivingJointsUtilities txAMR_DrivingJointsUtilities = new TxAMR_DrivingJointsUtilities();

        public TxTwistSubscriber(RosConnector rosConnector) : base(rosConnector)
        {
        }

        protected override void ReceiveMessage(RosSharp.RosBridgeClient.MessageTypes.Geometry.Twist message)
        {
            string msg = "Received message: " + message.linear.ToString() + message.angular.ToString() + "";
            System.Diagnostics.Debug.WriteLine(msg);
            MessageReceived?.Invoke(msg);
            linearVelocity = ToTxVector(message.linear);
            angularVelocity = ToTxVector(message.angular);

            Debug.WriteLine("linearVelocity: " + linearVelocity);
            Debug.WriteLine("angularVelocity: " + angularVelocity);

            ProcessMessage();
            // Thread.Sleep(2000);
        }

        private static TxVector ToTxVector(RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3 geometryVector3)
        {
            return new TxVector((double)geometryVector3.x, (double)geometryVector3.y, (double)geometryVector3.z);
        }

        public void ProcessMessage()
        {
            precision = 0.05;
            precision_angular = 0.0001;

            double distanceX = linearVelocity.X * precision;
            double distanceY = linearVelocity.Y * precision;
            double distanceZ = angularVelocity.Z * precision_angular;

            Debug.WriteLine("distanceX: " + distanceX); // delete 
            Debug.WriteLine("distanceY: " + distanceY); // delete 
            Debug.WriteLine("distanceZ: " + distanceZ); // delete

            if (distanceZ == 0 && distanceZPresent)
            {
                distanceZPresent = false;
                txAMR_DrivingJointsUtilities.UpdateDrivingJointsAxes();
            }
            else if (distanceZ != 0 && !distanceZPresent)
            {
                distanceZPresent = true;
            }

            txAMR_DrivingJointsUtilities.MoveRobot(distanceX, distanceY, distanceZ);
        }
    }
    
}