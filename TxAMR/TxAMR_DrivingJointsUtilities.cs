/* * Author: Ahmad ash-Sharkawi
 * * Year: 2024
 * * This file contains the utilities for the driving joints of the AMR robot.
 * * The driving joints are the joints that are used to move the robot in the x, y, and z directions.
 * * The driving joints are the straight, side, diagonal 45, diagonal 135, and circle joints.
 * */

using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Xml.Linq;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.DataTypes;
using static Tecnomatix.Engineering.TxJoint;

namespace TxROS_Playground.Tx
{
    public class TxAMR_DrivingJointsUtilities
    {
        protected TxRobot txAMR;
        public TxFrame txTCPF;
        public TxFrame txREFRAME;
        public TxFrame txBASEFRAME;
        TxDocument txDocument = TxApplication.ActiveDocument;

        public TxAMR_DrivingJointsUtilities()
        {
            txAMR = InitializeComponent();
            (txTCPF, txREFRAME) = GetTCPF();
            txBASEFRAME = GetBaseFrame();
        }

        public TxRobot InitializeComponent()
        {
            TxObjectList txRobots = txDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(TxRobot)));
            TxRobot txAMR = null;

            Debug.WriteLine("INFO: Searching for AMR instances");
            foreach (TxRobot txRobot in txRobots)
            {
                if (txRobot.Name.Contains("AMR"))
                {
                    Debug.WriteLine($"INFO: Found an instance of AMR: {txRobot.Name}");
                    txAMR = txRobot;

                    if (!txAMR.IsOpenForModeling)
                    {
                        Debug.WriteLine("INFO: Setting AMR for modeling");
                        txAMR.SetModelingScope();
                        Debug.WriteLine("INFO: AMR is set for modeling");
                    }
                    else
                    {
                        Debug.WriteLine("INFO: AMR is set for modeling.");
                    }
                    break;
                }
            }

            if (txAMR == null)
            {
                Debug.WriteLine("INFO: Tecnomatix environment has no AMR instances.");
            }

            return txAMR;
        }

        public (TxFrame, TxFrame) GetTCPF()
        {
            TxFrame txTCPF = null;
            TxFrame txREFRAME = null;
            TxObjectList txFrames = txAMR.GetAllDescendants(new TxTypeFilter(typeof(TxFrame)));

            foreach (TxFrame txframe in txFrames)
            {
                if (txframe.Name == "TCPF")
                {
                    txTCPF = txframe;
                }

                if (txframe.Name == "REFRAME")
                {
                    txREFRAME = txframe;
                }
            }
            return (txTCPF, txREFRAME);
        }

        public TxFrame GetBaseFrame()
        {
            TxFrame txBASEFRAME = null;
            TxObjectList txFrames = txAMR.GetAllDescendants(new TxTypeFilter(typeof(TxFrame)));

            foreach (TxFrame txframe in txFrames)
            {
                if (txframe.Name == "BASEFRAME")
                {
                    txBASEFRAME = txframe;
                    break;
                }
            }
            return txBASEFRAME;
        }

        public TxJoint[] GetDrivingJoints()
        {
            if (txAMR != null)
            {
                TxObjectList joints = txAMR.DrivingJoints;

                if (joints.Count > 0)
                {
                    return new TxJoint[]
                    {
                            joints[0] as TxJoint,
                            joints[1] as TxJoint,
                            joints[2] as TxJoint,
                            joints[3] as TxJoint,
                            joints[4] as TxJoint
                    };
                }
            }
            return null;
        }

        public TxJointCreationData GetCurrentJointData(TxJoint drivingJoint)
        {
            return new TxJointCreationData
            {
                ChildLink = drivingJoint.ChildLink,
                ParentLink = drivingJoint.ParentLink,
                JointType = drivingJoint.Type,
                Name = drivingJoint.Name
            };
        }

        public TxJointCreationData SetNewJointData(TxJoint drivingJoint, TxJointCreationData jointData, TxVector FromPoint, TxVector ToPoint, string jointName, TxJoint.TxJointType jointType)
        {
            jointData.JointType = jointType;
            jointData.SetAxisPoints(FromPoint, ToPoint);
            jointData.Name = jointName;

            return new TxJointCreationData
            {
                ChildLink = drivingJoint.ChildLink,
                ParentLink = drivingJoint.ParentLink,
                JointType = drivingJoint.Type,
                Name = jointName
            };
        }

        public (TxJoint, TxJoint, TxJoint, TxJoint) GetWheelsJoints()
        {
            TxJoint f_l_wheelToBase = null;
            TxJoint r_l_wheelToBase = null;
            TxJoint f_r_wheelToBase = null;
            TxJoint r_r_wheelToBase = null;

            if (txAMR != null)
            {
                TxObjectList joints = txAMR.DrivingJoints;

                if (joints.Count > 0)
                {
                    foreach (TxJoint joint in txAMR.AllJointsAfterCompilation)
                    {
                        if (joint.Name.Contains("f_l_wheelToBase") && joint.IsDependent)
                        {
                            f_l_wheelToBase = joint;
                        }
                        else if (joint.Name.Contains("r_l_wheelToBase") && joint.IsDependent)
                        {
                            r_l_wheelToBase = joint;
                        }
                        else if (joint.Name.Contains("f_r_wheelToBase") && joint.IsDependent)
                        {
                            f_r_wheelToBase = joint;
                        }
                        else if (joint.Name.Contains("r_r_wheelToBase") && joint.IsDependent)
                        {
                            r_r_wheelToBase = joint;
                        }
                    }
                }
            }
            return (f_l_wheelToBase, r_l_wheelToBase, f_r_wheelToBase, r_r_wheelToBase);
        }

        public void SetKinematicsFunction(string txWheelKF_f_l, string txWheelKF_r_l, string txWheelKF_f_r, string txWheelKF_r_r)
        {
            (TxJoint, TxJoint, TxJoint, TxJoint) txWheelsJoints = GetWheelsJoints();
            txWheelsJoints.Item1.KinematicsFunction = txWheelKF_f_l;
            txWheelsJoints.Item2.KinematicsFunction = txWheelKF_r_l;
            txWheelsJoints.Item3.KinematicsFunction = txWheelKF_f_r;
            txWheelsJoints.Item4.KinematicsFunction = txWheelKF_r_r;
        }

        public double GetJointState()
        {
            double jointState = 0.0;
            TxJoint[] drivingJoints = GetDrivingJoints();
            if (drivingJoints != null && drivingJoints.Length > 4)
            {
                jointState = drivingJoints[4].CurrentValue;
            }
            return jointState;
        }

        public void UpdateDrivingJointsAxes()
        {
            txAMR = InitializeComponent();
            txBASEFRAME = GetBaseFrame();
            TxJoint[] drivingJoints = GetDrivingJoints();

            if (txBASEFRAME != null && drivingJoints != null && drivingJoints.Length > 4)
            {
                double theta = drivingJoints[4].CurrentValue;

                double StraightJointState = drivingJoints[2].CurrentValue;
                double SideJointState = drivingJoints[3].CurrentValue;
                double Diagonal45JointState = drivingJoints[1].CurrentValue;
                double Diagonal135JointState = -drivingJoints[0].CurrentValue;

                double alpha_diagonal = Math.PI / 4;
                double x_diagonal = (Diagonal45JointState + Diagonal135JointState) * Math.Cos(alpha_diagonal);
                double y_diagonal = (Diagonal45JointState - Diagonal135JointState) * Math.Sin(alpha_diagonal);
                double x_total = SideJointState + x_diagonal;
                double y_total = StraightJointState + y_diagonal;
                double alpha = Math.Atan2(y_total, x_total);
                double R = Math.Sqrt(x_total * x_total + y_total * y_total);

                // Debug.WriteLine($"R: {R}, alpha: {alpha}");
                // Debug.WriteLine($"StraightJointState: {StraightJointState}, SideJointState: {SideJointState}, Diagonal45JointState: {Diagonal45JointState}, Diagonal135JointState: {Diagonal135JointState}");
                // Debug.WriteLine($"x_diagonal: {x_diagonal}, y_diagonal: {y_diagonal}, x_total: {x_total}, y_total: {y_total}");
                // Debug.WriteLine($"alpha: {alpha}, theta: {theta}");

                TxVector ToPoint = new TxVector(0, 0, 169.99);

                drivingJoints[3].Axis = new TxJointAxis( // Side
                    ToPoint,
                    new TxVector(100 * Math.Cos(theta), 100 * -Math.Sin(theta), 169.99)
                );
                drivingJoints[2].Axis = new TxJointAxis( // Straight
                    ToPoint,
                    new TxVector(100 * Math.Sin(theta), 100 * Math.Cos(theta), 169.99)
                );
                drivingJoints[1].Axis = new TxJointAxis( // Diagonal 45
                    ToPoint,
                    new TxVector(100 * Math.Cos(alpha_diagonal - theta), 100 * Math.Sin(alpha_diagonal - theta), 169.99)
                );
                drivingJoints[0].Axis = new TxJointAxis( // Diagonal 135
                    ToPoint,
                    new TxVector(100 * -Math.Sin(alpha_diagonal - theta), 100 * Math.Cos(alpha_diagonal - theta), 169.99)
                );

                drivingJoints[4].CurrentValue = theta;
                drivingJoints[3].CurrentValue = R * (Math.Cos(alpha) * Math.Cos(theta) - Math.Sin(alpha) * Math.Sin(theta));
                drivingJoints[2].CurrentValue = R * (Math.Sin(alpha) * Math.Cos(theta) + Math.Cos(alpha) * Math.Sin(theta));
            }
        }
        public void MoveRobot(double distanceX, double distanceY, double distanceZ)
        {
            TxJoint[] drivingJoints = GetDrivingJoints();

            if (drivingJoints == null || drivingJoints.Length < 5)
            {
                Debug.WriteLine("ERROR: Driving joints are not properly initialized.");
                return;
            }

            TxJoint circleJoint = drivingJoints[4]; // Circle joint
            TxJoint sideJoint = drivingJoints[3]; // Side joint
            TxJoint straightJoint = drivingJoints[2]; // Straight joint
            TxJoint diagonal45Joint = drivingJoints[1]; // Diagonal 45 joint
            TxJoint diagonal135Joint = drivingJoints[0]; // Diagonal 135 joint

            if (distanceX != 0 && distanceY == 0 && distanceZ == 0)
            {
                sideJoint.CurrentValue += distanceX;
                Debug.WriteLine($"Moved side joint by {distanceX}");
            }
            else if (distanceY != 0 && distanceX == 0 && distanceZ == 0)
            {
                straightJoint.CurrentValue += distanceY;
                Debug.WriteLine($"Moved straight joint by {distanceY}");
            }
            else if (distanceX != 0 && distanceY != 0 && distanceZ == 0)
            {
               if ((distanceX > 0 && distanceY > 0) || (distanceX < 0 && distanceY < 0))
               {
                   diagonal45Joint.CurrentValue += Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
                   Debug.WriteLine($"Moved diagonal 45 joint by {Math.Sqrt(distanceX * distanceX + distanceY * distanceY)}");
                }
               else
               {
                   diagonal135Joint.CurrentValue += Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
                   Debug.WriteLine($"Moved diagonal 135 joint by {Math.Sqrt(distanceX * distanceX + distanceY * distanceY)}");
               }
            }
            else if (distanceZ != 0 && distanceX == 0 && distanceY == 0)
            {
                circleJoint.CurrentValue += distanceZ;
                Debug.WriteLine($"Moved circle joint by {distanceZ}");
                // UpdateDrivingJointsAxes(); Deleted due to inefficiency, Instead I update the axes once after orintation is fixed
            }
            else
            {
                Debug.WriteLine("Unknown movement type.");
            }
 
        }
    }
}
