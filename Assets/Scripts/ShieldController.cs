using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{


    public GameObject player;
    private PlayerController ship;



    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            


          

            var deltaAngle =  Vector3.Angle(transform.up, collision.transform.up);

            var cross = Vector3.Cross(transform.up, collision.transform.up);

            Debug.Log("1 "+ deltaAngle);

            if(cross.z > 0 && (deltaAngle > 90 && deltaAngle < 180))
            {
                deltaAngle = -(deltaAngle - 360);

            }
            else if (cross.z < 0 && (deltaAngle > 90 && deltaAngle < 180))
            {
                deltaAngle = -(360 - deltaAngle);
            }
            else if (cross.z > 0 && (deltaAngle > 0 && deltaAngle < 90))
            {
                deltaAngle = 180 - deltaAngle;

            }
            else if (cross.z < 0 && (deltaAngle > 0 && deltaAngle < 90))
            {
                deltaAngle = deltaAngle - 180;
            }

            Debug.Log("2 " + deltaAngle);
            //if(deltaAngle < 90 || deltaAngle > 270)
            //{
            //    deltaAngle = 180 - deltaAngle;
            //}

            collision.transform.Rotate(new Vector3(0, 0, deltaAngle));
        }
    }


    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up);

    }
}
