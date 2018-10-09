using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NierHitbox : MonoBehaviour {

	public enum BoxType {hit, hurt};
	public BoxType Type = BoxType.hit; 
	public string Name = "myhitbox"; 
	public bool Active = false; 

	
	void OnTriggerEnter(Collider other)
	{

		Debug.Log("Contact"); 

	}



}
