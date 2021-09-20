using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Score
{
    public List<int> high_scores;
    public int total_score;


    public override string ToString()
    {
        return total_score.ToString();
    }

}


public class HighScoreManager : MonoBehaviour
{

    private string name = "";

    private List<GameObject> scoresUI;

    [SerializeField] private GameObject scorePlacePrefab;
    [SerializeField] private GameObject ScrollViewContent;


    public void InputName(string s)
    {
        name = s;

        PlayerPrefs.SetString("PlayerName", name);

    }

    public void WriteScore()
    {
        StartCoroutine(FirebaseManager.instance.Get<Score>(name, WriteScore));
    }

    private void WriteScore(Score dbScore)
    {
        if(dbScore != default(Score))
        {
            //CREATE NEW SCORE
            dbScore = new Score();
        }

        if (dbScore.high_scores == null)
        {
            //Create TO CURRENT LEVEL SIZE
            dbScore.high_scores = new List<int>();
        }

        int currentLevel = CargoHell.LevelController._levelID;
        int levelScore = CargoHell.LevelController.instance.Score;

        if (dbScore.high_scores.Count < currentLevel)
        {
            //ADD TO SIZE
            dbScore.high_scores.AddRange(new int[currentLevel - dbScore.high_scores.Count]);
        }

        //ADD SCORE
        dbScore.high_scores.Insert(currentLevel - 1, levelScore);


        FirebaseManager.instance.Put(name, dbScore);

    }


    public void GetScore()
    {
        StartCoroutine(FirebaseManager.instance.Get<Score>("001", ShowScore));
    }

    private void ShowScore(Score score)
    {
        Debug.Log(score);
    }






}
