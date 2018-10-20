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

	public void HitInform(NierHitData data, bool state)
	{
		if(state)
		{
			ShortRangeController.ChangeState("hit");
			Vector3 v = data.ImpulsionDirection.normalized*data.ImpulsionStrength; 
			ShortRangeController.ApplyImpulsion(v); 
		}

		ShortRangeController.ActivateHitbox(data, state); 


	}

	public void ImpactInform(NierHitData data, Vector3 direction)
	{
		rb.velocity += data.HitForce*direction; 
		ShortRangeController.TakeHit(40); 
	}
}
