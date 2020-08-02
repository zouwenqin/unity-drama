///<summary>
/// Xsens Stream Reader is a component which will read directly from the network.
/// It will spawn 4 threads, 1 for each actor. (MVN Studio can stream up to 4 actors)
/// 
///</summary>
///<version>
/// 0.1, 2013.03.12, Peter Heinen
/// 1.0, 2013.05.14 by Attila Odry, Daniël van Os
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
/// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
/// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
/// AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS
/// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
/// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
/// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
/// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
/// EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
///</remarks>

using UnityEngine;
using System.Net.Sockets;
using System;
using System.Net;
using System.Threading;
using System.IO;
namespace xsens
{
    /// <summary>
    /// This class is responsible for setting up the connection with MVN studio.
    /// MVN Studio can stream up to 4 actors, therefore we have 1 thread for each (4 total).
    /// It also reads which datapacket it should create, and is responsible for always having the last correct pose ready to read for Unity3D
    /// </summary>
    public class XsStreamReader : MonoBehaviour
    {
        public int listenPort = 5678;		//default port in MVN Studio
        private const int NUM_STREAMS = 4;      //number of streams from MVN

        private Thread connectionThread;//连接线程
        private Vector3[] emptyPositions;//位移
        private Quaternion[] emptyOrientations;//旋转角度
        private XsStreamReaderThread[] poseActors;//数据读取线程
        private bool[] availableStreams;//数据流，
        private int counter;
        private bool stopListening;
        private int[] segmentCounts;//骨骼数，，
        //public int[] idMap;
        public static int[] idMap = new int[5] { 0, 1, 2, 3, 4 };

        TcpClient romoteClient;

        const string ip = "10.1.18.64";
         string ipString = "10.1.18.64";
        const int port = 12345;
        //public enum StreamingProtocol
        //{
        //    SPPoseEuler = 1,
        //    SPPoseQuaternion = 2,
        //    SPPosePositions = 3,
        //    SPTagPositionsLegacy = 4,
        //    SPPoseUnity3D = 5,
        //    SPMetaScalingLegacy = 10,
        //    SPMetaPropInfoLegacy = 11,
        //    SPMetaMoreMeta = 12,
        //    SPMetaScaling = 13
        //};

        /// <summary>
        /// Wake this instance.
        /// </summary>
        void Awake()
        {
            //create empty list for reasons when no data arrives
            emptyPositions = new Vector3[XsMvnPose.MvnDefaultSegmentCount];
            emptyOrientations = new Quaternion[XsMvnPose.MvnDefaultSegmentCount];
            Debug.Log("这里是不是每次都执行--------------------------------");
            for (int i = 0; i < XsMvnPose.MvnDefaultSegmentCount; ++i)
            {
                emptyPositions[i] = Vector3.zero;
                emptyOrientations[i] = Quaternion.identity;
            }
            //s修改
            segmentCounts = new int[NUM_STREAMS];
            availableStreams = new bool[NUM_STREAMS];
            poseActors = new XsStreamReaderThread[NUM_STREAMS];

            //idMap = new int[NUM_STREAMS];

            for (int i = 0; i < NUM_STREAMS; ++i)
            {
               // idMap[i] = 9999;
                poseActors[i] = new XsStreamReaderThread();
                availableStreams[i] = false;    // this is set to true in the read thread when data is received for a stream
            }


            listenPort = 12345;
            ipString = "10.1.18.64";
          //  connectionThread = new Thread(new ThreadStart(read));
          //  connectionThread.Start();
            // Make a thread to read from the connection with MVN studio
            
        }

        public void SetLienPort(int port)
        {
            listenPort = port;
        }

        public void SetIP(string ip)
        {
            //ipString = ip;
        }

        /// <summary>
        /// Raises the application quit event.
        /// </summary>
        void OnApplicationQuit()
        {
          
            try
            {
                romoteClient.Dispose();
                // shutdown 'the friendly' way
                stopListening = true;
               
                if (!connectionThread.Join(2000))
                {
                    // shutdown Schwarzenegger style
                    connectionThread.Abort();
                }

                for (int i = 0; i < NUM_STREAMS; ++i)
                {
                    poseActors[i].killThread();
                }
            }
            catch
            {
                Debug.Log("[xsens] XsStreamReader: Something went wrong when trying to close down everything. This is not a critical error.");
            }
        }

        public bool ConnectServer(string ip, int port)
        {
         // ClientSocket mSocket = ClientSocket.GetInstance();
           //  mSocket.ConnectServer("127.0.0.1", 5678);
             return true;
           
        }
        public void SetupServer(string ip, int port)
        {
         //   ip = "10.1.18.64";
           // port = 12345;
           Debug.Log("IP is : " + ipString + "   port: " + port);
            ipString = ip;
            listenPort = port;
            connectionThread = new Thread(new ThreadStart(read));
            connectionThread.Start();
        }
        /// <summary>
        /// Read tcP network data.,建立连接后头部信息判断,获取到数据,进行角色id对应,传入数据到解析数据接口
        /// </summary>
        private void read()
        {

            int bufferSize = 8192;//缓冲区大小

            try
            {

                IPAddress ipAddress = IPAddress.Parse(ip);

                IPEndPoint ipEp = new IPEndPoint(ipAddress, listenPort);
                TcpListener tcpListener = new TcpListener(ipAddress, listenPort);
                tcpListener.Start();
                Debug.Log("服务端-->客户端完成,开启tcp连接监听");
                // Socket serverScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // serverScoket.Bind(ipEp);
                // serverScoket.Listen(5);
                romoteClient = tcpListener.AcceptTcpClient();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return;
            }
           
            NetworkStream stream = romoteClient.GetStream();
            Debug.Log("Connect Succes Msg!!!!");
            do
            {
                Debug.Log("Wait Msg!!!!");
                try
                {
                    //获取与客户端连接数据
                    byte[] request = new byte[bufferSize];
                    int bytesRead = stream.Read(request, 0, bufferSize);// client.Receive(request);
                    string input = System.Text.Encoding.ASCII.GetString(request);
                    //string input = bitconverter.tostring(request, 0, bytesread);//按照字符编码得到字符串
                    Debug.Log("server $$$$$$$$$$$:\n" + input);
                    Debug.Log("接收到客户端的数据: " + "   数据长度: " + bytesRead + "字节");
                    if (bytesRead > 10)
                    {
                        BinaryReader brReader = new BinaryReader(new MemoryStream(request)); 
                        byte[] headMessage = brReader.ReadBytes(6);//kskele消息
                        string input1=System.Text.Encoding.ASCII.GetString(headMessage);
                                Debug.Log("&&&&&&&&&&&&&&&" +input1);

                        byte[] headBytes = brReader.ReadBytes(4);//
                        int length2 = BitConverter.ToInt32(headBytes, 0);
                     //   Debug.Log("&&&&&&&&&&&&&&&***********************" + length2);

                    //    byte[] strLeng = brReader.ReadBytes(4);//消息字符串长度tlen
                    //    int length = BitConverter.ToInt32(strLeng, 0);
                    //    //string input3 = System.Text.Encoding.ASCII.GetString(headMessage);
                    ////    Debug.Log("&&&&&&&&&&&&&&&" + length);

                    //    byte[] strMessage = brReader.ReadBytes(length);//消息串
                    //    string input3 = System.Text.Encoding.ASCII.GetString(strMessage);
                     //   Debug.Log("&&&&&&&&&&&&&&&" + input3);

                        byte[] idBytes = brReader.ReadBytes(4);//总消息长度
                        int dataID =  BitConverter.ToInt32(idBytes, 0);
                       // string input4 = System.Text.Encoding.ASCII.GetString(headMessage);
                    //    Debug.Log("&&&&&&&&&&&&&&&" + dataID);
                    //sqy4.17这里双人判断
                        int streamID;
                        idMap[2] = dataID;
                        
                        if (idMap[2] == idMap[0])
                        {                          
                            streamID = 0;

                        }
                       else if(idMap[2] == idMap[0])
                        {
                           // idMap[1] = dataID;
                            streamID = 1;
                        }
                        else
                        {
                            if (idMap[0] < idMap[1])
                            {
                                idMap[0] = idMap[2];
                                streamID = 0;
                            }
                            else
                            {
                                idMap[1] = idMap[2];
                                streamID = 1;
                            }
                        }

                        Debug.Log("the id si " + dataID + "streamID:" + streamID);
                        // Debug.Log("GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG");
                        segmentCounts[streamID] =18;// (receiveBytes.Length - 10) / 24;
                        poseActors[streamID].setPacket(brReader);
                        //Debug.Log("GGGGGGGGGGGG------------------------GGGGGGGGGG");
                        availableStreams[streamID] = true;
                       
                    }
                    if (bytesRead == 0)
                    {
                        Debug.Log("客户端断开");
                        break;
                    }
                    Debug.Log("success perform");
                    string message = "1";

                  //  byte[] conecetDat = BitConverter.GetBytes(25);
                   // client.Send(conecetDat);

                }
                catch (Exception ex)
                {
                    Debug.Log("客户端异常: " + ex.Message);
                    //客户端出现异常或者断开的时候，关闭线程防止溢出
                   // client.Shutdown(SocketShutdown.Both);//关闭接收和发送
                    //client.Close();
                    break;
                }
            } while (true);
            
            
        }


        /// <summary>
        /// Get the latest pose by the actorId
        /// </summary>判断驱动了哪些角色模型
        /// <param name="actorId"> id of actor to associated with the data</param>
        /// <param name="positions">segment positions of the actor</param>
        /// <param name="orientations">segment orientations of the actor</param>
        /// <returns>true if actor has data or false </returns>
        public bool getLatestPose(int actorId, out Vector3[] positions, out Quaternion[] orientations)
        {
            Debug.Log(" Get ActorID :" + actorId);
            Debug.Log("是否执行===========" + availableStreams[actorId]);
            if (actorId >= 0 && actorId < NUM_STREAMS
             && (availableStreams[actorId] == true)
             && (poseActors[actorId].dataAvailable()))

            {
             //   Debug.Log("是否执行====================");
                return poseActors[actorId].getLatestPose(out positions, out orientations);
            }

            else
            {
                positions = emptyPositions;
                orientations = emptyOrientations;
               // Debug.Log("执行了else================");
                //s修改!!!
                return false;
            }
        }

        /// <summary>
        /// Allows the XsLiveAnimator to setup properly based on the incoming data
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="segmentCount"></param>
        /// <returns></returns>
        public bool poseEstablished(int actorId, out int segmentCount)
        {
            segmentCount = segmentCounts[actorId-1];
            if(segmentCount != 0)
            {
                return true;
            }
            return false;
        }

    }//class XsStreamReader
}//namespace xsens
