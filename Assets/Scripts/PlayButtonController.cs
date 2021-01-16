using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PlayButtonController : MonoBehaviour
{
    public AudioMixer globalMixer;

    public List<GameObject> nonOptionsMenu;
    public List<GameObject> OptionsMenu;

    public void SetLevel(float sliderValue)
    {
        //Decibels are NOT LINEARRR AAA
        //TODO: Store Volume and other preferences on PlayerPrefs!!!
        globalMixer.SetFloat("GlobalVolume", Mathf.Log10(sliderValue) * 20f);
    }

    public void ShowOptions()
    {
        foreach (var item in nonOptionsMenu)
        {
            item.SetActive(false);
        }

        foreach (var item in OptionsMenu)
        {
            item.SetActive(true);
        }
    }

    public void HideOptions()
    {
        foreach (var item in nonOptionsMenu)
        {
            item.SetActive(true);
        }

        foreach (var item in OptionsMenu)
        {
            item.SetActive(false);
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Leve0_1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Leve1_2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Leve2_3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Leve3_4()
    {
        SceneManager.LoadScene("Level4");
    }
}
