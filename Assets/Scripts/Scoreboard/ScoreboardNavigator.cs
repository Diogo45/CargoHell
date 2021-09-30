using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardNavigator : MonoBehaviour
{
    public GameObject[] Pages;
    public GameObject ArrowLeft;
    public GameObject ArrowRight;

    public int page;
    public int maxPage;

    void Start()
    {
        UpdatePage(0);
    }


    void UpdatePage(int pageNumber)
    {
        Pages[pageNumber].SetActive(true);

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

    public void Back()
    {
        gameObject.SetActive(false);
    }


    public void NextPage()
    {
        if (page < maxPage)
        {
            page++;
            UpdatePage(page);
            ArrowLeft.SetActive(true);
            if (page == maxPage)
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
            if (page == 0)
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
