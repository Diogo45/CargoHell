using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoHell.Animation;


public class CenteringState : StateMachineBehaviour
{

    private Vector3 InitialPos;
    private Vector3 Pos;
    private Vector3 FinalPos;

    private AnimationController instance;

    private float time;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        instance = AnimationController.instance;
        instance._player.GetComponent<PlayerController>().Movement = false;
        instance._shield.SetActive(false);
        InitialPos = instance._player.transform.position;
        FinalPos = Vector3.zero;
        time = Time.deltaTime;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Pos = instance._player.transform.position;
        instance._player.transform.position = Vector3.Lerp(Pos, FinalPos, time);
        time = ((Pos - InitialPos).magnitude * Time.deltaTime) * instance.AnimSpeed;

        instance._player.transform.up = (FinalPos - InitialPos).normalized;

        if (Vector3.Distance(instance._player.transform.position, FinalPos) < 0.1f)
        {
            animator.SetBool("PlayerCentered", true);
        }



    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        instance._player.transform.rotation = Quaternion.Euler(0, 0, -90);
        instance.Stars.SetActive(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
