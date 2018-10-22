using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodFight : MonoBehaviour {

	Camera cam; 
	public Vector3 PositionOffset; 
	public GameObject ShootProjectile; 
	public Vector3 ShootOffset; 
	public float ShootCooldown; 
	public GameObject ShootEffect; 
	public float MaxProjectileLifetime = 1f; 

	Vector3 TargetPosition; 

	public enum Tests {character, self, cam}; 
	public Tests CurrentTest = Tests.character;
	public float MinAngle = 15f;  
	public float MaxAngle = 160f; 
	public float LerpPositionSpeed=1f;
	public float ChangePosCooldown=0.1f; 

	

	float shoot_cooldown; 
	float changepos_cooldown; 

	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		
		// transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(cam.transform.position + cam.transform.forward*100, Vector3.up)); 
		if(CurrentTest == Tests.character)
		{
			TargetPosition = transform.parent.position + transform.parent.rotation*PositionOffset; 
		}
		else if(CurrentTest == Tests.self)
		{
			TargetPosition = transform.parent.position + transform.rotation*PositionOffset; 
		}
		else if(CurrentTest == Tests.cam)
		{
			TargetPosition = transform.parent.position + Camera.main.transform.rotation*PositionOffset; 
		}



		if(changepos_cooldown <= 0)
		{
			bool need_change_pos = ChangePosCheck(); 
			if(need_change_pos)
			{
				ChangeOffset();
				changepos_cooldown = ChangePosCooldown; 
			}
		}

		transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime*LerpPositionSpeed); 

		transform.LookAt(cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth/2f, cam.pixelHeight/2f, cam.farClipPlane))); 

		shoot_cooldown = shoot_cooldown <= 0f ? shoot_cooldown : shoot_cooldown - Time.deltaTime; 
		changepos_cooldown = changepos_cooldown <= 0f ? changepos_cooldown : changepos_cooldown - Time.deltaTime; 
	}

	void ChangeOffset()
	{
		float sign = Random.Range(-1f,1f) > 0 ? 1f : -1f; 
		PositionOffset = Quaternion.AngleAxis(sign*90, transform.parent.up)*PositionOffset; 
	}

	bool ChangePosCheck()
	{
		Vector3 v1 = Vector3.ProjectOnPlane(transform.parent.position - transform.position, Vector3.up); 
		float angle = Vector3.Angle(Vector3.ProjectOnPlane(transform.forward, Vector3.up), v1); 

		// to deal specifically with the case where character runs and the pod is forward; 
		float angle2 = Vector3.Angle(v1, transform.parent.forward); 
		Debug.Log(angle2); 

		if(angle < MinAngle)
		{
			return true; 
		}
		else if(angle > MaxAngle)
		{
			return true; 
		}
		else if(angle2 > 170)
		{
			return true; 
		}
		else
		{
			return false; 
		}
	}

	public void Shoot()
	{
		if(shoot_cooldown <= 0f)
		{
			GameObject p = Instantiate(ShootProjectile, transform.position + transform.rotation*ShootOffset, transform.rotation) as GameObject; 
			GameObject e = Instantiate(ShootEffect, transform.position, transform.rotation) as GameObject; 
			Destroy(p, MaxProjectileLifetime); 
			p.transform.LookAt(cam.transform.position + cam.transform.forward*100); 
			shoot_cooldown = ShootCooldown; 
		}
	}
}
