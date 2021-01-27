﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : IEnemy
{
    // Start is called before the first frame update

    private Vector3 directionLocal;


    private float projectileTimer;
    public float timerThreshold;


    

    void Start()
    {
        base.Start();
        //directionLocal = transform.InverseTransformDirection(direction);
        transform.up = -direction;
        baseScore = 100;
    }



    // Update is called once per frame
    void Update()
    {

        base.Update();
        transform.up = -direction;

        transform.position += (direction * speed) * Time.deltaTime;

        projectileTimer += Time.deltaTime;

        if (projectileTimer > timerThreshold + Random.Range(-timerThreshold, timerThreshold)/2f)
        {
            projectileTimer = 0f;
            //shotAudioSource.PlayOneShot(shotSound);
            var newProj = Instantiate(projectile, transform.position + (direction * 0.5f), Quaternion.identity);
            newProj.GetComponent<ProjectileController>().origin = gameObject;
            newProj.transform.up = direction;
        }



        //if (enteredScene)
        //{
        //    if (pos.x > 1 || pos.x < 0 || pos.y > 1 || pos.y < 0)
        //    {
        //        LevelController.instance.enemySpawnCount["SimpleEnemy"]++;
        //        LevelController.instance.spawned["SimpleEnemy"]--;
        //        Destroy(transform.parent.gameObject);
        //        Destroy(gameObject);
        //    }
              

        //}

        //if (pos.x < 1 && pos.x > 0 && pos.y < 1 && pos.y > 0)
        //{
        //    enteredScene = true;
        //}


        //if (health <= 0)
        //{
        //    var newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        //    newExplosion.transform.localScale = newExplosion.transform.localScale * 5;
        //    //LevelController.instance.enemySpawnCount["SimpleEnemy"]--;
        //    LevelController.instance.spawned["SimpleEnemy"]--;
        //    //Destroy(transform.parent.gameObject);
        //    Destroy(gameObject);

        //}
    }
}
