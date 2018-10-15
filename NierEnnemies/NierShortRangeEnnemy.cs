using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(NierEnnemyModular))]
public class NierShortRangeEnnemy : MonoBehaviour {

	public Transform Target; 

	public enum EnnemyState {idle, walk, hit, impact}; 
	public EnnemyState state = EnnemyState.idle; 


	[Header("Idle parameters")]
	public float IdleTime = 1f; 

	[Header("Walk parameters")]
	public float WalkSpeed = 1f; 
	public float RotationSpeed = 1f; 
	public float WalkDecision = 1f; 

	[Header("Hit parameters")]
	public NierHitbox hitbox; 
	public float HitDistance =1f; 

	[Header("Death parameters")]
	public float LifePoints = 100f; 
	public GameObject ExplosionEffect; 

	float idle_cooldown; 
	Rigidbody rb; 
	Animator anim; 

	// Use this for initialization
	void Start () {
			
			rb = GetComponent<Rigidbody>(); 
			anim = GetComponent<Animator>(); 
			idle_cooldown = IdleTime; 
	}
	
	// Update is called once per frame
	void Update () {


		if(state == EnnemyState.idle)
		{
			bool ready_to_decide = Count();
			if(ready_to_decide)
			{
				Decide(); 
			}  
		}
		else if(state == EnnemyState.walk)
		{
			Move(transform.forward*WalkSpeed);
			Globals.RotateTowardsFlat(transform, Target.position , RotationSpeed); 
			Decide(); 
		}
		
	}

	public void ActivateHitbox(bool state)
	{
		hitbox.Active = state; 
	}

	public void ChangeState(string state_name)
	{
		if(state_name == "walk")
		{
			state = EnnemyState.walk; 
		}
		else if(state_name == "hit")
		{
			state = EnnemyState.hit; 
		}
		else if(state_name == "impact")
		{
			state = EnnemyState.impact; 
		}
		else
		{
			state = EnnemyState.idle; 
		}
	}

	public void TakeHit(float hit_force)
	{
		LifePoints -= hit_force; 
		if(LifePoints <= 0f)
		{
			anim.SetTrigger("Destruction");
			EnnemyDestroy();  
		}
		else
			anim.SetTrigger("Impact"); 
	}

	public void EnnemyDestroy()
	{
		GameObject p = Instantiate(ExplosionEffect, transform.position, ExplosionEffect.transform.rotation) as GameObject; 
		Destroy(p, 5f); 
		Destroy(gameObject, 1f); 
	}

	void Fight()
	{

	}

	void Decide()
	{
		float current_distance = GetDirectionToTarget().magnitude; 
		if(current_distance < HitDistance)
		{
			anim.SetTrigger("Hit");
		}
		else
		{
			anim.SetTrigger("Walk"); 
		}
	}

	bool Count()
	{
		idle_cooldown -= Time.deltaTime; 
		if(idle_cooldown <= 0f)
		{
			idle_cooldown = IdleTime; 
			return true; 
		}
		else
		{
			return false; 
		}
	}

	Vector3 GetDirectionToTarget()
	{
		return Vector3.ProjectOnPlane(Target.position - transform.position, Vector3.up); 
	}

	void Move(Vector3 direction) 
	{
		rb.AddForce(direction); 
	}
}
