using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Globals : MonoBehaviour
{

	public static Dictionary <string, NierHitbox> FillHitboxes(List <NierHitbox> boxes)
	{
		Dictionary <string, NierHitbox> dict = new Dictionary<string, NierHitbox>(); 
		for(int i = 0; i<boxes.Count; i++)
		{
			dict.Add(boxes[i].Name, boxes[i]); 
		}
		return dict;
	}


	public static void FillAllBoxes(NierHitbox [] boxes, out Dictionary <string, NierHitbox> hit_dict, out Dictionary <string, NierHitbox> hurt_dict)
	{

		List <NierHitbox> hit = new List <NierHitbox>();  
		List <NierHitbox> hurt = new List <NierHitbox>();  

		foreach(NierHitbox box in boxes)
		{
			if(box.Type == NierHitbox.BoxType.hit)
				hit.Add(box); 
			else
				hurt.Add(box); 
		}

		hit_dict = FillHitboxes(hit); 
		hurt_dict = FillHitboxes(hurt); 

	}

	public static void RotateTowardsFlat(Transform character_transform, Vector3 position, float rotation_speed)
	{
		Vector3 direction = Vector3.ProjectOnPlane(position - character_transform.position, Vector3.up); 
		Quaternion rotation = Quaternion.FromToRotation(character_transform.forward, direction); 

		character_transform.rotation = Quaternion.Lerp(character_transform.rotation, rotation*character_transform.rotation, rotation_speed*Time.deltaTime); 
	}



	public static Transform [] CleanArray(Transform [] targets)
	{
		List<Transform> cleaned = new List<Transform>(); 
		foreach(Transform t in targets)
		{
			if(t != null)
				cleaned.Add(t); 
		}

		return cleaned.ToArray(); 
	}


}


[System.Serializable]
public struct NierHitData
{
	public string HitboxName;
	public float ImpulsionStrength;  
	public Vector3 ImpulsionDirection; 
	public float HitForce; 
	public float ActivationTime; 

}