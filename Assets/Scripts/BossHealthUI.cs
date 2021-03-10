using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthUI : MonoBehaviour
{
    // Start is called before the first frame update

    private UnityEngine.UI.Slider slider;
    private float initialHealth;
    public BossController bossController;

    void Start()
    {

        slider = GetComponent<UnityEngine.UI.Slider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bossController)
            slider.value = bossController.health / initialHealth;
    }

   

    private void LevelController_onEndLevel(bool win)
    {
        gameObject.SetActive(false);
    }

  

    private void LevelController_onSpawnEnemy(IEnemy controller)
    {
        if (controller.type == EnemyType.BOSS)
        {
            bossController = FindObjectOfType<BossController>();
            initialHealth = bossController.health;
            LevelController.onSpawnEnemy -= LevelController_onSpawnEnemy;
        }
    }

    private void OnEnable()
    {
        LevelController.onSpawnEnemy += LevelController_onSpawnEnemy;
        LevelController.onEndLevel += LevelController_onEndLevel;
    }

    private void OnDisable()
    {
        LevelController.onSpawnEnemy -= LevelController_onSpawnEnemy;
        LevelController.onEndLevel -= LevelController_onEndLevel;
    }

}
