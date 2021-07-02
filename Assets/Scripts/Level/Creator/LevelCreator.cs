using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : Singleton<LevelCreator>
{


    [SerializeField] private Level _level;
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

#if UNITY_STANDALONE
        Debug.LogError("Level Editor CREATE LEVEL not implemented in builded version yet");
#endif
    }

    public void SaveLevel()
    {

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif

#if UNITY_STANDALONE
        Debug.LogError("Level Editor SAVE LEVEL not implemented in builded version yet");
#endif

    }


    public void NextWave()
    {
        WaveNumber += 1;
    }

    public void PreviousWave()
    {
        WaveNumber += 1;
    }

    public void AddWave()
    {
        _level.LevelConfig.Add(new Level.Wave());
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

}
