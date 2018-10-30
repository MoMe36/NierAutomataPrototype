using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDecisionState : StateMachineBehaviour {

	public Vector2 TimerBounds; 
	public float DecisionTimer; 
	public BossActionData bad; 


	bool ready = true; 


	public float timer; 

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		ResetTimer(); 
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
		if(ready)
		{
			timer -= Time.deltaTime; 
			if(timer <= 0f)
			{
				ready = false; 
				Call(animator, true); 
			}
		}
	

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	// override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	// 	ResetTimer(); 	
	// }


	void ResetTimer()
	{
		DecisionTimer = Random.Range(TimerBounds.x, TimerBounds.y); 
		timer = DecisionTimer; 
		ready = true; 
	}

	void Call(Animator animator, bool state)
	{
		animator.gameObject.GetComponent<BossModular>().InformBoss(bad); 
	}	

}
