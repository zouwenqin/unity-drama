using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Record : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void StartRecord()
	{

	}
	public void StopRecord()
	{

	}
	public void ContinueRecord()
	{

	}
	public void EndRecord()
	{

	}

	public void StratRecordAgain()
	{

	}
	public void ReturnScene()
	{
		SceneManager.LoadScene("selectScene2");
		DirectorManager.getInstance().setSceneState(2);

	}

}
