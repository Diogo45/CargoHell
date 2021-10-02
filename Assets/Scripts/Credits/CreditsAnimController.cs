using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsAnimController : MonoBehaviour
{
    [SerializeField] private Material nebulaMat;
    [SerializeField] private Color nebulaColor;
    [SerializeField] private GameObject Stars;


    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator nebulaAnimator;
    [SerializeField] private AnimationClip _playerEndAnim;
    [SerializeField] private AnimationClip _playerCreditsAnim;

    [SerializeField] private GameObject CreditsManager;

    private float timer;

    void Start()
    {
        nebulaMat.SetColor("_Color", nebulaColor);
        playerAnimator.Play("CreditAnim");
        timer = _playerCreditsAnim.length;
        StartCoroutine(StartEndLevelAnim());
    }

    private IEnumerator StartEndLevelAnim()
    {
        yield return new WaitForSeconds(timer);
        Stars.SetActive(false);
        playerAnimator.Play("EndLevel");
        nebulaAnimator.Play("EndLevel");
        timer = _playerEndAnim.length;
        StartCoroutine(RollCredits());
    }

    private IEnumerator RollCredits()
    {
        yield return new WaitForSeconds(timer);
        CreditsManager.SetActive(true);
    }
}
