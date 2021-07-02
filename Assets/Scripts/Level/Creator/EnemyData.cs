using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public delegate void OnChangeEnemyData(int index, EnemyData data);
    public static event OnChangeEnemyData onChangeEnemyData;

    public EnemyType enemyType;
    public float Speed;
    public float Delay = 0f;
    public bool ShouldMove = true;


    public int index;
}
