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
        //public int side;
        //public float posInSide;
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

    [SerializeField]
    public Sprite planetSprite;

    public List<Wave> LevelConfig = new List<Wave>();

    public ShaderVar ShaderVariables = new ShaderVar
    {
        color = Color.blue
    };

    public int LevelMusicIndex;

    public int LevelClipIndex;

    public static Vector3 CorrectViewportPosition(EnemyConfig enemy)
    {
        var pos = enemy.viewportPosition;
        if (Mathf.Round(Mathf.Abs(enemy.direction.x)) != 0.0f)
        {            
            if (pos.x > 0.5f)
            {
                pos.x += 0.1f;
            }
            else if (pos.x < 0.5f)
            {
                pos.x -= 0.1f;
            }
        }
        else if (Mathf.Round(Mathf.Abs(enemy.direction.y)) != 0.0f)
        {
            if (pos.y > 0.5f)
            {
                pos.y += 0.1f;
            }
            else if (pos.y < 0.5f)
            {
                pos.y -= 0.1f;
            }
        }       

        return pos;
    }

}