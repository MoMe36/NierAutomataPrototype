using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NierHitbox : MonoBehaviour {

	public enum BoxType {hit, hurt};
	public BoxType Type = BoxType.hit;
	public GameObject ImpactEffect;  
	public string Name = "myhitbox"; 
	public bool Active = false; 

	
	void OnTriggerEnter(Collider other)
	{
		if(Active)
		{
			NierHitbox other_hb = other.GetComponent<NierHitbox>(); 
			if(other_hb != null)
			{
				if(other_hb.Type == NierHitbox.BoxType.hurt)
				{
					other_hb.Impacted(); 
					CreateEffect(transform.position); 
				}
			}
		}

	}

	void CreateEffect(Vector3 position)
	{
		GameObject p = Instantiate(ImpactEffect, position, ImpactEffect.transform.rotation) as GameObject; 
	}

	public void Impacted()
	{
		return ; 
	}



}
