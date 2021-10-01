using System;
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
            int levelIndex = i + (pageNumber * 6);
            var level = Pages[pageNumber].transform.Find((levelIndex + 1).ToString());
            if (levelIndex <= unlockedLevels)
            {
                level.gameObject.SetActive(true);
                level.gameObject.GetComponent<ImageAnimation>().enabled = false;
            }
            else
            {
                level.gameObject.GetComponent<Button>().interactable = false;
                level.gameObject.GetComponent<ImageAnimation>().enabled = true;

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
