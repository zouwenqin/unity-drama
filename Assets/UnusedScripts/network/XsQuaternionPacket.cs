

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace xsens
{
    /// <summary>
    /// Parse the data from the stream as quaternions.
    /// </summary>
    class XsQuaternionPacket : XsDataPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="xsens.XsQuaternionPacket"/> class.
        /// </summary>
        /// <param name='readData'>
        /// Create the packet from this data.
        /// </param>
        public XsQuaternionPacket(BinaryReader br)
            : base(br)
        {

        }
        /// <summary>
        ///������������,ǰ�����Ǹ��ڵ�����,������18����Ԫ��`
        /// </summary>
        /// <param(BinaryReader br,��������, int segmentCount'��������Ԫ��>
        ///
        protected override float[] parsePayload(BinaryReader br, int segmentCount)
        {
            //  double[] payloadData = new double[XsMvnPose.MvnSegmentCount * 8];
           float[] payloadData = new float[3+segmentCount *4];
            int startPoint = 0;
            int segmentCounter = 0;

            payloadData[startPoint + 0] = br.ReadSingle();// BitConverter.ToSingle(br.ReadBytes(4),0);   // Xλ������
            payloadData[startPoint + 1] = br.ReadSingle();// BitConverter.ToSingle(br.ReadBytes(4),0);   // Y Position
            payloadData[startPoint + 2] = br.ReadSingle();// /BitConverter.ToSingle(br.ReadBytes(4),0);   // Z Position
            startPoint += 3;

           // Debug.Log("x:    " + payloadData[startPoint - 3] + "    y:   " + payloadData[startPoint - 2] + "   z:   " + payloadData[startPoint - 1]);
          
           // while (segmentCounter != XsMvnPose.MvnSegmentCount)
                while (segmentCounter != segmentCount)
                {
              Debug.Log("����override=================================++++++++++++++++=:" + segmentCounter);
                payloadData[startPoint + 0] = br.ReadSingle();    //��ת����
                payloadData[startPoint + 1] = br.ReadSingle();
                payloadData[startPoint + 2] = br.ReadSingle();
                payloadData[startPoint + 3] = br.ReadSingle();
                //Debug.Log("segmentCounter:  " + segmentCounter + "  Roatex:    " + payloadData[startPoint] + "Roatey:   " + payloadData[startPoint + 1] + "Roatez:   " + payloadData[startPoint + 2]);
                //        + "Roatexw:   " + payloadData[startPoint + 2]);
                /*  payloadData[startPoint + 4] = convert32BitFloat(br.ReadBytes(4));   //��Ԫ����ת Quaternion W
                  payloadData[startPoint + 5] = convert32BitFloat(br.ReadBytes(4));   // Quaternion X
                  payloadData[startPoint + 6] = convert32BitFloat(br.ReadBytes(4));   // Quaternion Y 
                  payloadData[startPoint + 7] = convert32BitFloat(br.ReadBytes(4));   // Quaternion Z	*/
                startPoint += 4;
                segmentCounter++;
            }

            return payloadData;
        }


    }//class XsQuaternionPacket
}//namespace xsens