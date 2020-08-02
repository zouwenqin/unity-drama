using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Style : MonoBehaviour
{
	public GameObject firstPanel;
	public GameObject secondPanel;
	public GameObject threePanel;
	public GameObject nextButton;
	// Start is called before the first frame update
	void Start()
    {
		//DirectorManager.getInstance().setSceneState(1);
		//firstPanel.SetActive(true);
		//secondPanel.SetActive(false);

	}

    // Update is called once per frame
    void Update()
    {
        
    }
	public void setDramaName()
	{
		string daramaName = this.GetComponent<InputField>().text;
		//Debug.Log(daramaName);
		DirectorManager.getInstance().setDramaName(daramaName);
	}
	public void setBg()
	{
		string bgName = this.name;
		Debug.Log(bgName);
		DirectorManager.getInstance().setBg(bgName);
	}
	public void setCurrentActorOne()
	{
		DirectorManager.getInstance().setCurrentActorID(0);
	}
	public void setCurrentActorTwo()
	{
		DirectorManager.getInstance().setCurrentActorID(1);
	}
	public void setCurrentActorThree()
	{
		DirectorManager.getInstance().setCurrentActorID(2);
	}
	public void setActorName()
	{
		string actorName = this.name;
		Debug.Log(actorName);
		DirectorManager.getInstance().setActorName(actorName);
	}

	public void returnMainScene()
	{
		if (DirectorManager.getInstance().getSceneState() == 1)
		{
			SceneManager.LoadScene("mainScene");
			DirectorManager.getInstance().setSceneState(0);
			nextButton.SetActive(true);
		}
		else if (DirectorManager.getInstance().getSceneState() == 2)
		{
			DirectorManager.getInstance().setSceneState(1);
			firstPanel.SetActive(true);
			secondPanel.SetActive(false);
			threePanel.SetActive(false);
			nextButton.SetActive(true);
		}
		else if (DirectorManager.getInstance().getSceneState() == 3)
		{
			DirectorManager.getInstance().setSceneState(2);
			firstPanel.SetActive(false);
			secondPanel.SetActive(true);
			threePanel.SetActive(false);
			nextButton.SetActive(true);
		}


	}

	public void nextScene()
	{
		Debug.Log("next Scnene------------------"+ DirectorManager.getInstance().getSceneState().ToString());
		if (DirectorManager.getInstance().getSceneState() == 1)
		{
			DirectorManager.getInstance().setSceneState(2);
			firstPanel.SetActive(false);
			secondPanel.SetActive(true);
			threePanel.SetActive(false);
		}
		else if (DirectorManager.getInstance().getSceneState() == 2)
		{
			firstPanel.SetActive(false);
			secondPanel.SetActive(false);
			threePanel.SetActive(true);
			DirectorManager.getInstance().setSceneState(3);
			nextButton.SetActive(false);
		}
	}
}
