using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDefinition")]
public class Level : ScriptableObject
{
    [System.Serializable]
    public struct EnemyConfig
    {
        public EnemyType enemyType;
        public Vector2 viewportPosition;
        public Vector2 direction;
        public int side;
        public float posInSide;
        public float delay;
        public float speed;
        public bool shouldMove;
    }

    [System.Serializable]
    public struct ShaderVar
    {
        public Color color;

        //public float2 offset;

    }

    [System.Serializable]
    public struct Wave
    {
        [SerializeField]
        public List<EnemyConfig> enemies;
    }

    public List<Wave> LevelConfig = new List<Wave>();

    public ShaderVar ShaderVariables = new ShaderVar
    {
        color = Color.blue
    };

    public int LevelMusic;

  

}