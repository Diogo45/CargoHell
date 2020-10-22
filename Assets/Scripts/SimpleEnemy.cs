using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : IEnemy
{
    // Start is called before the first frame update

    public Vector3 direction;
    private Vector3 directionLocal;


    private float projectileTimer;
    public float timerThreshold;
    private bool hasCollided;


    private bool enteredScene = false;

    void Start()
    {
        //directionLocal = transform.InverseTransformDirection(direction);
        transform.up = -direction;
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
        transform.position += (direction * speed) * Time.deltaTime;

        projectileTimer += Time.deltaTime;

        if (projectileTimer > timerThreshold + Random.Range(-timerThreshold, timerThreshold)/2f)
        {
            projectileTimer = 0f;
            var newProj = Instantiate(projectile, transform.position + (direction * 0.5f), Quaternion.identity);
            newProj.transform.up = direction;
        }

        var pos = Camera.main.WorldToViewportPoint(transform.position);


        if (enteredScene)
        {
            if (pos.x > 1 || pos.x < 0 || pos.y > 1 || pos.y < 0)
            {
                LevelController.instance.enemySpawnCount["SimpleEnemy"]++;
                Destroy(transform.parent.gameObject);
                Destroy(gameObject);
            }
              

        }

        if (pos.x < 1 && pos.x > 0 && pos.y < 1 && pos.y > 0)
        {
            enteredScene = true;
        }


        if (health <= 0)
        {
            var newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            newExplosion.transform.localScale = newExplosion.transform.localScale * 5;
            //LevelController.instance.enemySpawnCount["SimpleEnemy"]--;
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);

        }
    }
}
