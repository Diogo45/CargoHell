using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIController : MonoBehaviour
{


    public GameObject DebugUIPrefab;
    private GameObject DebugUI;

    public GameObject Score;
    private TMPro.TMP_Text scoreText;

    public AudioMixer globalMixer;

    public GameObject nonOptionsMenu;
    public GameObject OptionsMenu;
    public GameObject LevelSelector;

    public GameObject GlobalVolumeSlider;
    public GameObject SFXVolumeSlider;

    public void Start()
    {
        DebugUI = Instantiate(DebugUIPrefab, GameObject.FindGameObjectWithTag("canvas").transform);

        globalMixer.SetFloat("GlobalVolume", Mathf.Log10(PlayerPrefs.GetFloat("GlobalVolume")) * 20f);
        globalMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20f);

        if(GlobalVolumeSlider)
            GlobalVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("GlobalVolume");
        if(SFXVolumeSlider)
            SFXVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("SFXVolume");

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

    public void ShowLevelSelector()
    {

        nonOptionsMenu.SetActive(false);
        LevelSelector.SetActive(true);

    }

    public void HideLevelSelector()
    {

        nonOptionsMenu.SetActive(true);
        LevelSelector.SetActive(false);

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

    public void Level1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Level4()
    {
        SceneManager.LoadScene("Level4");
    }

    public void Level5()
    {
        SceneManager.LoadScene("Level5");
    }
}
