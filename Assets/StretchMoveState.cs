using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoHell.Animation;

public class StretchMoveState : StateMachineBehaviour
{

    private AnimationController instance;
    private Vector3 targetScale = new Vector3(0.5f, 7f, 1f);
    private Vector3 targetPos = new Vector3(15f, 0, 0);
    private Vector3 InitialPos;
    private Vector3 InitialScale;

    private float delayFromPreviousState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        

        instance = AnimationController.instance;
        InitialPos = instance._player.transform.position;
        InitialScale = instance._player.transform.localScale;
       

        Color col = instance.nebulaMat.GetColor("_Color") * 3f;

        instance._player.GetComponent<SpriteRenderer>().material.SetColor("_Color", col);

        delayFromPreviousState = animator.GetFloat("DelayCharge");

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        instance.AnimateNebula();

        var time = instance.time - delayFromPreviousState;


        //var evalCurve = instance.scrollSpeedCurve.Evaluate(speed);
        var evalCurve = instance.scrollSpeedCurve.Evaluate(time);


        //var scale = instance._player.transform.localScale;
        var scale = InitialScale;

        //instance._player.transform.localScale = Vector3.Lerp(scale, targetScale, ScaleTime);
        instance._player.transform.localScale = Vector3.Lerp(scale, targetScale, time);

        //ScaleTime = (scale - targetScale).magnitude * Time.deltaTime * instance.AnimSpeed ;


        //var pos = instance._player.transform.position;
        var pos = InitialPos;

        instance._player.transform.position = Vector3.Lerp(pos, targetPos, evalCurve);

        //PosTime = ((pos - InitialPos).magnitude * Time.deltaTime * instance.AnimSpeed) * evalCurve;

       

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
