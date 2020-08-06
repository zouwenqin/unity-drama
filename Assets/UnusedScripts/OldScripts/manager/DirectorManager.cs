using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum SceneState
{
	StateStart=0,
	StateSelect1 = 1,
	StateSelect2 = 2,
	StateRecord = 3,
	StatePlay = 4,



}
public class DirectorManager
{
	DirectorManager()
	{
		currentSound = "sound1";
	}

   
   public static DirectorManager getInstance()
	{
		if(instance==null)
		{
			instance = new DirectorManager();
		}
		return instance;
	}


	public int getSceneState()
	{
		return sceneState;
	}
	public void setSceneState(int state)
	{
		sceneState = state;
	}

	public void setFrameStyle(int style)
	{
		frameStyle = style;
	}
	public void setFilterStyle(int style)
	{
		filterStyle = style;
	}
	public void setEditStyle(int style)
	{
		editStyle = style;
	}

	public int getFrameStyle()
	{
		return frameStyle ;
	}
	public int getFilterStyle()
	{
		return  filterStyle ;
	}
	public int getEditStyle()
	{
		return  editStyle ;
	}

	public void setDramaName(string name)
	{
		dramaName = name;
	}
	public void setBg(string name)
	{
		bgName = name;
	}
	public void setCurrentActorID(int id)
	{
		currentActorID = id;
	}
	public void setActorName(string name)
	{
		actorNames[currentActorID] = name;
	}
	public void setCurrentSound(string sound)
	{
		currentSound = sound;
		for (int i = 0; i < soundList.Length; i++)
			if (soundList[i] == sound)
				currentIndex = i;
	}
	public string getCurrentSound()
	{
		return soundList[currentIndex];
	}
	public void setNextSound()
	{
		currentIndex = currentIndex + 1 > soundList.Length - 1 ? 0 : currentIndex + 1;
	}
	public void setPreSound()
	{
		currentIndex = currentIndex -1<0 ? soundList.Length - 1 : currentIndex - 1;
	}

	public AudioSource GetMusicPlayer() {return musicPlayer; }
	public void SetMusicPlayer(AudioSource player) {  musicPlayer = player; }


	private static DirectorManager instance;
	private int sceneState = 0;

	private int frameStyle = 0;
	private int filterStyle = 0;
	private int editStyle = 0;


	private int currentActorID = 0;
	private string[] actorNames = { "", "", "", "", "", "", "", "", "", "", };
	private string[] soundList = { "sound1", "sound2", "sound3", "sound4" };
	private string bgName;
	private string dramaName;
	private string currentSound;
	private int currentIndex = 0;

	public AudioSource musicPlayer=null;


}
