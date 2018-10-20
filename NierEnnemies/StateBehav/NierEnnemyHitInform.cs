using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NierEnnemyHitInform : StateMachineBehaviour {

	public NierHitData HitData; 

	bool ready = true; 

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// animator.SetFloat("HitNumber", Random.Range(0, MaxRandom)); 

		ready = true; 
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
		if(ready)
		{
			if(stateInfo.normalizedTime >= HitData.ActivationTime)
			{
				Call(animator, true); 
				ready = false; 
			}
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
		Call(animator, false);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	// override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

	// }

	void Call(Animator animator, bool state)
	{
		animator.gameObject.GetComponent<NierEnnemyModular>().HitInform(HitData, state); 
	}	

}
