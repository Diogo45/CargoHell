using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CargoHell {
    public class EndLevelManager : MonoBehaviour
    {

        [SerializeField] private GameObject WinCanvas;
        [SerializeField] private GameObject LoseCanvas;

        private void EndLevelAnimation_onEndLevelAnim()
        {
            WinCanvas.SetActive(true);
            var button = WinCanvas.transform.Find("PlayButton").gameObject;
            //Debug.Log(button);

            EventSystem.current.SetSelectedGameObject(button);
        }

        private void LevelController_onEndLevel(bool win)
        {
            //Debug.Log("ENEND");
            if (!win)
            {
                LoseCanvas.SetActive(true);
                var button = LoseCanvas.transform.Find("PlayButton").gameObject;
                Debug.Log(button);
                EventSystem.current.SetSelectedGameObject(button);
            }
            else
            {
                var currentUnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels");

                if(currentUnlockedLevels <= LevelController._levelID)
                {
                    PlayerPrefs.SetInt("UnlockedLevels", LevelController._levelID + 1);
                    Debug.Log(PlayerPrefs.GetInt("UnlockedLevels"));

                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        private void OnEnable()
        {
            Animation.EndLevelAnimation.onEndLevelAnim += EndLevelAnimation_onEndLevelAnim;
            LevelController.onEndLevel += LevelController_onEndLevel;
        }

        

        private void OnDisable()
        {
            Animation.EndLevelAnimation.onEndLevelAnim -= EndLevelAnimation_onEndLevelAnim;
            LevelController.onEndLevel -= LevelController_onEndLevel;

        }

    }
}