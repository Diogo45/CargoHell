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
            if (!win)
            {
                LoseCanvas.SetActive(true);
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