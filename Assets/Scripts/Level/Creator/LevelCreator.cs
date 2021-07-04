using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelCreator : Singleton<LevelCreator>
{


    [field: SerializeField] public Level _level { get; private set; }
    public int WaveNumber { get; private set; }

    private string _levelSavePath;

    private void Awake()
    {
        base.Awake();

        _levelSavePath = Application.dataPath + @"/Levels/";
    }



    public void CreateLevel(string name)
    {
        _level = ScriptableObject.CreateInstance<Level>();
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(_level, _levelSavePath + name + ".asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif

#if !UNITY_EDITOR
        Debug.LogError("Level Editor CREATE LEVEL not implemented in builded version yet");
#endif
    }

    public void SaveLevel()
    {

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif

#if !UNITY_EDITOR
        Debug.LogError("Level Editor SAVE LEVEL not implemented in builded version yet");
#endif

    }


    public void NextWave()
    {
        if(WaveNumber + 1 < _level.LevelConfig.Count)
            WaveNumber += 1;
    }

    public void PreviousWave()
    {
        if(WaveNumber > 0)
            WaveNumber -= 1;
    }

    public void AddWave()
    {
        _level.LevelConfig.Add(new Level.Wave() { enemies = new List<Level.EnemyConfig>() });

    }

    public void DeleteWave()
    {
        _level.LevelConfig.Remove(_level.LevelConfig[WaveNumber]);
    }


    public void Save(List<GameObject> enemies)
    {
        Level.Wave currentWave = _level.LevelConfig[WaveNumber];

        currentWave.enemies.Clear();

        for (int i = 0; i < enemies.Count; i++)
        {
            AddEnemy(enemies[i].GetComponent<EnemyData>());

        }

        SaveLevel();

    }

    public void AddEnemy(EnemyData data)
    {
        Level.Wave currentWave = _level.LevelConfig[WaveNumber];

        currentWave.enemies.Add(new Level.EnemyConfig
        {
            enemyType = data.enemyType,
            viewportPosition = Camera.main.WorldToViewportPoint(data.gameObject.transform.position),
            direction = data.gameObject.transform.up,
            delay = data.Delay,
            shouldMove = data.ShouldMove,
            speed = data.Speed
        });


    }


    public List<Level.EnemyConfig> currentWaveEnemies()
    {
        return _level.LevelConfig[WaveNumber].enemies;
    }


    public static bool HitEnemy(out GameObject selected)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray { origin = mousePos, direction = Vector3.forward });

        if (hit && hit.transform.tag == "Enemy")
        {
            selected = hit.transform.gameObject;
            return true;
        }

        selected = null;

        return false;

    }

}
