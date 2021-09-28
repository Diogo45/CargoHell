using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CargoHell {
    public class EndLevelManager : MonoBehaviour
    {

        [SerializeField] private GameObject WinCanvas;
        [SerializeField] private GameObject LoseCanvas;

        private void EndLevelAnimation_onEndLevelAnim()
        {
            WinCanvas.SetActive(true);
        }

        private void LevelController_onEndLevel(bool win)
        {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            
            if (!win)
            {
                LoseCanvas.SetActive(true);
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