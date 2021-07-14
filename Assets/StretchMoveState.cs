using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoHell.Animation;

public class StretchMoveState : StateMachineBehaviour
{

    private AnimationController instance;
    private Vector3 targetScale = new Vector3(0.5f, 15f, 1f);
    private Vector3 targetPos = new Vector3(15f, 0, 0);
    private Vector3 InitialPos;

    private float ScaleTime;
    private float PosTime;
    private float speed = 0.05f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        instance = AnimationController.instance;
        InitialPos = instance._player.transform.position;
        PosTime = Time.deltaTime;
        ScaleTime = Time.deltaTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        instance.AnimateNebula();

        Color col = instance.nebulaMat.GetColor("_Color") * 3f;

        instance._player.GetComponent<SpriteRenderer>().material.SetColor("_Color",  col);
        var evalCurve = instance.scrollSpeedCurve.Evaluate(speed);


        var scale = instance._player.transform.localScale;

        instance._player.transform.localScale = Vector3.Lerp(scale, targetScale, ScaleTime);

        //ScaleTime = (scale - targetScale).magnitude * Time.deltaTime * instance.AnimSpeed ;
        
        
        var pos = instance._player.transform.position;

        instance._player.transform.position = Vector3.Lerp(pos, targetPos, PosTime);

        PosTime = ((pos - InitialPos).magnitude * Time.deltaTime * instance.AnimSpeed) * evalCurve;

        speed += Time.deltaTime;

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
