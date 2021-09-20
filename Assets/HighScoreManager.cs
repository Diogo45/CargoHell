using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Score
{
    public string name;
    public List<int> high_scores;
    public int total_score;


    public override string ToString()
    {
        return total_score.ToString();
    }

}

[System.Serializable]
public class Names
{
    public List<string> names;
}


public class HighScoreManager : MonoBehaviour
{

    private string scoreboardName = "";
    private string editedName;

    private List<GameObject> scoresUI;

    [SerializeField] private GameObject scorePlacePrefab;


    

    [SerializeField] private GameObject ScrollViewContent;


    public void InputName(string s)
    {
        scoreboardName = s;

        PlayerPrefs.SetString("PlayerName", scoreboardName);

    }

    public void StartWriteScore()
    {
        StartCoroutine(FirebaseManager.instance.Get<Score>(scoreboardName, WriteScore));
    }

    private void WriteScore(Score dbScore)
    {
        if(dbScore == default(Score) || dbScore == null)
        {
            //CREATE NEW SCORE
            dbScore = new Score();
            dbScore.name = scoreboardName;
        }

        if (dbScore.high_scores == null)
        {
            //Create TO CURRENT LEVEL SIZE
            dbScore.high_scores = new List<int>();
        }

        int currentLevel = CargoHell.LevelController._levelID + 1;
        //int levelScore = CargoHell.LevelController.instance.Score;
        int levelScore = 0;

        if (dbScore.high_scores.Count < currentLevel)
        {
            //ADD TO SIZE
            dbScore.high_scores.AddRange(new int[currentLevel - dbScore.high_scores.Count]);
        }

        //ADD SCORE
        dbScore.high_scores.Insert(currentLevel - 1, levelScore);

        dbScore.total_score = 0;

        for (int i = 0; i < dbScore.high_scores.Count; i++)
        {
            dbScore.total_score += dbScore.high_scores[i];
        }


        //Update database
        FirebaseManager.instance.Put(dbScore.name, dbScore);


        StartWriteToNameList(dbScore.name);

    }

    private void StartWriteToNameList(string name)
    {
        editedName = name;
        StartCoroutine(FirebaseManager.instance.Get<Names>("Names", WriteToNameList));
    }

    private void WriteToNameList(Names nameObj)
    {
        if (nameObj == null)
            nameObj = new Names();

        if (nameObj.names == null)
        {
            nameObj.names = new List<string>();
        }
                
        nameObj.names.Add(editedName);
        FirebaseManager.instance.Put("Names", nameObj);
        editedName = "";
    }

    public void FillScores()
    {

    }

    public void GetScore()
    {
        StartCoroutine(FirebaseManager.instance.Get<Score>("", ShowScore));
    }

    private void ShowScore(Score score)
    {
        Debug.Log(score);
    }






}
