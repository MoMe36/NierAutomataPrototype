using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class NierModular : MonoBehaviour {

	public enum NierStates {normal, fight, dash, sprint, jump};
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



	// Use this for initialization
	void Start () {

		Initialization(); 
		
	}
	
	// Update is called once per frame
	void Update () {


		player_direction = inputs.GetDirection(); 
		player_cam_direction = inputs.GetCamDirection(); 
		move.PlayerMove(player_direction); 

		if(inputs.ChangeState)
			fight.ChangeState(); 

		if(inputs.Dash)
			move.Dash(); 

		if(inputs.Jump)
			move.Jump(); 

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


		// if(inputs.Dodge)
		// 	fight.Dodge(player_direction); 
		// if(inputs.Fire)
		// 	fight.Fire(); 

		if(current_state == NierStates.fight)
		{
			fight.ChangeTarget(player_cam_direction.x); 
		}
		
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

	public bool IsHitting()
	{
		return current_sub_state == NierSubStates.fire; 
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
			}
		}

		else if(info == "Dash")
		{
			if(state)
				current_state = NierStates.dash; 
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
				move.Dodge(player_direction); 
				ComputeDashDirection(); 
				// current_sub_state = NierSubStates.dash; 
			}
		}
		else if(info == "Jump")
		{
			if(state)
			{
				move.JumpAction(); 
				current_state = NierStates.jump; 
				Debug.Log("Jump called"); 
			}
		}
		else if(info == "Landed")
		{
			float a = 0f; 
		}
	}

	public void HitInform(NierHitData data, bool state)
	{
		fight.Activation(data, state); 
		return; 
	}

	public void WeaponInform(string state)
	{
		fight.GetWeapon(state); 
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
