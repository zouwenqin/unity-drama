using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Ex_SendBroadCast : MonoBehaviour
{
    //接收父类发来的消息
    void ReceiveBroadcastMessage(string str)
    {
        Debug.Log("A0 ---- Receiver" + str);
    }

    //接收自己发来的消息
    void ReceiveSendMessage(string str)
    {
        Debug.Log("A0 ---- Receive" + str);
    }

    //接收父类发来的消息

    //private IEnumerator Start()
    //{
    //    yield return StartCoroutine(WaitAndPrint());
    //}

    IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(5f);
    }

    public static Object o = new Object(); //实例化一个锁的对象
    public int n = 0;
    

    private void Start()
    {
       
        Thread thread1 = new Thread(run1);
        Thread thread2 = new Thread(run2);

        thread1.Start();
        thread2.Start();
    }
    
    void run1()
    {

    }

    void run2()
    {
        for (int i = 0; i < 100; i++)
        {
            lock (o)
            {
                n++;
            }
        }
    }

}


