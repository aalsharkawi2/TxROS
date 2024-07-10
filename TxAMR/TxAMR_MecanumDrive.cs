/* * Author: Ahmad ash-Sharkawi
 * * Year: 2024
 * 
 * 
 * 
 * Deprecated
 * 
 * 
 * 
 */


using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using System;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.DataTypes;
using static Tecnomatix.Engineering.TxJoint;

namespace TxROS_Playground.Tx
{
    public class TxAMR_MecanumDrive
    {
        protected TxRobot txAMR;
        public TxFrame txTCPF;
        public TxFrame txREFRAME;
        TxDocument txDocument = TxApplication.ActiveDocument;
        public TxAMR_MecanumDrive()
        {
            txAMR = InitializeComponent();
            (txTCPF, txREFRAME) = GetTCPF();
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
                    Debug.WriteLine("INFO: Found an instance of AMR: {obj.Name}");
                    txAMR = txRobot;

                    if (txAMR != null)
                    {
                        Debug.WriteLine("INFO: Checking AMR modeling state");
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

        public TxJoint GetDrivingJoint()
        {
            TxJoint drivingJoint = null;
            if (txAMR != null)
            {
                TxObjectList joints = txAMR.DrivingJoints;

                if (joints.Count > 0)
                {
                    drivingJoint = joints[0] as TxJoint;
                }
            }
            return drivingJoint;
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

        public TxJoint CreateNewDrivingJoint(TxVector FromPoint, TxVector ToPoint, TxJoint.TxJointType jointType, string txWheelKF_f_l, string txWheelKF_r_l, string txWheelKF_f_r, string txWheelKF_r_r)
        {
            Debug.WriteLine("INFO: Switching the driving joint to ${txCase} mode.");
            TxJoint drivingJoint = GetDrivingJoint();
            TxJointCreationData jointData = GetCurrentJointData(drivingJoint);
            jointData = SetNewJointData(drivingJoint, jointData, txTCPF.LocationRelativeToWorkingFrame.Transform(FromPoint), txTCPF.LocationRelativeToWorkingFrame.Transform(ToPoint), "chaseToBaseFootPrint", jointType);
            drivingJoint.Delete();
            ITxKinematicsModellable txKinematicsModellable = txAMR;
            TxJoint txNewDrivingJoint = txKinematicsModellable.CreateJoint(jointData);
            SetKinematicsFunction(txWheelKF_f_l, txWheelKF_r_l, txWheelKF_f_r, txWheelKF_r_r);
            Debug.WriteLine("INFO: The driving joint of the robot is on the ${txCase} mode and the joint type is ${jointData.JointType}.");

            return txNewDrivingJoint;
        }
        
        public (TxVector FromPoint, TxVector ToPoint) GetRevoluteAxis(double x, double y, double theta)
        {
            double R = Math.Sqrt(x * x + y * y) / Math.Abs(theta);
            double alpha = Math.Atan2(x, y);

            double Cx = -R * Math.Sin(alpha);
            double Cy = R * Math.Cos(alpha);

            TxVector FromPoint = new TxVector(Cx, Cy, 0);
            TxVector ToPoint = new TxVector(Cx, Cy, -1);

            return (FromPoint, ToPoint);
        }

        public TxJoint SwitchJointMode(string txCase, double x, double y, double theta)
        {
            string KF_f_l_wheelToBase;
            string KF_r_l_wheelToBase;
            string KF_f_r_wheelToBase;
            string KF_r_r_wheelToBase;

            TxJoint drivingJoint = null;

            switch (txCase)
            {
                case "side":
                    KF_f_l_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0.01745329252)))";
                    KF_r_l_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((-0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((-0.01745329252)))";
                    KF_f_r_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((-0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((-0.01745329252)))";
                    KF_r_r_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0.01745329252)))";

                    drivingJoint = CreateNewDrivingJoint(new TxVector(0, 0, 0), new TxVector(1, 0, 0), TxJoint.TxJointType.Prismatic, KF_f_l_wheelToBase, KF_r_l_wheelToBase, KF_f_r_wheelToBase, KF_r_r_wheelToBase);
                    break;
                case "straight":
                    KF_f_l_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0.01745329252)))";
                    KF_r_l_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0.01745329252)))";
                    KF_f_r_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0.01745329252)))";
                    KF_r_r_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0.01745329252)))";
                    drivingJoint = CreateNewDrivingJoint(new TxVector(0, 0, 0), new TxVector(0, 1, 0), TxJoint.TxJointType.Prismatic, KF_f_l_wheelToBase, KF_r_l_wheelToBase, KF_f_r_wheelToBase, KF_r_r_wheelToBase);
                    break;
                case "diagonal_45":
                    KF_f_l_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0.01745329252)))";
                    KF_r_l_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0)))";
                    KF_f_r_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0)))";
                    KF_r_r_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0.01745329252)))";
                    drivingJoint = CreateNewDrivingJoint(new TxVector(0, 0, 0), new TxVector(Math.Cos(Math.PI / 4), Math.Sin(Math.PI / 4), 0), TxJoint.TxJointType.Prismatic, KF_f_l_wheelToBase, KF_r_l_wheelToBase, KF_f_r_wheelToBase, KF_r_r_wheelToBase);
                    break;
                case "diagonal_135":
                    KF_f_l_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0)))";
                    KF_r_l_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0.01745329252)))";
                    KF_f_r_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0.01745329252)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0.01745329252)))";
                    KF_r_r_wheelToBase = "(((D(chaseToBaseFootPrint))>0)*(D(chaseToBaseFootPrint))*((0)) + (((D(chaseToBaseFootPrint))==0)*(0)) + ((D(chaseToBaseFootPrint))<0)*(D(chaseToBaseFootPrint))*((0)))";
                    drivingJoint = CreateNewDrivingJoint(new TxVector(0, 0, 0), new TxVector(Math.Cos(3 * Math.PI / 4), Math.Sin(3 * Math.PI / 4), 0), TxJoint.TxJointType.Prismatic, KF_f_l_wheelToBase, KF_r_l_wheelToBase, KF_f_r_wheelToBase, KF_r_r_wheelToBase);
                    break;
                case "circle":
                    KF_f_l_wheelToBase = "(((T(chaseToBaseFootPrint))>0)*(T(chaseToBaseFootPrint))*((1)) + (((T(chaseToBaseFootPrint))==0)*(0)) + ((T(chaseToBaseFootPrint))<0)*(T(chaseToBaseFootPrint))*((1)))";
                    KF_r_l_wheelToBase = "(((T(chaseToBaseFootPrint))>0)*(T(chaseToBaseFootPrint))*((1)) + (((T(chaseToBaseFootPrint))==0)*(0)) + ((T(chaseToBaseFootPrint))<0)*(T(chaseToBaseFootPrint))*((1)))";
                    KF_f_r_wheelToBase = "(((T(chaseToBaseFootPrint))>0)*(T(chaseToBaseFootPrint))*((-1)) + (((T(chaseToBaseFootPrint))==0)*(0)) + ((T(chaseToBaseFootPrint))<0)*(T(chaseToBaseFootPrint))*((-1)))";
                    KF_r_r_wheelToBase = "(((T(chaseToBaseFootPrint))>0)*(T(chaseToBaseFootPrint))*((-1)) + (((T(chaseToBaseFootPrint))==0)*(0)) + ((T(chaseToBaseFootPrint))<0)*(T(chaseToBaseFootPrint))*((-1)))";
                    (TxVector FromPoint, TxVector ToPoint) = GetRevoluteAxis(x, y, theta);
                    drivingJoint = CreateNewDrivingJoint(FromPoint, ToPoint, TxJoint.TxJointType.Revolute, KF_f_l_wheelToBase, KF_r_l_wheelToBase, KF_f_r_wheelToBase, KF_r_r_wheelToBase);
                    break;
                default:
                    Debug.WriteLine("INFO: Invalid mode specified.");
                    break;
            }
            return drivingJoint;
        }
    }
}
