using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoHell
{

    public class ScoreCounter : MonoBehaviour
    {
        private int[] intCount;
        private bool[] finishedCount;


        [SerializeField] private GameObject finalScoreCounter;


        // Start is called before the first frame update
        void Start()
        {
            var score = LevelController.instance.Score.ToString().ToCharArray();

            intCount = new int[score.Length];
            finishedCount = new bool[score.Length];
            StartCoroutine(countScore());

            for (int i = 0; i < score.Length; i++)
            {
                StartCoroutine(CountUp(i));
            }

            Audio.AudioController.instance.CountScore();

        }




        //SCORE
        IEnumerator countScore()
        {
            //AnimateNebula();

            yield return new WaitForSeconds(0.005f);

            var score = LevelController.instance.Score.ToString().ToCharArray();

            string strCount = "";

            if (!Array.Exists(finishedCount, x => x == false))
            {
                Audio.AudioController.instance.StopCountScore();
                yield break;
            }

            for (int i = 0; i < score.Length; i++)
            {
                strCount += intCount[i].ToString();
            }

            finalScoreCounter.GetComponent<TMPro.TMP_Text>().text = "SCORE \n" + strCount;

            yield return countScore();
            yield break;
        }

        //SCORE

        IEnumerator CountUp(int index)
        {
            var score = LevelController.instance.Score.ToString().ToCharArray();
            var counter = intCount[index];
            if (counter.ToString().ToCharArray()[0] == score[index] && ((index - 1 >= 0 && finishedCount[index - 1]) || index == 0))
            {
                finishedCount[index] = true;
                yield break;
            }

            if (counter >= 9)
            {
                counter = 0;

            }
            else
            {
                counter += 1;
            }
            intCount[index] = counter;
            yield return new WaitForSeconds(0.1f * (score.Length - (index + 1)));
            yield return CountUp(index);
            yield break;
        }

    }
}