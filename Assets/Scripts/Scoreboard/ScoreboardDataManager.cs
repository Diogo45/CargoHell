using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardDataManager : Singleton<ScoreboardDataManager>
{

    public enum RetrivalStatus
    {
        None, Retriving, Done
    }

    private List<string> _names;

    public List<Score> scores { get; private set; }

    private int scoresRetrivied = 0;

    public RetrivalStatus status { get; private set; } = RetrivalStatus.None;

    // Start is called before the first frame update
    private void Awake()
    {
        base.Awake();

        scores = new List<Score>();

        var scoreboards = transform.GetComponentsInChildren<ScoreboardManager>(includeInactive: true);


        for (int i = 0; i < scoreboards.Length; i++)
        {
            scoreboards[i].Initialize(i);
        }
    }


    void OnEnable()
    {

        scores = new List<Score>();

        StartCoroutine(FirebaseManager.instance.Get<Names>("Names", GetScores));

    }

    private void OnDisable()
    {
        status = RetrivalStatus.None;
    }

    private void GetScores(Names obj)
    {
        if (obj == null)
        {
            Debug.LogError("NO NAMES ON DATABASE");
            return;
        }

        status = RetrivalStatus.Retriving;
        scoresRetrivied = 0;

        _names = obj.names;

        for (int i = 0; i < obj.names.Count; i++)
        {
            StartCoroutine(FirebaseManager.instance.Get<Score>(obj.names[i], AddScore));
        }
    }

    private void AddScore(Score obj)
    {
        if (obj == null || obj == default(Score))
            return;

        scores.Add(obj);
        scoresRetrivied++;

        if(scoresRetrivied == _names.Count)
        {
            status = RetrivalStatus.Done;
        }

    }


}
