using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerController : IObject
{
    // Start is called before the first frame update

    public float rotationSpeed;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
