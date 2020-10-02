using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{


    public GameObject player;
    private PlayerController ship;


    private Vector2 debugNormal;


    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, collision.transform.up, 100);

            var normalVector = hit.normal;
            debugNormal = normalVector;
            collision.transform.up = Vector2.Reflect((Vector2)collision.transform.up, normalVector);
            collision.tag = "ProjectileReflected";

        }
    }


    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up);
        Debug.DrawRay(transform.position, debugNormal, Color.black);

    }
}
