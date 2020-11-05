﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]public class GameObjectDictionary : SerializableDictionary<string, GameObject> { }
[System.Serializable]public class IntObjectDictionary : SerializableDictionary<string, int> { }

public class LevelController : MonoBehaviour
{

    public static LevelController instance;

    public GameObject explosionAnim;
     
    public GameObjectDictionary enemyTypes;

    public IntObjectDictionary enemySpawnCount;
    public IntObjectDictionary spawned;


    public IntObjectDictionary maxScreenEnemies;

    //private int enemyTotal;

    private int frame = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        spawned = new IntObjectDictionary();

        foreach (var enemyType in enemyTypes.Keys)
        {
            spawned.Add(enemyType, 0);
        }


        for (int i = 0; i < maxScreenEnemies["SimpleEnemy"]; i++)
        {
            StartCoroutine((SpawnEnemy("SimpleEnemy", Random.Range(2f, 4f))));
        }


    }

    IEnumerator SpawnEnemy(string enemyType, float delay)
    {
        if (spawned[enemyType] > maxScreenEnemies[enemyType])
        {
            yield break;
        }
        int side = Random.Range(0, 4);
        float inSide = Random.Range(0.1f, 0.9f);
        Vector3 spawnPos = Vector3.zero;
        Vector2 direction = Vector2.zero;
        switch (side)
        {
            case 0:
                spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(inSide, 1, 0));
                direction = Vector2.down;
                break;
            case 1:
                spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1, inSide, 0));
                direction = Vector2.left;
                break;
            case 2:
                spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(inSide, 0, 0));
                direction = Vector2.up;
                break;
            case 3:
                spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0, inSide, 0));
                direction = Vector2.right;
                break;
        }

        var newEnemy = Instantiate(enemyTypes[enemyType], spawnPos, Quaternion.identity);
        var comp = (SimpleEnemy)newEnemy.GetComponentInChildren(typeof(SimpleEnemy));
        comp.direction = direction;
        //Debug.Log("instantiate:  " + Camera.main.WorldToViewportPoint(spawnPos) + " " + direction);

        enemySpawnCount[enemyType]--;
        spawned[enemyType]++;

        yield return new WaitForSeconds(delay);

    }


    // Update is called once per frame
    void Update()
    {
        foreach (var enemyType in enemyTypes.Keys)
        {
            if (enemySpawnCount[enemyType] > 0)
            {
                //Debug.Log("Spawn a new Enemy");
                StartCoroutine(SpawnEnemy(enemyType, Random.Range(2f,4f)));
            }
        }



        frame++;

        foreach (var enemyType in enemyTypes.Keys)
        {
            if (enemySpawnCount[enemyType] != 0)
            {
                return;
            }

            if (spawned[enemyType] > 0)
            {
                return;
            }
        }




    }


    private void LateUpdate()
    {
        //enemyTotal = 0;
        //foreach (var item in spawned)
        //{
        //    enemyTotal += item.Value;    
        //}


      

    }
}
