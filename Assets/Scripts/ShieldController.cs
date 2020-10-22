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
            collision.transform.up = -Vector2.Reflect(collision.transform.up, normalVector);
            collision.tag = "ProjectileReflected";

            Vector2 hitCenter = (Vector2)transform.position - hit.point;
            Vector2.Angle(hitCenter, transform.up);
            Debug.Log(Vector2.Angle(hitCenter, transform.up));
            //if (Vector2.Angle(hitCenter, transform.up) == 179.9115)
            //{
            //    collision.transform.up = Vector2.up;
            //}


            //if()

        }
    }


    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up);
        Debug.DrawRay(transform.position, debugNormal, Color.black);

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.left, 100);
        Vector2 hitCenter = (Vector2)transform.position - hit.point;
        Debug.DrawLine(hit.point, hit.point + hitCenter);
        //Debug.Log(Vector2.Angle(hitCenter, transform.up));
        //for (int i = 0; i < 100; i++)
        //{
        //    
        //    Debug.DrawRay(hit.point, hit.normal);
        //}
        //for (int i = 0; i < 100; i++)
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector3.up * 0.01f * i), Vector3.left, 100);
        //    Debug.DrawRay(hit.point, hit.normal);
        //}


    }
}
