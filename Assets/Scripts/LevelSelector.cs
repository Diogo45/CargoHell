﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Pages;
    public GameObject ArrowLeft;
    public GameObject ArrowRight;

    public int page;
    public int maxPage;

    public UIController levelController;

    private int unlockedLevels;

    void Start()
    {
        unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels");
        Debug.Log(unlockedLevels);
        UpdatePage(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdatePage(int pageNumber)
    {
        Pages[pageNumber].SetActive(true);

        for (int i = 0; i < Pages[pageNumber].transform.childCount; i++)
        {
            var level = Pages[pageNumber].transform.GetChild(i);
            if (i <= unlockedLevels)
                level.gameObject.SetActive(true);
            else
            {
                //level.gameObject.SetActive(false);
                level.gameObject.GetComponent<Button>().interactable = false;

            }
                
        }

        


        try
        {
            Pages[pageNumber + 1].SetActive(false);

        }
        catch (IndexOutOfRangeException e)
        {

        }

        try
        {
            Pages[pageNumber - 1].SetActive(false);

        }
        catch (IndexOutOfRangeException e)
        {

        }


    }

    public void NextPage()
    {
        if (page < maxPage)
        {
            page++;
            UpdatePage(page);
            ArrowLeft.SetActive(true);
            if(page == maxPage)
            {
                ArrowRight.SetActive(false);
            }
        }
        else
        {
            ArrowRight.SetActive(false);
        }
    }

    public void PreviousPage()
    {
        if (page > 0)
        {
            page--;
            UpdatePage(page);
            ArrowRight.SetActive(true);
            if(page == 0)
            {
                ArrowLeft.SetActive(false);
            }

        }
        else
        {
            ArrowLeft.SetActive(false);
        }
    }



}
