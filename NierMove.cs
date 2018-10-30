using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class NierMove : MonoBehaviour {

	public float Speed; 
	public float RotationSpeed; 

	[Header("Drag values. X is min, Y is Max")]
	public Vector2 RigidbodyDrag; 

	[Header("Dash parameters")]
	public float DashSpeed = 1f; 

	[Header("Sprint parameters")]
	public float SprintSpeed = 1f;
	public float MaxAngleRunStop = 120f;  

	[Header("Dodge parameters")]
	public float DodgeDistance = 1f;
	public float DodgeLerpSpeed; 
	Vector3 DodgePositionTarget; 

	[Header("Jump parameters")]
	public Vector3 GroundedDetectionOffset; 
	public float AdditionalGravity = 1f; 
	public float MinVerticalSpeedForGravity = 1f; 
	public float AirSpeed = 1f; 
	public float JumpForce = 1f;
	[Range(0f, 1f)]
	public float TransferSpeedOnLandingRatio = 0.5f; 
	public float MaxHeight = 1f; 
	[Range(0f, 1f)]
	public float LandingLimit = 0.9f; 
	public string JumpAnimationName = "Jump"; 
	public float JumpControlLerpSpeed = 0f; 
	[Range(0f, 1f)]
	public float InitialJumpLerpValue = 0.5f; 

	float height;  
	float jump_lerp_current_value; 


	[Header("Animator lerp parameters")]
	[Range(1,50f)]
	public float AnimSpeedRatio = 1f; 
	public float AnimSpeedLerp = 1f; 


	bool continuous_jump_control = false; 

	Quaternion TargetRotation; 

	Animator anim; 
	Rigidbody rb; 
	NierModular mothership; 	
	NierFight fight; 

	// Use this for initialization
	void Start () {
		Initialization(); 
	}
	
	// Update is called once per frame
	void Update () {

		UpdatePhysics(); 
		UpdateState(); 
		UpdateAnim(); 

	}


	void UpdatePhysics()
	{
		if(continuous_jump_control)
		{
			bool up = true; 
			if(rb.velocity.y <= MinVerticalSpeedForGravity)
			{
				ApplyAdditionalGravity();  
				up = false; 
			}
			ContinuousJumpControl(up);
		}
		else
		{
			CheckGrounded(); 
		}
	}

	void UpdateState()
	{
		UpdateRotation(); 
	}

	void UpdateAnim()
	{
		// Update speed value

		float current_speed = anim.GetFloat("Speed"); 
		current_speed = Mathf.Lerp(current_speed, Vector3.ProjectOnPlane(rb.velocity, Vector3.up).magnitude/AnimSpeedRatio, AnimSpeedLerp*Time.deltaTime); 
		anim.SetFloat("Speed", Mathf.Clamp01(current_speed)); 

	}

	void UpdateRotation()
	{
		// Vector3 current_axis, target_axis;  
		// float current_angle, target_angle; 

		// TargetRotation.ToAngleAxis(out target_angle, out target_axis); 
		// transform.rotation.ToAngleAxis(out current_angle, out current_axis); 


		// print("Expected axis " + target_axis.ToString()); 
		// print("Current axis" + current_axis.ToString()); 


		// if(target_axis != new Vector3(0f, 1f, 0f))
		// 	Debug.Break(); 

		transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, RotationSpeed*Time.deltaTime); 
	}

	void ContinuousJumpControl(bool going_up)
	{
		if(going_up)
		{
			jump_lerp_current_value = Mathf.Lerp(jump_lerp_current_value, 0f, JumpControlLerpSpeed*Time.deltaTime); 
			anim.Play(JumpAnimationName, 0,  jump_lerp_current_value);
		}
		else
		{
			Ray ray = new Ray(transform.position, - transform.up);
		    RaycastHit hit;

		    float current_height = MaxHeight;

		    if(Physics.Raycast(ray, out hit, MaxHeight))
		    {
		      current_height = hit.distance;
		    }

		    float a = 1f/(1.1f*height - MaxHeight);
		    float b = MaxHeight/(MaxHeight - 1.1f*height);
		    float normalized_clip_value = Mathf.Clamp01((current_height*a + b));

		    jump_lerp_current_value = Mathf.Lerp(jump_lerp_current_value, normalized_clip_value, JumpControlLerpSpeed*Time.deltaTime);

		    if(normalized_clip_value > LandingLimit && rb.velocity.y <0.1f)
		    {
		      anim.SetTrigger("Land");
		      jump_lerp_current_value = InitialJumpLerpValue;
		      mothership.Inform("Landed", true); 
		      continuous_jump_control = false; 
		      // SetAdditionalGravity(false);
		      // current_additional_gravity = 0f;
		      TransferSpeedOnLanding();
		    }
		    else
		    {
		      anim.Play(JumpAnimationName, 0,  jump_lerp_current_value);
		    }
		}
	}

	void CheckGrounded()
	{
		if(!mothership.IsJumping())
		{
			Ray ray = new Ray(transform.position + transform.rotation*GroundedDetectionOffset, Vector3.down); 
			RaycastHit hit; 

			if(!Physics.Raycast(ray, out hit, height*1.05f))
			{
				Drop(); 
			}
		}
	}

	void TransferSpeedOnLanding()
	{
		rb.velocity += Vector3.ProjectOnPlane(rb.velocity, Vector3.up)*TransferSpeedOnLandingRatio; 
	}

	void ApplyAdditionalGravity()
	{
		rb.velocity += Vector3.down*AdditionalGravity; 
	}

	public void PlayerMove(Vector2 direction)
	{
		bool has_inputs = direction.magnitude > 0.15f;
		if(mothership.IsNormal() ||  mothership.IsSprinting())
		{
			if(has_inputs)
			{
				if(rb.velocity.magnitude < 0.01f)
				{
					QuickStart(); 
				}
				float actual_speed = mothership.IsNormal() ? Speed : SprintSpeed; 
				Vector3 desired_direction = ComputePlayerDirection(direction); 
				Move(desired_direction*actual_speed); 
				TargetRotation = transform.rotation*ComputeAngleFromForward(desired_direction); 
				SetDrag("max"); 


				if(mothership.IsSprinting())
				{
					float angle = Vector3.Angle(desired_direction, rb.velocity.normalized); 
					if(!Globals.SmallerAngle(desired_direction, rb.velocity.normalized, MaxAngleRunStop))
						StopRun(); 
				}
			} 
			else
			{
				SetDrag("min"); 
				if(mothership.IsSprinting())
				{
					StopRun(); 
				}
			}

		}
		else if(mothership.IsDashing())
		{
			// TargetRotation = transform.rotation; 
			// SetDrag("min");
			// Above is now set in EnterDash(). Called by NierModular

			if(has_inputs)
			{
				Vector3 desired_direction = ComputePlayerDirection(direction); 
				Move(desired_direction*DashSpeed); 
				AdjustAnimXY(desired_direction); 

				if(!Globals.SmallerAngle(desired_direction, rb.velocity.normalized, MaxAngleRunStop))
					StopRun(); 

			}	
			else
			{
				Move(transform.forward*DashSpeed); 
				AdjustAnimXY(transform.forward);
			}
		}
		else if(mothership.IsJumping())
		{
			// Same as sprint and running, but with drag being max 
			if(has_inputs)
			{
				Vector3 desired_direction = ComputePlayerDirection(direction); 
				Move(desired_direction*AirSpeed); 
				TargetRotation = transform.rotation*ComputeAngleFromForward(desired_direction); 
			} 
		}
		else if(mothership.IsFighting())
		{
			// Drag is set to max in the EnterFight function called by NierModular
			
			Vector3 desired_direction = ComputePlayerDirection(direction); 
			TargetRotation = transform.rotation*ComputeAngleFromForward(desired_direction);
		}
		else if(mothership.IsDodging())
		{
			transform.position = Vector3.Lerp(transform.position, DodgePositionTarget, DodgeLerpSpeed*Time.fixedDeltaTime); 
		}
	}

	void Move(Vector3 v)
	{
		rb.AddForce(v); 
	}

	public void ChangeVelocity(Vector3 v)
	{
		rb.velocity = v; 
	}

	void SetDrag(string target)
	{
		rb.drag = target == "max" ? RigidbodyDrag.y : RigidbodyDrag.x; 
	}

	public void QuickStart()
	{
		anim.SetTrigger("QuickStart"); 
	}

	public void StopRun()
	{
		anim.SetTrigger("StopRun"); 
	}

	public void EnterStop()
	{
		SetDrag("max"); 
	}

	public void EnterFight()
	{
		SetDrag("max"); 
	}

	public void EnterDash()
	{
		TargetRotation = transform.rotation; 
		SetDrag("min"); 
	}

	public void EnterImpact()
	{
		SetDrag("max"); 
	}

	public void EnterJump()
	{
		SetDrag("min"); 
		continuous_jump_control = true; 
	}

	public void EnterDodge()
	{
		SetDrag("max"); 
		DodgePositionTarget = transform.position - transform.forward*DodgeDistance; 
	}

	public void EnterDrop()
	{
		SetDrag("min"); 
		continuous_jump_control = true; 
	}

	public void HitImpulsion(NierHitData data)
	{
		Vector3 impulsion_direction = transform.rotation*data.ImpulsionDirection.normalized*data.ImpulsionStrength;
		rb.velocity += impulsion_direction; 
	}

	public void Dash()
	{
		anim.SetTrigger("Dash"); 
	}

	public void Jump()
	{
		anim.SetTrigger("Jump"); 
	}

	public void Drop()
	{
		anim.SetTrigger("Drop"); 
	}

	public void JumpAction()
	{ 
		rb.velocity += Vector3.up*JumpForce; 
	}

	public void SetWasSprinting(bool state)
	{
		anim.SetBool("WasSprinting", state);
	}	

	public void Dodge(Vector2 player_direction)
	{
		Vector3 desired_direction = ComputePlayerDirection(player_direction);
		// rb.velocity += desired_direction.normalized*DashForce; 
	}

	public Quaternion ComputeAngleFromForward(Vector3 v)
	{
		float angle = Vector3.SignedAngle(transform.forward, v, Vector3.up); 
		Quaternion target = Quaternion.AngleAxis(angle, Vector3.up); 
		return target; 
	}

	public Vector3 CamToPlayer()
	{
		Vector3 v = Vector3.ProjectOnPlane(transform.position - Camera.main.transform.position, Vector3.up); 
		return v.normalized; 
	}

	public Vector3 ComputePlayerDirection(Vector2 player_direction)
	{
		Vector3 from_cam = CamToPlayer(); 
		Vector3 result = from_cam*player_direction.y + Quaternion.AngleAxis(90, Vector3.up)*from_cam*player_direction.x; 
		return result.normalized; 
	}

	public Vector2 CosSinToPlayer(Vector2 player_direction)
	{
		float current_angle = Vector3.SignedAngle(transform.forward, player_direction, Vector3.up); 
		current_angle *= Mathf.Deg2Rad; 

		float x = Mathf.Sin(current_angle); 
		float y = Mathf.Cos(current_angle); 

		return new Vector2(x,y); 
	}

	public void AdjustAnimXY(Vector3 walking_direction)
	{
		float current_angle = Vector3.SignedAngle(transform.forward, walking_direction, Vector3.up); 
		current_angle *= Mathf.Deg2Rad; 

		anim.SetFloat("X", Mathf.Sin(current_angle)); 
		anim.SetFloat("Y", Mathf.Cos(current_angle));  
	}

	public void ResetAnimXY()
	{
		float f = anim.GetFloat("X"); 
		f = Mathf.Clamp01(f-Time.deltaTime); 
		anim.SetFloat("X", f);
		anim.SetFloat("Y", f);	
	}

	public void ComputeDirectionAndAdjustAnim(Vector2 player_direction)
	{
		Vector3 result = ComputePlayerDirection(player_direction); 
		AdjustAnimXY(result); 
	}

	void Initialization()
	{
		anim = GetComponent<Animator>(); 
		rb = GetComponent<Rigidbody>(); 
		mothership = GetComponent<NierModular>(); 
		fight = GetComponent<NierFight>() ;

		TargetRotation = transform.rotation; 
		
		JumpInitialization(); 
	}

	void JumpInitialization()
	{
		height = GetComponent<CapsuleCollider>().height/2f; 
		jump_lerp_current_value = InitialJumpLerpValue; 

	}

}
