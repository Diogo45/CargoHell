using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitScoreUIManager : MonoBehaviour
{

    [SerializeField] private Button setHighScoreButton;
    [SerializeField] private Button closeScreenButton;

    [SerializeField] private TMPro.TMP_InputField playerNameField;


    private HighScoreManager _highScoreManager;

    private void Start()
    {

        _highScoreManager = HighScoreManager.instance;

        if (!_highScoreManager)
            Debug.LogError("There's not  have a High Score Manager reference!");


        playerNameField.onValueChanged.AddListener(_highScoreManager.InputName);
        setHighScoreButton.onClick.AddListener(SetScore);
        closeScreenButton.onClick.AddListener(CloseScreen);

    }

    private void CloseScreen()
    {
        gameObject.SetActive(false);
    }

    private void SetScore()
    {
        if(_highScoreManager.status != HighScoreManager.Status.Writing)
            _highScoreManager.StartWriteScore();
        
    }
}
