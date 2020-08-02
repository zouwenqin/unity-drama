
using System;
using UnityEngine;
using System.Threading;
using System.IO;

namespace xsens
{
    /// <summary>
    /// Xsens Stream Reader Thread.
    /// Every actor from Stream has its own reader trhead.
    /// </summary>
    class XsStreamReaderThread
    {
        private Thread thread;
        private byte[] lastPackets;
        //private bool newData = false;
        private bool dataUpdated = true;

        private Vector3[] lastPosePositions;
        private Quaternion[] lastPoseOrientations;

        /// <summary>
        /// Initializes a new instance of the <see cref="xsens.XsStreamReaderThread"/> class.
        /// </summary>
        public XsStreamReaderThread()
        {
            //make sure we always have some date, even when no streaming
         //   Debug.Log("寻找lastposeposition位置1222223333333333334");
            lastPosePositions = new Vector3[XsMvnPose.MvnDefaultSegmentCount];
            lastPoseOrientations = new Quaternion[XsMvnPose.MvnDefaultSegmentCount];
            //start a new thread		
            thread = new Thread(new ThreadStart(start));
            thread.Start();
        }

        /// <summary>
        /// Start this instance.
        /// The datapacket will be set to one of the supported mode, based on its type.
        /// </summary>
        public void start()
        {
            while (true)
            {
                  Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Check if there is data available.
        /// </summary>
        /// <returns>
        /// true if data is available
        /// </returns>
        public bool dataAvailable()
        {
            return dataUpdated;
        }

        /// <summary>
        /// Get the latest pose info that is available
        /// </summary>
        /// <param name="positions">This will return the positions</param>
        /// <param name="orientations">This will return the orientations</param>
        /// <returns>True if a proper pose was available, false otherwise</returns>
        public bool getLatestPose(out Vector3[] positions, out Quaternion[] orientations)
        {
            positions = lastPosePositions;
            orientations = lastPoseOrientations;

           // Debug.Log("get latest pose *****************");
            return true;
        }

        /// <summary>
        /// Kills the thread.
        /// </summary>
        public void killThread()
        {
            thread.Abort();
        }

     



        public void setPacket(BinaryReader br)
        {
            XsDataPacket dataPacket = new XsQuaternionPacket(br);
          
            XsMvnPose pose = dataPacket.getPose();
            if (pose != null)
            {
                lastPosePositions = pose.positions;
                lastPoseOrientations = pose.orientations;
                dataUpdated = true;
            }

        }
    }//class XsStreamReaderThread
}//namespace xsens