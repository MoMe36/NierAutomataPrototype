using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(NierEnnemyModular))]
public class NierShortRangeEnnemy : MonoBehaviour {

	public Transform Target; 
	public float Speed; 

	Rigidbody rb; 

	// Use this for initialization
	void Start () {
			
			rb = GetComponent<Rigidbody>(); 
	}
	
	// Update is called once per frame
	void Update () {


		Move(); 
		
	}


	void Move()
	{
		Vector3 v = Vector3.ProjectOnPlane((Target.position - transform.position).normalized, Vector3.up); 
		rb.AddForce(Speed*v); 
	}
}
