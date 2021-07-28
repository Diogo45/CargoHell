using CargoHell.Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelAnimation : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator nebulaAnimator;

    private void OnEnable()
    {
        LevelController.onEndLevel += LevelController_onEndLevel;
        playerAnimator.enabled = false;

    }

    private void LevelController_onEndLevel(bool win)
    {
        if (win)
        {
            Debug.Log("WON");
            Play();
        }
            
    }

    private void Play()
    {
        AnimationController.instance.SetStars(false);
        playerAnimator.enabled = true;
        playerAnimator.Play("EndLevel");
        nebulaAnimator.Play("EndLevel");
    }


    private void OnDisable()
    {
        LevelController.onEndLevel -= LevelController_onEndLevel;
    }

}
