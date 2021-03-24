using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniper : IEnemy
{
    // Start is called before the first frame update


   

    void Start()
    {
        base.Start();
        aim = true;
        aimAt = LevelController.instance.Player;
        baseScore = 200;
    }


   

    // Update is called once per frame
    void Update()
    {
        base.Update();

        


        if(ShouldMove)
            transform.position += (direction * speed) * Time.deltaTime;

        aimAt = LevelController.instance.Player;

        NormalShot();

        //projectileTimer += Time.deltaTime;

        //if (projectileTimer > timerThreshold + Random.Range(-timerThreshold, timerThreshold) / 2f)
        //{
        //    projectileTimer = 0f;
        //    //shotAudioSource.PlayOneShot(shotSound);
        //    var newProj = Instantiate(projectile, transform.position + (transform.up * 0.5f), Quaternion.identity);
        //    newProj.GetComponent<ProjectileController>().origin = gameObject;
        //    newProj.transform.up = transform.up;
        //}

        //var pos = Camera.main.WorldToViewportPoint(transform.position);



        //if (health <= 0)
        //{
        //    var newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        //    newExplosion.transform.localScale = newExplosion.transform.localScale * 5;
        //    LevelController.instance.spawned["EnemySniper"]--;
        //    Destroy(gameObject);

        //}
    }
}
