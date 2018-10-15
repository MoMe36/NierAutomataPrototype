using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NierEnnemyModular : MonoBehaviour {


	public NierShortRangeEnnemy ShortRangeController; 
	Rigidbody rb; 

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	// Update is called once per frame
	void Update () {
		
	}


	public void Inform(string info, bool state)
	{
		if(info == "Walk")
		{
			if(state)
			{
				ShortRangeController.ChangeState("walk"); 
			}
		}
		else if(info == "Hit")
		{
			if(state)
			{
				ShortRangeController.ChangeState("hit");
			}	
			ShortRangeController.ActivateHitbox(state); 
		}
		else if(info == "Impact")
		{
			if(state)
			{
				ShortRangeController.ChangeState("impact"); 
			}
		}
		else if(info == "Idle")
		{
			if(state)
			{
				ShortRangeController.ChangeState("idle"); 
			}
		}
	}

	public void ImpactInform(NierHitData data, Vector3 direction)
	{
		rb.velocity += data.HitForce*direction; 
		ShortRangeController.TakeHit(40); 
	}
}
