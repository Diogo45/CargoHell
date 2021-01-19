using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update

    public float projectileSpeed = 0.01f;

    public float mult = 0f;

    public bool HPTP = false;

    private float distanceTraveled = 0f;
    private float angleReflected = 180f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * projectileSpeed * Time.deltaTime;


        Debug.DrawRay(transform.position, transform.up);

        var pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x > 1 || pos.x < 0 || pos.y > 1 || pos.y < 0)
            GameObject.Destroy(gameObject);

    }
}
