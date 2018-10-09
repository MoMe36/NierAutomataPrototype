using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NierEnnemyShooter : MonoBehaviour {

	public Transform Target; 
	public Transform Barrel; 
	public GameObject Projectile;
	public float DestructionTime = 1f;  
	public float RotationSpeed = 1f; 
	public float DelayBetweenShoot = 2f; 
	public float MaxAngle = 5f; 
	public float CurrentAngle; 


	float cooldown; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

		Rotate(); 
		Fire(); 

	}

	void Fire()
	{
		if(cooldown <= 0f)
		{
			if(CurrentAngle < MaxAngle)
			{	
				Shoot();
				cooldown = DelayBetweenShoot; 
			}	 
		}
		else
		{
			cooldown -= Time.deltaTime; 
		}
	}

	void Rotate()
	{
		Quaternion rot = Quaternion.FromToRotation(transform.forward, Vector3.ProjectOnPlane(Target.position - transform.position, Vector3.up));  
		transform.rotation = Quaternion.Lerp(transform.rotation, rot*transform.rotation, RotationSpeed*Time.deltaTime); 

		Vector3 axis; 
		rot.ToAngleAxis(out CurrentAngle, out axis); 
	}

	void Shoot()
	{
		GameObject p = Instantiate(Projectile, Barrel.position, transform.rotation*Projectile.transform.rotation) as GameObject; 
		Destroy(p, DestructionTime); 
	}
		



}
