using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{

    public enum FadeState
    {
        None, Done
    }

    [SerializeField] private GameObject CreditsScreen;

    [SerializeField] private float ChangeScreenTimer;

    private List<GameObject> _creditPanels;

    private int _currentScreen;

    private TMPro.TMP_Text[] _screenTexts;

    private FadeState fadeState;

    [SerializeField] private float FadeSpeed;

    [SerializeField] private float alpha;

    void Start()
    {
        _creditPanels = new List<GameObject>();

        for (int i = 0; i < CreditsScreen.transform.childCount; i++)
        {
            _creditPanels.Add(CreditsScreen.transform.GetChild(i).gameObject);
        }

        _screenTexts = _creditPanels[_currentScreen].GetComponentsInChildren<TMPro.TMP_Text>();
        foreach (var item in _screenTexts)
        {
            item.alpha = 0;
        }

        fadeState = FadeState.None;

        StartCoroutine(StartChangeScreens());

    }

    public void StartCredits()
    {
        StartCoroutine(StartChangeScreens());
    }


    IEnumerator StartChangeScreens()
    {
        _creditPanels[_currentScreen].SetActive(true);


        Debug.Log("Start Fade In");

        fadeState = FadeState.None;

        StartCoroutine(FadeTransition(true));

        //FADE IN

        yield return new WaitUntil(() => fadeState == FadeState.Done);

        Debug.Log("Done Fade In");


        yield return new WaitForSeconds(ChangeScreenTimer);

        

        fadeState = FadeState.None;

        

        //FADE IN
        Debug.Log("Start Fade Out");



        StartCoroutine(FadeTransition(false));
        //FADE OUT
        yield return new WaitUntil(() => fadeState == FadeState.Done);

        _creditPanels[_currentScreen].SetActive(false);


        if (_currentScreen < _creditPanels.Count)
        {
            _currentScreen++;
            _screenTexts = _creditPanels[_currentScreen].GetComponentsInChildren<TMPro.TMP_Text>();
            foreach (var item in _screenTexts)
            {
                item.alpha = 0;
            }
        }
        else
        {
            SceneManager.LoadSceneAsync("Menu");
            yield break;
        }


        yield return StartChangeScreens();


    }


    IEnumerator FadeTransition(bool dir)
    {
        if (dir)
        {
            for (int i = 0; i < _screenTexts.Length; i++)
            {
                var textAlpha = _screenTexts[i].alpha;

                _screenTexts[i].alpha = Mathf.Lerp(textAlpha, 1, Time.deltaTime * FadeSpeed);

                if(_screenTexts[i].alpha >= 0.95)
                {
                    fadeState = FadeState.Done;
                    yield break;
                }

            }

        }
        else
        {
            for (int i = 0; i < _screenTexts.Length; i++)
            {
                var textAlpha = _screenTexts[i].alpha;

                _screenTexts[i].alpha = Mathf.Lerp(textAlpha, 0, Time.deltaTime * FadeSpeed);

                if (_screenTexts[i].alpha <= 0.05)
                {
                    fadeState = FadeState.Done;
                    yield break;
                }

            }
        }

        yield return new WaitForEndOfFrame();
        yield return FadeTransition(dir);
    }


    // Update is called once per frame
    void Update()
    {
       
        alpha = _screenTexts[0].alpha;
    }
}
