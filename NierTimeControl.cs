using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NierTimeControl : MonoBehaviour {

	[Range(0f,1f)]
	public float SlowDownFactor = 1f; 

	public bool Doit; 

	public bool Reset; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Doit)
		{
			Doit = false; 
			StopTime(); 
		}
		if(Reset)
		{
			// Time.timeScale /= SlowDownFactor; 
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f*Time.timeScale; 
		}
		
	}


	public float StopTime()
	{
		Time.timeScale = 1f*SlowDownFactor; 
		Time.fixedDeltaTime = 0.02f*Time.timeScale; 
	
		return SlowDownFactor; 
	}

	public void ResetTime()
	{
		Time.timeScale = 1f; 
		Time.fixedDeltaTime = Time.timeScale*0.02f; 
	}
}
