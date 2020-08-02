using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//sqy4.17修改为25绑定个关节点参数,但是传入是18个
public class JointController : MonoBehaviour
{
    public int ActorId;//模型Id
    //public Transform m_SpineBase;             //0
    //public Transform m_SpineMid;       //1
    //public Transform m_Neck;         //2
    //public Transform m_Head;        //3
    //public Transform m_ShoulderLeft;        //4
    //public Transform m_ElbowLeft;          //5
    //public Transform m_WristLeft;         //6
    //public Transform m_HandLeft;            //7
    //public Transform m_ShoulderRight;           //8
    //public Transform m_ElbowRight;           //9
    //public Transform m_WristRight;           //10

    //public Transform m_HandRight;             //11
    //public Transform m_HipLeft;             //12
    //public Transform m_KneeLeft;    //13
    //public Transform m_AnkleLeft;         //14
    //public Transform m_FootLeft;     //15
    //public Transform m_HipRight;        //16
    //public Transform m_KneeRight;  //17
    //public Transform m_AnkleRight;  //18
    //public Transform m_FootRight;       //19 
    //public Transform m_SpineShoulder;            //20 
    //public Transform m_HandTipLeft;        //21
    //public Transform m_ThumbLeft;           //22 
    //public Transform m_HandTipRight;      //23
    //public Transform m_ThumbRight;      //24
    public Transform m_Hips;             //0
    public Transform m_RightUpLeg;       //1
    public Transform m_RightLeg;         //2
    public Transform m_RightFoot;        //3
    public Transform m_LeftUpLeg;        //4
    public Transform m_LeftLeg;          //5
    public Transform m_LeftFoot;         //6
    public Transform m_Spine;            //7
    public Transform m_Spine1;           //8
    public Transform m_Spine2;           //9
    public Transform m_Spine3;           //10

    public Transform m_Neck;             //11
    public Transform m_Head;             //12
    public Transform m_RightShoulder;    //13
    public Transform m_RightArm;         //14
    public Transform m_RightForeArm;     //15
    public Transform m_RightHand;        //16
    public Transform m_RightHandThumb1;  //17
    public Transform m_RightHandThumb2;  //18
    public Transform m_LeftShoulder;       //36 
    public Transform m_LeftArm;            //37 
    public Transform m_LeftForeArm;        //38 
    public Transform m_LeftHand;           //39 
    public Transform m_LeftHandThumb1;      //40
    public Transform m_LeftHandThumb2;      //41
    public Transform m_LeftHandThumb3;      //42

    public Transform m_LeftInHandIndex;     //43
    public Transform m_LeftHandIndex1;      //44
    public Transform m_LeftHandIndex2;      //45
    public Transform m_LeftHandIndex3;      //46

    public Transform m_LeftInHandMiddle;    //47
    public Transform m_LeftHandMiddle1;     //48
    //public float m_Height = 180f;
    //private float m_ModelHeight = 2.743842f;
    //private float m_HeightUnit = 1 / 180f;

    private float m_ModelHips = 1.472f;
    private float m_HipsHeight = 100f;
    private float m_HipsHeightUnit = 1.472f / 100f;
    private float m_HipsUnit = 0.001957f;

    //public List<Vector3[]> m_ModelAxisOffset = new List<Vector3[]>();

    private Vector3[] m_ModelWorldPosOffset = new Vector3[25];
    private Quaternion[] m_ModelQuaOffset = new Quaternion[25];
    private Vector3[] m_PostionOffset = new Vector3[25];
    private int m_QuaNumbers = 25;// 59;
    public List<Quaternion> m_JointsQuatOffset = new List<Quaternion>();
    public List<Transform> m_Joints = new List<Transform>();
    public float PositionUnit = 1;

    public Quaternion m_NullQuat;

    /**
    * @brief Unity에서 쓰는 모델의 설정 되어있는 rotation과 position값을 저장합니다.
    */
    public void Awake()
    {
        // robot.LookAt(new Vector3(-16.375f, 0, 0.0f));


        //print("the robot**********************" + robot.localEulerAngles.x + "   y:" + robot.localEulerAngles.y + " z" + robot.localEulerAngles.z);

        //   robot.rotation = robot.rotation * Quaternion.Euler(0, 79,-70) *Quaternion.Euler(70, -79, 0);
        m_NullQuat = new Quaternion(0, 0, 0, 0);
        //print("awake *****************************************" + m_Hips.localEulerAngles.x + "y:" + m_Hips.localEulerAngles.y + "z:" + m_Hips.localEulerAngles.z);
        //m_ModelQuaOffset[0] = m_SpineBase.rotation;             //0
        //m_ModelQuaOffset[1] = m_SpineMid.rotation;       //1
        //m_ModelQuaOffset[2] = m_Neck.rotation;         //2
        //m_ModelQuaOffset[3] = m_Head.rotation;        //3
        //m_ModelQuaOffset[4] = m_ShoulderLeft.rotation;        //4
        //m_ModelQuaOffset[5] = m_ElbowLeft.rotation;          //5
        //m_ModelQuaOffset[6] = m_WristLeft.rotation;         //6
        //m_ModelQuaOffset[7] = m_HandLeft.rotation;            //7
        //m_ModelQuaOffset[8] = m_ShoulderRight.rotation;           //8
        //m_ModelQuaOffset[9] = m_ElbowRight.rotation;           //9
        //m_ModelQuaOffset[10] = m_WristRight.rotation;           //10

        //m_ModelQuaOffset[11] = m_HandRight.rotation;             //11
        //m_ModelQuaOffset[12] = m_HipLeft.rotation;             //12
        //m_ModelQuaOffset[13] = m_KneeLeft.rotation;    //13
        //m_ModelQuaOffset[14] = m_AnkleLeft.rotation;         //14
        //m_ModelQuaOffset[15] = m_FootLeft.rotation;     //15
        //m_ModelQuaOffset[16] = m_HipRight.rotation;        //16
        //m_ModelQuaOffset[17] = m_KneeRight.rotation;  //17
        //m_ModelQuaOffset[18] = m_AnkleRight.rotation;  //18
        //m_ModelQuaOffset[19] = m_FootRight.rotation;       //19 
        //m_ModelQuaOffset[20] = m_SpineShoulder.rotation;            //20 
        //m_ModelQuaOffset[21] = m_HandTipLeft.rotation;        //21
        //m_ModelQuaOffset[22] = m_ThumbLeft.rotation;           //22 
        //m_ModelQuaOffset[23] = m_HandTipRight.rotation;      //23
        //m_ModelQuaOffset[24] = m_ThumbRight.rotation;      //24

        m_ModelQuaOffset[0] = m_Hips.rotation;

        m_ModelQuaOffset[1] = m_LeftUpLeg.rotation;
        m_ModelQuaOffset[2] = m_LeftLeg.rotation;
        m_ModelQuaOffset[3] = m_LeftFoot.rotation;

        m_ModelQuaOffset[4] = m_RightUpLeg.rotation;
        m_ModelQuaOffset[5] = m_RightLeg.rotation;
        m_ModelQuaOffset[6] = m_RightFoot.rotation;

        m_ModelQuaOffset[7] = m_Spine.rotation;
        m_ModelQuaOffset[8] = m_Spine1.rotation;
        m_ModelQuaOffset[9] = m_Neck.rotation;

        m_ModelQuaOffset[10] = m_LeftShoulder.rotation;
        m_ModelQuaOffset[11] = m_LeftArm.rotation;
        m_ModelQuaOffset[12] = m_LeftForeArm.rotation;
        m_ModelQuaOffset[13] = m_LeftHand.rotation;

        m_ModelQuaOffset[14] = m_RightShoulder.rotation;
        m_ModelQuaOffset[15] = m_RightArm.rotation;
        m_ModelQuaOffset[16] = m_RightForeArm.rotation;
        m_ModelQuaOffset[17] = m_RightHand.rotation;
        Quaternion q = new Quaternion();
        q = Quaternion.FromToRotation(new Vector3(0, 1, 0), new Vector3(-1, 2, 0));
        print("the angle is ****************************x:" + q.eulerAngles.x + "   y:" + q.eulerAngles.y + "   z：" + q.eulerAngles.z);

    }

   
    //sqy4.17
    public void SetRotation(Quaternion[] rot, Vector3 pos)
    {
        print("set Rotation***************************************x:" + pos.x + "y:" + pos.y + "z" + pos.z);
        for (int i = 0; i < rot.Length; i++)//rot.Length
        {
            if (i < m_Joints.Count && m_Joints[i] != null)
            {
                m_Joints[i].rotation = m_JointsQuatOffset[i];// new Quaternion(-0.5f, 0.5f, 0.5f, 1.0f);// m_JointsQuatOffset[i];m_Joints[i].rotation;//
            }
        }

        for (int i = rot.Length - 1; i >= 0; i--)//rot.Length  rot.Length-1rot.Length - 1
        {
            //if (i == 9)
            //    continue;
            {
                m_Joints[i].rotation =  rot[i]*m_JointsQuatOffset[i];// Quaternion.Euler(0, 79, 0);// rot[i];// *m_JointsQuatOffset[i];
            }
        }

        Vector3 temp = new Vector3(pos.x * 0.01f, pos.y*0.01f, pos.z * 0.01f);
        m_Hips.position = temp;
        // m_Hips.position = pos[0]/200.0f;

    }

  


    public void Clear()
    {

        if (m_JointsQuatOffset != null)
        {
            print("*******************" + m_Joints.Count);

            for (int i = 0; i < m_Joints.Count; i++)
            {
                if (m_Joints[i] != null && m_JointsQuatOffset[i] != null)
                    m_Joints[i].rotation = m_JointsQuatOffset[i];
            }

            m_JointsQuatOffset.Clear();
        }
        if (m_Joints != null)
            m_Joints.Clear();
    }




    /**
    * @brief m_Joints와 m_JointsQuatOffset에 관절데이터를 추가합니다.
    * @param flag   관절 상위 flag값. 0:hips, 1:head, 2: lhand, 3: rhand, 4:lleg, 5:rleg.
    * @param count  상위세그먼트에 대한 하위세그먼트 갯수.
    */
    public void AddJoint(int flag, int count)
    {
        Debug.Log("add joints******************************************");
        SetDelfaultJoint();

    }

    /**
    * @brief bvh에비해 모델에 관절이없어서 더미데이터를 추가합니다.
    */
    //public void AddDummyJoint(int count)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        m_Joints.Add(null);
    //        m_JointsQuatOffset.Add(Quaternion.identity);
    //    }
    //}

    /**
    * @brief 모델에 붙어있는 관절볼을 보여줍니다.
    */
    //public void SetJointBallActive(bool active)
    //{
    //    foreach (Transform t in m_JointBall)
    //    {
    //        t.gameObject.SetActive(active);
    //    }
    //    SkinTransparent();
    //}

        
    public void SetWorldRotation(Quaternion[] rot, Vector3 pos)
    {
        print("SetWorldRotation***************************************");
        for (int i = 0; i < rot.Length; i++)
        {
            int idx = i;// m_CsvOrderTable[i];
            if (idx < m_Joints.Count && m_Joints[idx] != null)
            {
                if (!rot[idx].Equals(m_NullQuat))
                    m_Joints[idx].rotation = rot[idx] * m_JointsQuatOffset[idx];
            }
        }
        pos *= m_HipsHeightUnit;
        m_Hips.position = pos;
    }






    public void SetDelfaultJoint()
    {
        Clear();
        //bvh start之后运行
        print("SetDelfaultJoint******************************");

        //m_Joints.Add(m_SpineBase);   //0
        //    m_Joints.Add(m_SpineMid);   //1
        //    m_Joints.Add(m_Neck);   //2
        //    m_Joints.Add(m_Head);   //3
        //    m_Joints.Add(m_ShoulderLeft);   //4
        //    m_Joints.Add(m_ElbowLeft);   //5
        //    m_Joints.Add(m_WristLeft);   //6
        //    m_Joints.Add(m_HandLeft);   //7
        //    m_Joints.Add(m_ShoulderRight);   //8
        //    m_Joints.Add(m_ElbowRight);   //9


        //    m_Joints.Add(m_WristRight);   //10
        //    m_Joints.Add(m_HandRight);   //11
        //    m_Joints.Add(m_HipLeft);   //12
        //    m_Joints.Add(m_KneeLeft);   //13


        //    m_Joints.Add(m_AnkleLeft);   //14
        //    m_Joints.Add(m_FootLeft);   //15
        //    m_Joints.Add(m_HipRight);   //16
        //    m_Joints.Add(m_KneeRight);   //17

        //    m_Joints.Add(m_AnkleLeft);   //18
        //    m_Joints.Add(m_FootLeft);   //19
        //    m_Joints.Add(m_HipRight);   //20
        //    m_Joints.Add(m_KneeRight);   //21
        //     m_Joints.Add(m_ThumbLeft);   //22
        //    m_Joints.Add(m_HandTipRight);   //23
        //    m_Joints.Add(m_ThumbRight);   //24

        m_Joints.Add(m_Hips);   //0
        m_Joints.Add(m_LeftUpLeg);   //1
        m_Joints.Add(m_LeftLeg);   //2
        m_Joints.Add(m_LeftFoot);   //3
        m_Joints.Add(m_RightUpLeg);   //4
        m_Joints.Add(m_RightLeg);   //5
        m_Joints.Add(m_RightFoot);   //6
        m_Joints.Add(m_Spine);   //7
        m_Joints.Add(m_Spine1);   //8
        m_Joints.Add(m_Neck);   //9


        m_Joints.Add(m_LeftShoulder);   //10
        m_Joints.Add(m_LeftArm);   //11
        m_Joints.Add(m_LeftForeArm);   //12
        m_Joints.Add(m_LeftHand);   //13


        m_Joints.Add(m_RightShoulder);   //14
        m_Joints.Add(m_RightArm);   //15
        m_Joints.Add(m_RightForeArm);   //16
        m_Joints.Add(m_RightHand);   //17

        //有效代码传进来是rotation
        for (int i = 0; i < m_QuaNumbers; i++)
            m_JointsQuatOffset.Add(m_ModelQuaOffset[i]);


    }
}
