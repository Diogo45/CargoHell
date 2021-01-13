﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
#if UNITY_ANDROID
    public FixedJoystick moveJoystick;
    public FixedJoystick lookJoystick;
#endif
    public GameObject Projectile;

    private int maxPossibleHealth = 7;

    public int maxCurrentHealth = 3;

    public int currentHealth = 3;
    private bool hasCollided = false;


    [Range(0, 360)]
    public int rotationSpeed;

    public float moveSpeed;

    private float moveForward = 0f;
    private float moveBackward = 0f;

    private bool rotateClockWise = false;
    private bool rotateCounterClockWise = false;

    private Material material;
    public int MaxPossibleHealth { get => maxPossibleHealth; set => maxPossibleHealth = value; }


    public float AngularSpeed;

    public bool PlayerInvulnerable = false;

    void Start()
    {
        material = gameObject.GetComponent<SpriteRenderer>().material;
        
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "PowerUpHealth")
        {
            if (currentHealth < maxCurrentHealth)
            {
                currentHealth++;
                Destroy(collision.gameObject);
            }

        }

        if (collision.gameObject.tag == "PowerUpInv")
        {
            StartCoroutine(Invulnerability(5f));
            Destroy(collision.gameObject);
        }

        if (PlayerInvulnerable)
            return;

        if (collision.gameObject.tag == "Projectile")
        {
            if (!hasCollided)
            {
                currentHealth--;
                Destroy(collision.gameObject);
                hasCollided = true;
            }
        }

        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<IEnemy>().health = 0;
            currentHealth--;
        }

        if (collision.tag == "Object")
        {
            collision.gameObject.GetComponent<IObject>().health = 0;
            currentHealth--;
        }

        if (collision.tag == "Enemy" || collision.tag == "Projectile")
        {
            StartCoroutine(StrobeColor(Color.white));
        }
    }


    IEnumerator Invulnerability(float duration)
    {
        PlayerInvulnerable = true;
        Color c = Color.cyan;
        c.a = 0.4f;
        material.color = c;

        yield return new WaitForSeconds(duration * 0.8f);
        StartCoroutine(StrobeColor(Color.cyan));
        yield return new WaitForSeconds(duration * 0.2f);

        PlayerInvulnerable = false;
        c = Color.white;
        c.a = 0;
        material.color = c;

    }

    IEnumerator StrobeColor(Color c)
    {
        for (int i = 0; i < 5; i++)
        {
            c.a = 1f;
            SetColor(c);
            yield return new WaitForSeconds(0.1f);
            c.a = 0f;
            SetColor(c);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {


        #region Input

#if UNITY_STANDALONE

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
            moveForward = 1f;

        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            moveForward = 0f;
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            moveBackward = 1f;

        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            moveBackward = 0f;
        }

#endif

#if UNITY_ANDROID

        if(moveJoystick.Vertical > 0f)
        {
            moveBackward = 0f;
            moveForward = moveJoystick.Vertical;
        }
        else
        {
            moveForward = 0f;
            moveBackward = -moveJoystick.Vertical;
        }

#endif

#endregion

        #region Apply Input



#if UNITY_ANDROID

        Vector2 direction = Vector3.up * lookJoystick.Vertical + Vector3.right * lookJoystick.Horizontal;


#endif

#if UNITY_STANDALONE

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

#endif
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= 90f;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, AngularSpeed * Time.deltaTime);

        if (moveForward > 0f)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;

        }
        if (moveBackward > 0f)
        {
            transform.position -= transform.up * moveSpeed * Time.deltaTime;

        }







        //if (rotateClockWise)
        //    transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        //if (rotateCounterClockWise)
        //    transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    var newProj = Instantiate(Projectile, transform.position + (transform.up * 1.5f), Quaternion.identity);
        //    newProj.transform.up = transform.up;
        //    newProj.tag = "ProjectileReflected";
        //}

#endregion

#region Limit to screen space

        var pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

#endregion
        hasCollided = false;





    }




    private void SetColor(Color color)
    {
        material.SetColor("_Color", color);
    }



}
