using CargoHell.Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CargoHell.Animation
{
    public class EndLevelAnimation : MonoBehaviour
    {
        public delegate void OnEndLevelAnim();
        public static event OnEndLevelAnim onEndLevelAnim;

        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Animator nebulaAnimator;

        [SerializeField] private AnimationClip _playerEndAnim;

        private float timer;

        private float time;

        private void LevelController_onEndLevel(bool win)
        {
            if (win)
            {

                var initialPos = AnimationController.instance._player.transform.position;
                var final = Vector3.zero;
                
                AnimationController.instance._player.GetComponent<PlayerController>().Movement = false;
                AnimationController.instance._shield.SetActive(false);

                StartCoroutine(Centering(initialPos, final));
            }

        }

        private void Play()
        {
            AnimationController.instance.SetStars(false);
            playerAnimator.enabled = true;
            playerAnimator.Play("EndLevel");
            nebulaAnimator.Play("EndLevel");

            timer = _playerEndAnim.length;
            StartCoroutine(TriggerEvent());
        }

        private IEnumerator TriggerEvent()
        {
            yield return new WaitForSeconds(timer);

            onEndLevelAnim?.Invoke();
        }

        private IEnumerator Centering( Vector3 InitialPos, Vector3 FinalPos)
        {
            while (true)
            {
                yield return new WaitForSeconds(Time.deltaTime);

                AnimationController.instance._player.transform.position = Vector3.Lerp(InitialPos, FinalPos, time);
                //time = ((Pos - InitialPos).magnitude * Time.deltaTime);
                //Debug.Log(time);
                time += Time.deltaTime;

                AnimationController.instance._player.transform.up = (FinalPos - InitialPos).normalized;

                if (Vector3.Distance(AnimationController.instance._player.transform.position, FinalPos) < 0.1f)
                {
                    Play();
                    yield return null;
                }
            }

            
        }

        private void Update()
        {
            
        }


        private void OnEnable()
        {
            LevelController.onEndLevel += LevelController_onEndLevel;
            playerAnimator.enabled = false;
        }

        private void OnDisable()
        {
            LevelController.onEndLevel -= LevelController_onEndLevel;
        }

    }
}