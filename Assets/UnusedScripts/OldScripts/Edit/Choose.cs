using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choose : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void ChooseSound()
	{
		string soundName = this.name;
		Debug.Log("the choose Name is "+ soundName);
		DirectorManager.getInstance().setCurrentSound(soundName);
	}
	public  void ChooseNextSound()
	{
		
	}
	
}
