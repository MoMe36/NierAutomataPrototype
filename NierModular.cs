using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class NierModular : MonoBehaviour {

	public enum NierStates {normal, fight, dash, sprint, jump, impact, dodge, run_stop};
	public enum NierSubStates {idle, fire}; 

	public NierStates current_state = NierStates.normal; 
	public NierSubStates current_sub_state = NierSubStates.idle; 


	Vector2 player_direction; 
	Vector2 player_cam_direction; 

	NierInputs inputs; 
	NierMove move; 
	NierFight fight; 
	Rigidbody rb; 
	Animator anim; 


	float camera_change_state_cooldown; 


	// Use this for initialization
	void Start () {

		Initialization(); 
		
	}
	
	// Update is called once per frame
	void Update () {


		

		RelativeToMove(); 
		RelativeToFight(); 
		UpdateTimers(); 

		
	}

	void UpdateTimers()
	{
		camera_change_state_cooldown = camera_change_state_cooldown <= 0f ? camera_change_state_cooldown : camera_change_state_cooldown - Time.deltaTime; 
	}

	void RelativeToMove()
	{
		player_direction = inputs.GetDirection(); 
		player_cam_direction = inputs.GetCamDirection(); 

		if(Input.GetKeyDown(KeyCode.Space))
		{
			inputs.Dash = true;
			player_direction = transform.right;  
		}

		move.PlayerMove(player_direction); 

		if(inputs.Dash)
			move.Dash(); 

		if(inputs.Jump)
			move.Jump(); 

		if(inputs.Shoot)
			fight.Shoot(); 
	}

	void RelativeToFight()
	{
		if(inputs.ChangeState && camera_change_state_cooldown <= 0f)
		{
			fight.ChangeState();
			camera_change_state_cooldown = 0.5f;  
		}

		if(inputs.Hit)
		{
			if(current_state == NierStates.fight)
				fight.Hit();
			else
				fight.StartCombo();  
		}

		if(inputs.HeavyHit)
		{
			if(current_state == NierStates.fight)
				fight.Hit(); 
			else
				fight.StartHeavyCombo(); 
		}

		fight.ChangeTarget(player_cam_direction.x); 
	}

	public bool IsJumping()
	{
		return current_state == NierStates.jump; 
	}

	public bool IsNormal()
	{
		return current_state == NierStates.normal; 
	}

	public bool IsFighting()
	{
		return	current_state == NierStates.fight; 
	}

	public bool IsDashing()
	{
		return current_state == NierStates.dash; 
	}

	public bool IsSprinting()
	{
		return current_state == NierStates.sprint; 
	}

	public bool IsImpacted()
	{
		return current_state == NierStates.impact; 
	}

	public bool IsDodging()
	{
		return current_state == NierStates.dodge; 
	}


	public void Inform(string info, bool state)
	{

		if(info == "Move")
		{
			if(state)
			{
				current_state = NierStates.normal; 
			}
		}

		else if(info == "FightMode")
		{
			if(state)
			{
				current_state = NierStates.fight; 
				move.EnterFight(); 
			}
		}

		else if(info == "Dash")
		{
			if(state)
			{
				current_state = NierStates.dash; 
				move.EnterDash(); 
			}
		}

		else if(info == "Sprint")
		{
			if(state)
				current_state = NierStates.sprint; 
			else
				move.SetWasSprinting(true); 
		}

		else if(info == "Dodge")
		{
			if(state)
			{
				fight.StopEnnemyTime(); 
				current_state = NierStates.dodge; // Makes NierMove push the character transform
				move.EnterDodge(); 
			}
			else
			{
				fight.ResetTime(); 
			}
		}
		else if(info == "Jump")
		{
			if(state)
			{
				move.JumpAction(); 
				move.EnterJump(); 
				current_state = NierStates.jump; 
			}
		}
		else if(info == "Drop")
		{
			if(state)
			{
				move.EnterDrop(); 
				current_state = NierStates.jump; 
			}
		}
		else if(info == "RunStop")
		{
			if(state)
			{
				move.EnterStop(); 
				current_state = NierStates.run_stop; 
			}
		}
		else if(info == "Impact")
		{
			if(state)
			{
				move.EnterImpact(); 
				current_state = NierStates.impact; 
			}
		}
		else if(info == "Landed")
		{
			float a = 0f; 
		}

	}

	public bool DodgeInform()
	{
		if(inputs.Dodge)
			return true; 
		else
			return false; 
	}

	public void HitInform(NierHitData data, bool state)
	{
		fight.Activation(data, state); 
		if(state)
		{
			move.HitImpulsion(data); 
		}
		return; 
	}

	public void WeaponInform(string state)
	{
		fight.GetWeapon(state); 
	}

	public void ImpactInform(NierHitData data, Vector3 direction)
	{
		bool dodge = DodgeInform(); 

		if(dodge)
		{
			fight.Dodge(); 
		}
		else
		{
			fight.Impacted(); // triggers impact animation 
			move.ChangeVelocity(direction*data.HitForce);  
		}
	}


	// This function is used to shortcut the hitbox activation in the case of a projectile using particles 

	public void ProjectileImpactInform()
	{
		bool dodge = DodgeInform(); 
		if(dodge)
		{
			fight.Dodge(); 
		}
		else
		{
			fight.Impacted(); 
		}
	}

	public bool Ask(string info)
	{
		if(info == "Jump")
		{
			return (current_state != NierStates.jump);
		}

		else
			return false; 
	}

	public void ComputeDashDirection()
	{
		move.ComputeDirectionAndAdjustAnim(player_direction); 
	}

	void Initialization()
	{
		inputs = GetComponent<NierInputs>(); 
		move = GetComponent<NierMove>(); 
		rb = GetComponent<Rigidbody>(); 
		anim = GetComponent<Animator>(); 
		fight = GetComponent<NierFight>(); 
	}
}
