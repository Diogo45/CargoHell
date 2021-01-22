using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{


    public GameObject player;
    private PlayerController ship;


    private Vector2 debugNormal;
    private Vector2 debugPoint;

    //controls if HPTP, the boolean variable

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<EdgeCollider2D>().
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {



        if (collision.gameObject.tag == "Projectile"  || collision.gameObject.tag == "ProjectileReflected" || collision.gameObject.tag == "ProjectileSpinner")
        {
            //RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, collision.transform.up, 100);


            var hit = collision.ClosestPoint(collision.gameObject.transform.position);



            //var normalVector = hit.normal;
            //var normalVector = hit.normal;

            var normalVector = hit - (Vector2)(LevelController.instance.Player.transform.position + player.transform.up / 3.5f); 
            debugNormal = normalVector;
            //debugPoint = hit.point;
            debugPoint = hit;
            collision.transform.up = Vector2.Reflect(collision.transform.up, normalVector.normalized);

            if(player.tag == "Spinner")
            {
                collision.tag = "ProjectileSpinner";
            }
            else
            {
                collision.tag = "ProjectileReflected";
                collision.GetComponent<ProjectileController>().HPTP = true;
                collision.GetComponent<ProjectileController>().angleReflected = Vector2.Angle(player.transform.up, collision.transform.up);
                //Debug.Log(collision.GetComponent<ProjectileController>().angleReflected);
            }

            collision.GetComponent<ProjectileController>().mult += LevelController.instance.MultIncrease;


            //Vector2 hitCenter = (Vector2)transform.position - hit.oint;
            //Vector2.Angle(hitCenter, transform.up);
            ////Debug.Log(Vector2.Angle(hitCenter, transform.up));
            //Debug.Log("Normal:" + normalVector.normalized);
            ////if (Vector2.Angle(hitCenter, transform.up) == 179.9115)
            ////{
            ////    collision.transform.up = Vector2.up;
            ////}


            //if()

        }
    }


    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up);
        Debug.DrawRay(player.transform.position + player.transform.up/3.5f, debugNormal, Color.red);
        Debug.DrawLine(player.transform.position, (Vector2)player.transform.position + debugPoint, Color.blue);

        //Vector2 hitCenter = (Vector2)transform.position - hit.point;
        //Debug.DrawLine(hit.point, hit.point + hitCenter);
        //Debug.Log(Vector2.Angle(hitCenter, transform.up));
        //for (int i = 0; i < 100; i++)
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition + (Vector3.up * 0.01f * i)), Vector3.left, 100);

        //    Debug.DrawRay(hit.point, hit.normal);
        //}
        //for (int i = 0; i < 100; i++)
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector3.up * 0.01f * i), Vector3.left, 100);
        //    Debug.DrawRay(hit.point, hit.normal);
        //}


    }
}
