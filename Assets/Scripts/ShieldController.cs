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
            // var closestPoint = collision.ClosestPoint(collision.gameObject.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, collision.transform.up, 100);
            //if (hit.collider != null)
            //{
            //    Debug.DrawRay(hit.point, hit.normal, Color.yellow, 10f);
            //    float angle = Vector2.Angle(transform.right, hit.normal) - 90;
            //    Debug.Log("angle = " + angle); // 0 on ground, 18.2 on inclined plane
            //}
            //var normalVector = closestPoint - (Vector2)transform.position;
            var normalVector = hit.normal;
            debugNormal = normalVector;
            //collision.transform.up = Vector2.Reflect(collision.transform.up, (normalVector + closestPoint).normalized);
            collision.transform.up = Vector2.Reflect((Vector2)collision.transform.up, normalVector);


            #region Old

            //var deltaAngle =  Vector3.Angle(transform.up, Vector3.zero);

            //var cross = Vector3.Cross(transform.up, collision.transform.up);

            //Debug.Log("1 "+ deltaAngle);

            //if(cross.z > 0 && (deltaAngle > 90 && deltaAngle < 180))
            //{
            //    deltaAngle = -(deltaAngle - 360);

            //}
            //else if (cross.z < 0 && (deltaAngle > 90 && deltaAngle < 180))
            //{
            //    deltaAngle = -(360 - deltaAngle);
            //}
            //else if (cross.z > 0 && (deltaAngle > 0 && deltaAngle < 90))
            //{
            //    deltaAngle = 180 - deltaAngle;

            //}
            //else if (cross.z < 0 && (deltaAngle > 0 && deltaAngle < 90))
            //{
            //    deltaAngle = -(deltaAngle - 180);
            //}

            //Debug.Log("2 " + deltaAngle);
            //if(deltaAngle < 90 || deltaAngle > 270)
            //{
            //    deltaAngle = 180 - deltaAngle;
            //}

            //collision.transform.Rotate(new Vector3(0, 0, deltaAngle));

            #endregion
        }
    }


    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up);
        Debug.DrawRay(transform.position, debugNormal, Color.black);

    }
}
