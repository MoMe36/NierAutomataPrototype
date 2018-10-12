using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class NierInputs : MonoBehaviour {

	public float x,y,camX, camY; 
	public bool Dash; 
	public bool ChangeState; 
	public bool Jump; 
	public bool Hit; 
	public bool HeavyHit; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		
		ProcessJS(); 
		
	}

	void ProcessJS()
	{
		x = Input.GetAxis("Horizontal"); 
		y = Input.GetAxis("Vertical"); 

		camX = Input.GetAxis("HorCam");
		camY = Input.GetAxis("VerCam"); 

		ChangeState = Input.GetAxis("R2") > 0.5f; 
		Dash = Input.GetButtonDown("BButton"); 
		Jump = Input.GetButtonDown("AButton"); 
		Hit = Input.GetButtonDown("XButton"); 
		HeavyHit = Input.GetButtonDown("YButton"); 

	}

	public Vector2 GetDirection()
	{
		Vector2 v = new Vector2(x,y); 
		return v.normalized; 
	}

	public Vector2 GetCamDirection()
	{
		Vector2 v = new Vector2(camX,camY); 
		return v.normalized; 
	}
}
