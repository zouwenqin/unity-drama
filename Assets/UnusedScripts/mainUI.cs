using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xsens;


//4.17sqy删除bvhcontrol直接对绑定的模型附上关节数据
public class mainUI : MonoBehaviour
{
    public XsStreamReader mvnActors;
    public JointController[] m_JointController;
    public InputField ipStringInput;
    public InputField portStringInput;

    public int MonitorActorId;

    // Start is called before the first frame update

    void Start()
    {
         

       Screen.SetResolution(800, 600, true);
        Screen.fullScreen = false;
        string ip = "10.1.18.64";
        int port = 12345;
        mvnActors.SetupServer(ip, port);
        for (int i = 0; i < m_JointController.Length; i++)
        {
            m_JointController[i].SetDelfaultJoint();
        }

    }
    
    public void SetUp()
    {

        // Text text = this.transform.Find("Text").GetComponent<Text>();
        //if (text != null)
        //{

        //string ip = ipStringInput.text;
        //int port = int.Parse(portStringInput.text);
       
      //  Debug.Log("yyyyyyyyyyyyyyyyyyyyyyyyyyyyyy" + ip + "yyyyyyyyy" + port);
            
           // text.text = "listening";
       // }

    }

    // Update is called once per frame
    void Update()
    {
       
       // Debug.Log("+++++++++++++++++++++++++++++是否被执行");
        Vector3[] latestPositions;
        Quaternion[] latestOrientations;
        Vector3[] rot3;
        //sqy4.17这里结构发生变化
        for (int i = 0; i < m_JointController.Length; i++)
        {//这里是模型个数m_JointController
            if (mvnActors.getLatestPose(m_JointController[i].ActorId, out latestPositions, out latestOrientations))
            {
                Debug.Log("latest position length:" + latestPositions.Length + "latestOrientations.length:" + latestOrientations.Length);
                m_JointController[i].SetWorldRotation(latestOrientations, latestPositions[0]);
                //  m_MotionController2.PlayPerFrame(latestOrientations, latestPositions);
            }
            //if (mvnActors.getLatestPose(0, out latestPositions, out latestOrientations))
            // {
            //   Debug.Log("latest position length:" + latestPositions.Length + "latestOrientations.length:" + latestOrientations.Length);
            //   m_MotionController.PlayPerFrame(latestOrientations, latestPositions);
            //  //  m_MotionController2.PlayPerFrame(latestOrientations, latestPositions);
            //}
            else
            {
                Debug.Log("getLatestPose false");
            }
        }
     

      
        //本地播放    
        //   m_MotionController.PlayPerFrame();

    }
}
