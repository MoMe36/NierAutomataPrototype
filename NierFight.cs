using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NierFight : MonoBehaviour {

	public NierCam camera_control; 
	public Transform [] Targets; 


	public NierHitbox [] Hitboxes; 

	// public Vector3 CanonOffset; 
	// public GameObject SpellPrefab;
	// public float DestroyTime;  
	

	[Header("Changing target parameters")]
	public float ChangingCooldown = 0.5f; 
	float changing_cooldown; 

	[HideInInspector] public Transform Target;
	int CurrentTargetIndex = 0;  



	Dictionary <string, NierHitbox> hitboxes; 
	Dictionary <string, NierHitbox> hurtboxes;

	Animator anim; 
	Rigidbody rb; 
	NierModular mothership; 	


	// Use this for initialization
	void Start () {
		Initialization(); 
	}
	
	// Update is called once per frame
	void Update () {

		
		// FINIR LOGIC POUR HIT: HIT DATA (PARAMS POUR FORCE, INPUT ETC ) + TRIGGER + BOOL + STATE BEHAV  
		UpdateTimers(); 


	}

	void UpdateTimers()
	{
		changing_cooldown = changing_cooldown >= 0f ? changing_cooldown - Time.deltaTime : changing_cooldown; 
	}

	// public void InstantiateSpell()
	// {
	// 	GameObject p = Instantiate(SpellPrefab, transform.position + transform.rotation*CanonOffset, transform.rotation*SpellPrefab.transform.rotation) as GameObject; 
	// 	Destroy(p, DestroyTime); 
	// }

	public void Hit()
	{
		anim.SetBool("Hit", true); 
	}

	public void StartCombo()
	{
		anim.SetTrigger("StartCombo"); 
		anim.SetBool("Hit", true); 
	}

	public void Activation(NierHitData data, bool state)
	{
		hitboxes[data.HitboxName].Active = state; 
	}

	public void Fire()
	{
		anim.SetTrigger("Spell"); 
	}

	public void Dodge(Vector2 player_direction)
	{	
		if(player_direction.magnitude > 0.15f)
			anim.SetTrigger("Dodge"); 
	}

	public void ChangeState()
	{
		anim.SetTrigger("FightStance"); 
		Camera.main.GetComponent<NierCam>().SetNewTarget(Target); 
		Camera.main.GetComponent<NierCam>().ChangeState(); 
	}

	public void ChangeTarget(float x)
	{
		if(Mathf.Abs(x) > 0.5f && changing_cooldown <= 0f)
		{

			CurrentTargetIndex += (int)(Mathf.Sign(x)); 
			CurrentTargetIndex = CurrentTargetIndex < 0 ? Targets.Length - 1 : CurrentTargetIndex%Targets.Length; 

			Target = Targets[CurrentTargetIndex]; 

			camera_control.SetNewTarget(Target);

			changing_cooldown = ChangingCooldown;  
		}
	}

	void Initialization()
	{
		anim = GetComponent<Animator>(); 
		rb = GetComponent<Rigidbody>(); 
		mothership = GetComponent<NierModular>(); 

		Target = Targets[0]; 
		Globals.FillAllBoxes(Hitboxes, out hitboxes, out hurtboxes); 
		// hitboxes = Globals.FillHitboxes(Hitboxes); 
	}



}
