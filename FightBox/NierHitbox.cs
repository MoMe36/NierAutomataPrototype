using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NierHitbox : MonoBehaviour {

	public string Name = "myhitbox"; 

	public enum BoxType {hit, hurt};
	public BoxType Type = BoxType.hit;
	public GameObject ImpactEffect;  
	public bool Active = false; 

	[Header("Refers to")]
	public NierModular PlayerReference; 
	public NierEnnemyModular EnnemyReference; 


	NierHitData current_hit_data; 

	void OnTriggerEnter(Collider other)
	{
		if(Active)
		{
			NierHitbox other_hb = other.GetComponent<NierHitbox>(); 
			if(other_hb != null)
			{
				if(other_hb.Type == NierHitbox.BoxType.hurt)
				{
					Vector3 normalized_direction = (other_hb.gameObject.transform.position - transform.position).normalized; 
					other_hb.Impacted(current_hit_data, normalized_direction); 
					CreateEffect(transform.position); 
				}
			}
		}

	}

	void CreateEffect(Vector3 position)
	{
		GameObject p = Instantiate(ImpactEffect, position, ImpactEffect.transform.rotation) as GameObject; 
	}

	public void Impacted(NierHitData impact_data, Vector3 direction)
	{
		if(PlayerReference != null)
		{
			PlayerReference.ImpactInform(impact_data);
		}
		else
		{
			EnnemyReference.ImpactInform(impact_data, direction);
		}
		return ; 
	}

	public void SetState(NierHitData data, bool state)
	{
		if(state)
		{
			current_hit_data = data; 
			Active = true; 
		}
		else
		{
			Active = false; 
		}
	}



}
