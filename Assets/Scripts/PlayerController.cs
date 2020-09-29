﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Projectile;
    
    private int maxPossibleHealth = 7;

    public int maxCurrentHealth = 3;

    public int currentHealth = 3;
    private bool hasCollided = false;


    [Range(0, 360)]
    public int rotationSpeed;

    public float moveSpeed;

    private bool moveForward = false;
    private bool moveBackward = false;

    private bool rotateClockWise = false;
    private bool rotateCounterClockWise = false;


    public int MaxPossibleHealth { get => maxPossibleHealth; set => maxPossibleHealth = value; }

    void Start()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            if (!hasCollided)
            {
                currentHealth--;
                Destroy(collision.gameObject);
                hasCollided = true;
            }
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
            transform.position += transform.up * moveSpeed * Time.deltaTime;
        if (moveBackward)
            transform.position -= transform.up * moveSpeed * Time.deltaTime;
        if (rotateClockWise)
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        if (rotateCounterClockWise)
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var newProj = Instantiate(Projectile, transform.position + (transform.up * 1.5f), Quaternion.identity);
            newProj.transform.up = transform.up;
            newProj.tag = "ProjectileReflected";
        }

        #endregion

        #region Limit to screen space

        var pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        #endregion



        hasCollided = false;

    }
}
