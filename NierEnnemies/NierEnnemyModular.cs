using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NierEnnemyModular : MonoBehaviour {

	Rigidbody rb; 


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>(); 
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void ImpactInform(NierHitData data, Vector3 direction)
	{
		rb.velocity += data.HitForce*direction; 
	}
}
