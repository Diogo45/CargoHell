using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> scoresUI;

    [SerializeField] private GameObject scorePlacePrefab;

    [SerializeField] private GameObject ScrollViewContent;

    private int _id;
    public void Initialize(int id)
    {
        _id = id;
    }


    private IEnumerator Start()
    {

        scoresUI = new List<GameObject>();

        var first = ScrollViewContent.transform.Find("1st");
        var second = ScrollViewContent.transform.Find("2nd");
        var third = ScrollViewContent.transform.Find("3rd");

        GameObject[] initial = {first.gameObject, second.gameObject, third.gameObject };

        scoresUI.AddRange(initial);

        //TODO:For now waits until getting all the scores from every player, maybe in the future should have a limit -> 128?
        yield return new WaitUntil(() => ScoreboardDataManager.instance.status == ScoreboardDataManager.RetrivalStatus.Done);

        StartFillScores();


    }

    


    public void StartFillScores()
    {

        var scores = ScoreboardDataManager.instance.scores;


        List<(Score score, int value)> highScores = new List<(Score,int)>();

        for (int i = 0; i < scores.Count; i++)
        {

            if (scores[i].high_scores.Count <= _id)
                continue;

            int levelHighScore = scores[i].high_scores[_id];

            highScores.Add((scores[i], levelHighScore));

        }

        var array = highScores.ToArray();

        Array.Sort(array, (x,y) => x.value < y.value ? 1 : -1);

        highScores = array.ToList();

      

        for (int i = 0; i < highScores.Count; i++)
        {
            GameObject podiumPlace = null;

            if(i < scoresUI.Count)
            {
                podiumPlace = scoresUI[i];
            }
            else
            {
                podiumPlace = Instantiate(scorePlacePrefab, ScrollViewContent.transform);
            }


            //TODO:In future prefetch these
           
            TMPro.TMP_Text playerName = podiumPlace.transform.Find("PlayerName").GetComponent<TMPro.TMP_Text>();
            TMPro.TMP_Text score = podiumPlace.transform.Find("PlayerScore").GetComponent<TMPro.TMP_Text>();

            if(i >= scoresUI.Count)
            {
                 TMPro.TMP_Text place = podiumPlace.transform.Find("Place").GetComponent<TMPro.TMP_Text>();
                place.text = (i + 1) + "th";
            }

            playerName.text = highScores[i].score.name;
            score.text = highScores[i].value.ToString();

        }


    }


}
