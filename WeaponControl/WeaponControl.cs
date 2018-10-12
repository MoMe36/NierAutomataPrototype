using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour {

	public Transform ParentIdle; 
	public Transform ParentCombat; 


	Transform CurrentParent; 

	public enum WeaponStates {idle, combat};
	public WeaponStates current_state = WeaponStates.idle; 

	public float TransitionSpeed = 1f; 

	bool is_changing = false; 

	// Use this for initialization
	void Start () {
		
		CurrentParent = ParentIdle; 
	}
	
	// Update is called once per frame
	void Update () {


		if(is_changing)
		{
			GoToParent(); 
			is_changing = false; 
		}
		
	}

	void GoToParent()
	{
		transform.parent = CurrentParent; 
		transform.position = CurrentParent.position; 
		transform.rotation = CurrentParent.rotation; 
	}

	public void ChangeParent(string state)
	{

		if(state == "combat")
		{
			current_state = WeaponStates.combat; 
			is_changing = true; 
			CurrentParent = ParentCombat; 
		}
		else if(state == "idle")
		{
			current_state = WeaponStates.idle; 
			is_changing = true; 
			CurrentParent = ParentIdle; 
		}
	}
}
