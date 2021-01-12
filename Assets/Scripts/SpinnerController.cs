using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerController : IObject
{
    // Start is called before the first frame update

    public float rotationSpeed;
    private float repeat = 1;
    private bool hasCollided;
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ProjectileReflected" || collision.gameObject.tag == "Projectile")
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

        transform.position += (direction * speed) * Time.deltaTime;

        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        if (health <= 0)
        {
            var newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            newExplosion.transform.localScale = newExplosion.transform.localScale * 5;
            Destroy(gameObject);

        }
    }
}
