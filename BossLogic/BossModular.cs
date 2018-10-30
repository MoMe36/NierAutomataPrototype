using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class BossModular : MonoBehaviour {

	public Transform Target; 
	public enum BossState {idle, walk, dash, attack, dodge, pause}; 
	public BossState boss_state; 

	[Space(10)]
	[Header("Moving params")]
	public float Speed; 
	public float RotationSpeed; 
	public Vector2 DragValues; 

	[Header("Dash params")]
	public float DashSpeed; 
	public float HitDistance; 

	[Header("Dodge params")]
	public float LateralRatio; 
	public float DodgeSpeed;

	[Header("Fight params")]
	public NierHitbox [] Hitboxes; 


	Dictionary <string, NierHitbox> hit_dict;
	Dictionary <string, NierHitbox> hurt_dict;

	Vector3 Direction; 

	Animator anim; 
	Rigidbody rb; 


	// Use this for initialization
	void Start () {
		
		anim = GetComponent<Animator>(); 
		rb = GetComponent<Rigidbody>(); 


		Globals.FillAllBoxes(Hitboxes, out hit_dict, out hurt_dict); 
	}
	
	// Update is called once per frame
	void Update () {


		if(boss_state == BossState.walk)
		{
			Move(); 
			RotateTowardsTarget(); 
		}
		else if(boss_state == BossState.dash)
		{
			bool close_enough = TranslateTowardsTarget(); 
			RotateTowardsTarget();

			if(close_enough)
				anim.SetTrigger("Punch");  
		}
		else if(boss_state == BossState.dodge)
		{
			// TranslateTowardsDirection(); 
			ChangeVelocity(Direction*DodgeSpeed); 
		}
		
	}

	public void Inform(string info, bool state)
	{
		if(info == "Walk")
		{
			if(state)
			{
				boss_state = BossState.walk; 
				SetDrag("max"); 
			}
		}

		else if(info == "Idle")
		{
			SetDrag("min"); 
			boss_state = BossState.idle; 
		}

		else if(info == "Dash")
		{
			SetDrag("max"); 
			boss_state = BossState.dash; 
		}
		else if(info == "Attack")
		{
			boss_state = BossState.attack; 
		}
		else if(info == "DashLeft")
		{
			if(state)
			{
				boss_state = BossState.dodge; 
				ComputeDodgeDirection(-1f);
			}
		}
		else if(info == "DashRight")
		{
			if(state)
			{
				boss_state = BossState.dodge; 
				ComputeDodgeDirection(1f); 
			}
		}
		else if(info == "Pause")
		{
			if(state)
			{
				boss_state = BossState.pause; 
				SetDrag("max"); 
			}
		}

	}

	public void HitInform(NierHitData data, bool state)
	{
		hit_dict[data.HitboxName].SetState(data, state); 
		if(state)
		{
			boss_state = BossState.attack; 
		}
	}

	public void InformBoss(BossActionData bad)
	{
		if(bad.ActivateTrigger)
		{
			anim.SetTrigger(bad.TriggerName); 
			if(bad.TriggerName == "Walk")
			{
				Vector2 direction = BadGuysUtils.RandomDirection(90f);
				Direction = new Vector3(direction.y, 0f, direction.x); 
				Direction = transform.rotation*Direction;

				anim.SetFloat("X", direction.x); 
				anim.SetFloat("Y", direction.y); 

			}

		}
		
	}

	public void ComputeDodgeDirection(float dir)
	{
		Direction = (transform.forward + LateralRatio*transform.right*dir).normalized; 
	}

	public void ChangeVelocity(Vector3 v)
	{
		rb.velocity = v; 
	}

	public void Move()
	{
		rb.AddForce(Direction*Speed); 
	}

	public bool TranslateTowardsTarget()
	{
		Vector3 to_target = Vector3.ProjectOnPlane(Target.position - transform.position, Vector3.up); 
		// transform.position += to_target.normalized*DashSpeed*Time.deltaTime; 
		// transform.position = Vector3.Lerp(transform.position, transform.position + to_target.normalized, Time.deltaTime*DashSpeed);
		// rb.AddForce(to_target.normalized*DashSpeed);  
		rb.velocity = to_target.normalized*DashSpeed; 
		return to_target.magnitude < HitDistance; 
	}

	public void TranslateTowardsDirection()
	{
		transform.position += Direction.normalized*Time.deltaTime*DodgeSpeed; 
	}

	public void RotateTowardsTarget()
	{
		Vector3 v = Vector3.ProjectOnPlane(Target.position - transform.position, Vector3.up); 
		Quaternion rot = Quaternion.AngleAxis(Vector3.SignedAngle(transform.forward, v, Vector3.up), Vector3.up); 
		
		// To be lerped  
		transform.rotation *= rot; 
	}

	public void SetDrag(string val)
	{
		rb.drag = val == "max" ? DragValues.y : DragValues.x; 
	}
}

[System.Serializable]
public struct BossActionData
{
	public bool ActivateTrigger; 
	public string TriggerName; 

	public bool ComputeDirection; 
	public float MaxAngle; 
}


