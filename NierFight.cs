using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NierFight : MonoBehaviour {

	public PodFight pod; 
	public NierTimeControl TimeManager; 
	public NierCam camera_control; 
	public Transform [] Targets; 

	public WeaponControl weapon_control;
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
		FindTargets(); 
	}
	
	// Update is called once per frame
	void Update () {

		UpdateTimers(); 

	}

	void UpdateTimers()
	{
		changing_cooldown = changing_cooldown >= 0f ? changing_cooldown - Time.deltaTime : changing_cooldown; 
	}

	public void Hit()
	{
		anim.SetBool("Hit", true); 
	}

	public void StartHeavyCombo()
	{
		anim.SetTrigger("StartSwordCombo"); 
		anim.SetBool("Hit", true); 
	}

	public void GetWeapon(string state)
	{
		weapon_control.ChangeParent(state); 
	}

	public void StartCombo()
	{
		anim.SetTrigger("StartCombo"); 
		anim.SetBool("Hit", true); 
	}

	public void Activation(NierHitData data, bool state)
	{
		hitboxes[data.HitboxName].SetState(data, state); 
	}

	public void Shoot()
	{
		pod.Shoot(); 
	}

	public void Dodge()
	{	
		anim.SetTrigger("Dodge"); 
	}

	public void Impacted()
	{
		anim.SetTrigger("Impact"); 
	}

	public void StopEnnemyTime()
	{
		float factor = TimeManager.StopTime(); 
		anim.speed = 1f/factor; 
	}

	public void ResetTime()
	{
		anim.speed = 1f; 
		TimeManager.ResetTime(); 
	}

	public void ChangeState()
	{
		Camera.main.GetComponent<NierCam>().SetNewTarget(Target); 
		Camera.main.GetComponent<NierCam>().ChangeState(); 
	}

	public void ChangeTarget(float x)
	{
		if(Mathf.Abs(x) > 0.5f && changing_cooldown <= 0f)
		{

			Targets = Globals.CleanArray(Targets); 
			if(Targets.Length > 0)
			{
				
				CurrentTargetIndex += (int)(Mathf.Sign(x)); 
				CurrentTargetIndex = CurrentTargetIndex < 0 ? Targets.Length - 1 : CurrentTargetIndex%Targets.Length; 

				Target = Targets[CurrentTargetIndex]; 

				camera_control.SetNewTarget(Target);

			}
			
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


	void FindTargets()
	{
		NierShortRangeEnnemy [] ennemies = FindObjectsOfType(typeof(NierShortRangeEnnemy)) as NierShortRangeEnnemy []; 
		Transform [] target_placeholder = new Transform [ennemies.Length];
		for(int i = 0; i<ennemies.Length; i++)
		{
			target_placeholder[i] = ennemies[i].gameObject.transform; 
		}

		Targets = target_placeholder;  
	}



}
