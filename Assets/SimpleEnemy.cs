using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public Vector3 direction;
    private Vector3 directionLocal;
    public GameObject projectile;
    public GameObject explosion;

    private float projectileTimer;
    public float timerThreshold;

    private bool hasCollided;
    private int health = 1;

    void Start()
    {
        directionLocal = transform.InverseTransformDirection(direction);
        //transform.up = directionLocal;
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
        transform.position += (directionLocal * speed) * Time.deltaTime;

        projectileTimer += Time.deltaTime;

        if(projectileTimer > timerThreshold)
        {
            projectileTimer = 0f;
            var newProj = Instantiate(projectile, transform.position + (directionLocal * 0.5f), Quaternion.identity);
            newProj.transform.up = directionLocal;
        }


        var pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x > 1 || pos.x < 0 || pos.y > 1 || pos.y < 0)
            GameObject.Destroy(gameObject);

        if(health <= 0)
        {
            var newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            newExplosion.transform.localScale = newExplosion.transform.localScale * 5;
            Destroy(gameObject);
            //Animação bonitinha
        }
    }
}
