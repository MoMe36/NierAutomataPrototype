using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class NierCam : MonoBehaviour {

	[Header("Idle state")]
	public Transform Target; 
	public Vector3 LookAtOffset; 
	public float DampSpeed = 2;
	public float LookAtSpeed = 5f; 

	public float RotationSpeed = 5; 
	public Vector2 LimX; 

	[Space(20)]
	[Header("Fight state")]
	public bool ChangeSide;
	public float CloseDistance; 
	public float FightPositionLerpSpeed = 1f; 
	public Vector3 FightOffset;  
	public Vector3 CloseOffset; 

	Vector3 frame_vector; 

	public enum CamStates {normal, fight}; 
	public CamStates CurrentState = CamStates.normal; 

	[HideInInspector] public Transform EnnemyToLookAt = null; 	

	Vector3 Offset; 
	Vector3 velocity= Vector3.zero; 

	public float angleY = 0; 
	public float angleX = 0; 
	Vector3 LookTarget; 
	Vector3 TargetPos; 

	// Use this for initialization
	void Start () {

		Offset = transform.position - Target.transform.position; 
		LookTarget = Target.position + LookAtOffset; 
		TargetPos = transform.position; 

		frame_vector = FightOffset; 
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float ix = Input.GetAxis("VerCam"); 
		float iy = Input.GetAxis("HorCam"); 

		if(CurrentState == CamStates.normal)
			NormalProcess(ix,iy); 
		else
			FightProcess(); 


		AdaptPosition(); 
		transform.LookAt(LookTarget); 
	
	}

	void FightProcess()
	{
		// Reconstruct the frame based on Target-Character axis 

		Vector3 v_x = Vector3.ProjectOnPlane((Target.position - EnnemyToLookAt.position).normalized, Vector3.up); 
		Vector3 v_y = Quaternion.AngleAxis(90, Vector3.up)*v_x;
		Vector3 v_z = Vector3.Cross(v_x, v_y);


		Vector3 target_frame_vector = (Target.position - EnnemyToLookAt.position).magnitude < CloseDistance ? CloseOffset : FightOffset; 
		frame_vector = Vector3.Lerp(frame_vector, target_frame_vector, FightPositionLerpSpeed*Time.deltaTime); 

		TargetPos = Target.position + v_x*frame_vector.x + v_y*frame_vector.y + v_z*frame_vector.z; 


		LookTarget = Vector3.Lerp(LookTarget, EnnemyToLookAt.position, LookAtSpeed*Time.deltaTime); 
		
		if(ChangeSide)
		{
			ChangeSide = false; 
			// FightOffset.x *= -1f;
		}
	}
	
	public void ChangeState()
	{
		if(CurrentState == CamStates.fight)
			CurrentState = CamStates.normal; 
		else
			CurrentState = CamStates.fight; 
	}

	public void SetNewTarget(Transform target)
	{
		EnnemyToLookAt = target; 
	}

	void NormalProcess(float ix, float iy)
	{
		Vector3 rotated_offset = Rotate(ix*RotationSpeed, iy*RotationSpeed)*Offset;
		TargetPos = Target.position + rotated_offset; 
		LookTarget = Vector3.Lerp(LookTarget,Target.position + LookAtOffset, LookAtSpeed*Time.deltaTime); 
		// transform.LookAt(Target.position + LookAtOffset);
		// LookTarget = Target.position + LookAtOffset; 

	}

	// void FixedUpdate()
	// {
	// 	AdaptPosition(); 
	// 	transform.LookAt(LookTarget); 
	// }
	void AdaptPosition()
	{
		
		// if(CurrentState == CamStates.idle)
		transform.position = Vector3.SmoothDamp(transform.position,TargetPos, ref velocity, DampSpeed*Time.fixedDeltaTime); 
		// else
		// 	transform.position =TargetPos; 
	}

	Quaternion Rotate(float x, float y)
	{
		angleX += x;
		angleY += y; 

		angleY = angleY%360f; 

		angleX = Mathf.Clamp(angleX,LimX.x, LimX.y);
		Quaternion r = Quaternion.AngleAxis(angleX, transform.right)*Quaternion.AngleAxis(angleY,Vector3.up); 
		return r; 
	}

	

	void ResetRotation()
	{
		angleX = angleY = 0f; 
	}

	// void OnDrawGizmos()
	// {
	// 	Gizmos.color = Color.red; 
	// 	Gizmos.DrawSphere(Target.position + OverTheShoulderOffset, 0.5f); 
	// }
}