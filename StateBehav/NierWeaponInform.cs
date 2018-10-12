using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NierWeaponInform : StateMachineBehaviour {

	public enum WeaponState {idle, combat};
	public WeaponState weapon_state = WeaponState.idle; 
	public bool OnEnter = true; 

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
		if(OnEnter)
			Call(animator); 
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	// override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

	// }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(!OnEnter)
			Call(animator); 
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	// override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

	// }


	void Call(Animator animator)
	{
		string state = weapon_state == WeaponState.idle ? "idle" : "combat"; 
		animator.gameObject.GetComponent<NierModular>().WeaponInform(state);
	}
}
