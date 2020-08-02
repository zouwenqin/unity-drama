using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayMusicManager : MonoBehaviour
{
	// Start is called before the first frame update

	private static bool bPlay = false;

	public AudioSource musicPlayer;
	public Button btn;
	public Text audioTimeIndex;
	public Text audioTimeAll;
	public Slider audioTimeSlider;
	public bool bShowTime;


	private int currentHour;
	private int currentMinute;
	private int currentSecond;
	private int clipHour;
	private int clipMinute;
	private int clipSecond;




    void Start()
    {

	}

	// Update is called once per frame
	void Update()
	{
		if(bShowTime)
		ShowAudioTime();
	}
	private void ShowAudioTime()
	{
		currentHour = (int)musicPlayer.time / 3600;

		currentMinute = (int)(musicPlayer.time - currentHour * 3600) / 60;

		currentSecond = (int)(musicPlayer.time - currentHour * 3600 - currentMinute * 60);
		audioTimeIndex.text = string.Format("{0:D2}:{1:D2}:{2:D2} ", currentHour, currentMinute, currentSecond);
		string name = string.Format("{0:D2}:{1:D2}:{2:D2} ", currentHour, currentMinute, currentSecond);
		Debug.Log(name);
		audioTimeIndex.text = name;
		//audioTimeAll.text = string.Format("{3:D2}:{4:D2}:{5:D2}",

		//clipHour, clipMinute, clipSecond);

		//audioTimeSlider.value = musicPlayer.time / musicPlayer.clip.length;

	}
	public void playMusic()
	{
		if (bPlay == false)
		{
			bPlay = true;
			//btn.image.sprite =

			btn.image.sprite = Resources.Load<Sprite>("Texture/stopButton");
			if (musicPlayer.isPlaying == false)
			{
				string soundName = DirectorManager.getInstance().getCurrentSound();
				AudioClip clip = Resources.Load<AudioClip>(soundName);
				Debug.Log("playMusic");
				if (clip == null)
				{
					Debug.Log("clip is null");
				}
				musicPlayer.clip = clip;
				musicPlayer.Play();
			}
		}else
		{
			btn.image.sprite = Resources.Load<Sprite>("Texture/playButton");
			bPlay = false;
			if (musicPlayer.isPlaying == true)
			{
				
				musicPlayer.Stop();
			}

		}
		

	}

	public void ChooseNextSound()
	{
		DirectorManager.getInstance().setNextSound();
		Debug.Log("ChooseNextSound");
		if (bPlay == true)
		{
			if (musicPlayer.isPlaying == false)
			{
				string soundName = DirectorManager.getInstance().getCurrentSound();
				AudioClip clip = Resources.Load<AudioClip>(soundName);
				Debug.Log("playMusic");
				if (clip == null)
				{
					Debug.Log("clip is null");
				}
				musicPlayer.clip = clip;
				musicPlayer.Play();
			}
			else
			{
				musicPlayer.Stop();
				string soundName = DirectorManager.getInstance().getCurrentSound();
				AudioClip clip = Resources.Load<AudioClip>(soundName);
				Debug.Log("playMusic");
				if (clip == null)
				{
					Debug.Log("clip is null");
				}
				musicPlayer.clip = clip;
				musicPlayer.Play();

			}
		}

	}

	public void ChoosePreSound()
	{
		DirectorManager.getInstance().setPreSound();

		if (bPlay == true)
		{
			if (musicPlayer.isPlaying == false)
			{
				string soundName = DirectorManager.getInstance().getCurrentSound();
				AudioClip clip = Resources.Load<AudioClip>(soundName);
				Debug.Log("playMusic");
				if (clip == null)
				{
					Debug.Log("clip is null");
				}
				musicPlayer.clip = clip;
				musicPlayer.Play();
			}
			else
			{
				musicPlayer.Stop();
				string soundName = DirectorManager.getInstance().getCurrentSound();
				AudioClip clip = Resources.Load<AudioClip>(soundName);
				Debug.Log("playMusic");
				if (clip == null)
				{
					Debug.Log("clip is null");
				}
				musicPlayer.clip = clip;
				musicPlayer.Play();

			}
		}
	}
}
