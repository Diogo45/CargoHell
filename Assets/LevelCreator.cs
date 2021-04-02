using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private EnemyList _enemyList;
    public int WaveNumber { get; private set; }

    private string _levelSavePath;

    private void Awake()
    {
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

    public void AddEnemy(EnemyType enemy, Vector2 screenPosition)
    {
        Level.Wave currentWave = _level.LevelConfig[WaveNumber];
        currentWave.enemies.Add(new Level.EnemyConfig 
        {
            enemyType = enemy,
            side = (int)screenPosition.x,
            posInSide = screenPosition.y,
            
        });
    }

}
