using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniper : IEnemy
{
    // Start is called before the first frame update

    private float projectileTimer;
    public float timerThreshold;

    private GameObject aimAt;

    void Start()
    {
        base.Start();

        //directionLocal = transform.InverseTransformDirection(direction);
        //transform.up = -direction;
        aimAt = LevelController.instance.Player;
        baseScore = 200;
    }


   

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (aimAt)
        {
            Vector2 dir = aimAt.transform.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            angle -= 90f;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);

        }


        if(ShouldMove)
            transform.position += (direction * speed) * Time.deltaTime;

        projectileTimer += Time.deltaTime;

        if (projectileTimer > timerThreshold + Random.Range(-timerThreshold, timerThreshold) / 2f)
        {
            projectileTimer = 0f;
            //shotAudioSource.PlayOneShot(shotSound);
            var newProj = Instantiate(projectile, transform.position + (transform.up * 0.5f), Quaternion.identity);
            newProj.GetComponent<ProjectileController>().origin = gameObject;
            newProj.transform.up = transform.up;
        }

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
