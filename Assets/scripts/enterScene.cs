using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class enterScene : MonoBehaviour
{
   
    public void OnClickEnterStyleScene()
    {
       
        SceneManager.LoadSceneAsync("selectScene2");
        
        //GameManager.Instance.SetCurrentScene(CurrentPanel.panel1);

    }


}
