using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniper : IEnemy
{
    // Start is called before the first frame update

    private float projectileTimer;
    public float timerThreshold;
    private bool hasCollided;



    private GameObject aimAt;

    void Start()
    {
        //directionLocal = transform.InverseTransformDirection(direction);
        //transform.up = -direction;
        aimAt = LevelController.instance.Player;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ProjectileReflected")
        {
            if (!hasCollided)
            {
                health--;
                Destroy(collision.gameObject);
                hasCollided = true;
            }

        }
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



        transform.position += (direction * speed) * Time.deltaTime;

        projectileTimer += Time.deltaTime;

        if (projectileTimer > timerThreshold + Random.Range(-timerThreshold, timerThreshold) / 2f)
        {
            projectileTimer = 0f;
            var newProj = Instantiate(projectile, transform.position + (transform.up * 0.5f), Quaternion.identity);
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
