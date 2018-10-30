using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitInform : StateMachineBehaviour {

	public NierHitData HitData;   
	

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Call(animator, true); 
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	// override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
		// if(ready)
		// {
		// 	timer -= Time.deltaTime; 
		// 	if(timer <= 0f)
		// 	{
		// 		ready = false; 
		// 		Call(animator, true); 
		// 	}
		// }
	

	// }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Call(animator, false); 	
	}

	void Call(Animator animator, bool state)
	{
		animator.gameObject.GetComponent<BossModular>().HitInform(HitData, state); 
	}	

}
