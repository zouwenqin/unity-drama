///<summary>
/// Xsens Mvn Pose represents all segment data to create a pose.
///</summary>
///<version>
/// 0.1, 2013.03.12, Peter Heinen
/// 1.0, 2013.04.11, Attila Odry
///</version>
///<remarks>
/// Copyright (c) 2013, Xsens Technologies B.V.
/// All rights reserved.
/// 
/// Redistribution and use in source and binary forms, with or without modification,
/// are permitted provided that the following conditions are met:
/// 
/// 	- Redistributions of source code must retain the above copyright notice, 
///		  this list of conditions and the following disclaimer.
/// 	- Redistributions in binary form must reproduce the above copyright notice, 
/// 	  this list of conditions and the following disclaimer in the documentation 
/// 	  and/or other materials provided with the distribution.
/// 

///</remarks>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace xsens
{
    /// <summary>
    /// This class converts all the data from the packet into something Unity3D can easily read.
    /// This also contains the orientations and position fixes needed because of the different coordinate system.
    /// </summary>
    class XsMvnPose
    {
        //Stored segment counts for iterating through
        public static int MvnDefaultSegmentCount = 18;
        public int MvnCurrentSegmentCount = 0;//服务端发送的骨骼数
        public int MvnCurrentPropCount = 0;
        public static int MvnBodySegmentCount = 18;
        public static int MvnFingerSegmentCount = 0;
        public static int MvnPropSegmentCount = 0;

        public Vector3[] positions;
        public Quaternion[] orientations;
        public Vector3[] orientationsVec3;



         Vector3[] rotatVec = new Vector3[3];//旋转向量转四元数的变量
         int m_IndexX = 0, m_IndexY = 1, m_IndexZ = 2;//旋转顺序，x,y，z转换
        //For use with MVN 2018-
        public XsMvnPose(int segmentCount)
        {
            //rotatVec[m_IndexX] = Vector3.right;
            //rotatVec[m_IndexY] = Vector3.up;
            //rotatVec[m_IndexZ] = Vector3.forward;

            //设置骨骼数为25.服务端传过来
            SetupSegmentAmounts(segmentCount);

            positions = new Vector3[MvnCurrentSegmentCount];
            orientations = new Quaternion[MvnCurrentSegmentCount];
            orientationsVec3 = new Vector3[MvnCurrentSegmentCount];
        }

        //For use with MVN 2019+
        public XsMvnPose(int bodySegments, int fingerSegments, int propCount)
        {
            SetupSegmentAmounts(bodySegments,fingerSegments,propCount);

            positions = new Vector3[MvnCurrentSegmentCount];
            orientations = new Quaternion[MvnCurrentSegmentCount];
            orientationsVec3 = new Vector3[MvnCurrentSegmentCount];
        }

        //For use with MVN 2018-
        private void SetupSegmentAmounts(int segmentCount)
        {
            MvnCurrentSegmentCount = segmentCount;
          /*  if(segmentCount > MvnBodySegmentCount + MvnFingerSegmentCount)
            {
                MvnCurrentPropCount = segmentCount - (MvnBodySegmentCount + MvnFingerSegmentCount);
            }*/
        }

        //For use with MVN 2019+
        private void SetupSegmentAmounts(int bodySegments, int fingerSegments, int propCount)
        {
            MvnCurrentSegmentCount = bodySegments + fingerSegments + propCount;
            if (MvnCurrentSegmentCount > MvnBodySegmentCount + MvnFingerSegmentCount)
            {
                MvnCurrentPropCount = MvnCurrentSegmentCount - (MvnBodySegmentCount + MvnFingerSegmentCount);
            }
        }

        /// <summary>
        /// Creates the vector3 positions and the Quaternion rotations for unity, based on the current data packet.
        /// Recursive so it does every segment
        /// </summary>
        /// <param name='startPosition'>
        /// Start position.
        /// </param>
        /// <param name='segmentCounter'>
        /// Segment counter.
        /// </param>
        public void createPose(double[] payloadData)//欧拉角转四元数
        {
            int segmentCounter = 0;
            int startPosition = 0;
            Vector3 position = new Vector3();
          //  Debug.Log("createPose perform");
            position.x = Convert.ToSingle(payloadData[startPosition + 0]);  //X=1
            position.y = Convert.ToSingle(payloadData[startPosition + 1]);  //Y=2
            position.z = Convert.ToSingle(payloadData[startPosition + 2]);  //Z=3
            startPosition +=3;
            int m_IndexX = 1;
            int m_IndexY = 0;
            int m_IndexZ = 2;


         //   Debug.Log("the position X and y and z is :  x: " + position.x + "   ROTATE y:    " + position.y + "    ROTATE z:   " + position.z );
           
            Vector3[] tmpVec = new Vector3[3];
            tmpVec[m_IndexX] = Vector3.right;
            tmpVec[m_IndexY] = Vector3.up;
            tmpVec[m_IndexZ] = Vector3.forward;
            float[] rot = new float[3];
            while (segmentCounter < MvnCurrentSegmentCount)
            {
                Quaternion q = new Quaternion();
                rot[0] = Convert.ToSingle(payloadData[startPosition + 0]);//z
                rot[1] = Convert.ToSingle(payloadData[startPosition + 1]);//y
                rot[2] = Convert.ToSingle(payloadData[startPosition + 2]);//x

               // rot[2] = -rot[2];
               // rot[0] = -rot[0];

   
                q = Quaternion.AngleAxis(rot[2], Vector3.right) //x
                                   * Quaternion.AngleAxis(rot[1], Vector3.up) //y
                                   * Quaternion.AngleAxis(rot[0], Vector3.forward);//z

              // if (segmentCounter == 0)
               //  Debug.Log("segment is " + segmentCounter + "   ROTATE x:    " + rot[0] + "    ROTATE y:   " + rot[1] + "     ROTATE z:   " + rot[2]);



                positions[segmentCounter] = ConvertToUnity(position);
               // orientationsVec3[segmentCounter] = ConvertToUnity(rot);
                orientations[segmentCounter] = q;

                segmentCounter++;
                startPosition += 3;

         
              
            }
        }


        public void createPose2(float[] payloadData)//欧拉角转四元数
        {
            int segmentCounter = 0;
            int startPosition = 0;
            Vector3 position = new Vector3();
            //  Debug.Log("createPose perform");
            position.x = Convert.ToSingle(payloadData[startPosition + 0]);  //X=1
            position.y = Convert.ToSingle(payloadData[startPosition + 1]);  //Y=2
            position.z = Convert.ToSingle(payloadData[startPosition + 2]);  //Z=3
           // Debug.Log("x**@@@@@** " + position.x + "   yyyyy    " + position.y + "   zzzzzzz:   " + position.z);
            startPosition +=3;

            //   Debug.Log("the position X and y and z is :  x: " + position.x + "   ROTATE y:    " + position.y + "    ROTATE z:   " + position.z );

            float[] rot = new float[3];
            while (segmentCounter < MvnCurrentSegmentCount)
            {
                Quaternion q = new Quaternion();
                q.w = Convert.ToSingle(payloadData[startPosition + 0]);//z
                q.x= Convert.ToSingle(payloadData[startPosition + 1]);//y
                q.z = Convert.ToSingle(payloadData[startPosition + 2]);//x
                q.y = Convert.ToSingle(payloadData[startPosition + 3]);
                // rot[2] = -rot[2];
                // rot[0] = -rot[0];
              // q.y = -q.y;
            //   q.z = -q.z;
                // if (segmentCounter == 0)
         //     Debug.Log("qw****** " + q.w + "   ROTATE x:    " + q.x + "    ROTATE y:   " + q.y + "     ROTATE z:   " +q.z);

                positions[segmentCounter] = ConvertToUnity(position);
                // orientationsVec3[segmentCounter] = ConvertToUnity(rot);
                orientations[segmentCounter] = q;

                segmentCounter++;
                startPosition += 4;



            }
        }







        /// <summary>
        /// Converts a position from MVN Coordinate Space to Unity Coordinate Space
        /// </summary>
        /// <param name="originalVector"></param>
        /// <returns></returns>
        Vector3 ConvertToUnity(Vector3 originalVector)
        {
            return new Vector3(
                -originalVector.x,
                //originalVector.y,
                //originalVector.z
                      originalVector.y,
                originalVector.z

                );
            /*
            return new Vector3(
                -originalVector.y,
                originalVector.z,
                originalVector.x);*/
        }

        /// <summary>
        /// Converts a orientation from MVN Coordinate Space to Unity Coordinate Space
        /// </summary>
        /// <param name="originalOrientation"></param>
        /// <returns></returns>
        Quaternion ConvertToUnity(Quaternion originalOrientation)
        {
            return new Quaternion(
                originalOrientation.y,
                -originalOrientation.z,
                -originalOrientation.x,
                originalOrientation.w);
        }


    }//class XsMvnPose	
}//namespace xsens