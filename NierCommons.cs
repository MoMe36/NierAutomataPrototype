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


}
public struct NierHitData
{
	public string HitboxName;
	public float ImpulsionStrength;  
	public Vector3 ImpulsionDirection; 

}