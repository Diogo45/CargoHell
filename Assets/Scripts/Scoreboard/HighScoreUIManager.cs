using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUIManager : MonoBehaviour
{

    [SerializeField] private Button setHighScoreButton;
    [SerializeField] private GameObject setHighScoreScreen;

    [SerializeField] private Button showScoreboardButton;  
    [SerializeField] private GameObject scoreboardScreen;


    private void Awake()
    {
        setHighScoreButton.onClick.AddListener(ShowHighScoreScreen);
        showScoreboardButton.onClick.AddListener(ShowScoreboardScreen);
    }

    private void ShowScoreboardScreen()
    {
        scoreboardScreen.SetActive(true);
    }

    private void ShowHighScoreScreen()
    {
        setHighScoreScreen.SetActive(true);
    }
}
