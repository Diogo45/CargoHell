using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    
    public int damage = 0;


    [Range(0, 360)]
    public int rotationSpeed;

    public float moveSpeed;

    private bool moveForward = false;
    private bool moveBackward = false;

    private bool rotateClockWise = false;
    private bool rotateCounterClockWise = false;

    void Start()
    {

    }


    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            damage++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region Input

        if (Input.GetKeyDown(KeyCode.A))
        {
            rotateClockWise = true;

        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            rotateClockWise = false;
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            rotateCounterClockWise = true;

        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            rotateCounterClockWise = false;
        }



        if (Input.GetKeyDown(KeyCode.W))
        {
            moveForward = true;

        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            moveForward = false;
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            moveBackward = true;

        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            moveBackward = false;
        }

        #endregion

        #region Apply Input

        if (moveForward)
            transform.position += transform.up * moveSpeed;
        if (moveBackward)
            transform.position -= transform.up * moveSpeed;
        if (rotateClockWise)
            transform.Rotate(Vector3.forward, rotationSpeed);
        if (rotateCounterClockWise)
            transform.Rotate(Vector3.forward, -rotationSpeed);

        #endregion

        #region Limit to screen space

        var pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        #endregion



    }
}
