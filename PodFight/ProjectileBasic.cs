using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBasic : MonoBehaviour {

	public float Speed; 
	public NierHitData HitInfo; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position += transform.forward*Speed*Time.deltaTime; 

	}
}
