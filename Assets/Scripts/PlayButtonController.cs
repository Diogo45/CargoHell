using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PlayButtonController : MonoBehaviour
{


    public GameObject DebugUIPrefab;
    private GameObject DebugUI;

    public GameObject Score;
    private TMPro.TMP_Text scoreText;

    public AudioMixer globalMixer;

    public GameObject nonOptionsMenu;
    public GameObject OptionsMenu;

    public GameObject GlobalVolume;
    public GameObject SFXVolume;

    public void Start()
    {
        DebugUI = Instantiate(DebugUIPrefab, GameObject.FindGameObjectWithTag("canvas").transform);
        globalMixer.SetFloat("GlobalVolume", Mathf.Log10(PlayerPrefs.GetFloat("GlobalVolume")) * 20f);
        globalMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20f);
        if(GlobalVolume)
            GlobalVolume.GetComponent<Slider>().value = PlayerPrefs.GetFloat("GlobalVolume");
        if(SFXVolume)
            SFXVolume.GetComponent<Slider>().value = PlayerPrefs.GetFloat("SFXVolume");

    }
    private void Update()
    {
        if (Score)
        {
            scoreText = Score.GetComponent<TMPro.TMP_Text>();
            scoreText.text = LevelController.instance.Score.ToString();
        }
        
    }

    public void ToggleDebugUI()
    {
        DebugUI.SetActive(!DebugUI.activeSelf);
    }

    public void SetLevel(float sliderValue)
    {
        //Decibels are NOT LINEARRR AAA
        //TODO: Store Volume and other preferences on PlayerPrefs!!!
        globalMixer.SetFloat("GlobalVolume", Mathf.Log10(sliderValue) * 20f);
        PlayerPrefs.SetFloat("GlobalVolume", sliderValue);

    }

    public void SetSFXLevel(float sliderValue)
    {
        //Decibels are NOT LINEARRR AAA
        //TODO: Store Volume and other preferences on PlayerPrefs!!!
        globalMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20f);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }

    public void ShowOptions()
    {

        nonOptionsMenu.SetActive(false);
        OptionsMenu.SetActive(true);
      
    }

    public void HideOptions()
    {
        nonOptionsMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    public void SetJoystickType(int type)
    {
        PlayerPrefs.SetInt("InputType", type);
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
