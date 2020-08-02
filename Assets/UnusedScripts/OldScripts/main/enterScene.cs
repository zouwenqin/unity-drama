using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class enterScene : MonoBehaviour
{
   
    public void enterStyeScene()
    {
       // Debug.LogError("点击了");
        SceneManager.LoadScene("selectScene2");
        //DirectorManager.getInstance().setSceneState(1);
        GameLoop.Instance.SetCurrentScene(CurrentState.panel1);

    }


}
