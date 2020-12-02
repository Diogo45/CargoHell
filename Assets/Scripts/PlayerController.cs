using System.Collections;
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

    private Material material;
    public int MaxPossibleHealth { get => maxPossibleHealth; set => maxPossibleHealth = value; }


    public float AngularSpeed;



    void Start()
    {
        material = gameObject.GetComponent<SpriteRenderer>().material;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PowerUpHealth")
        {
            if(currentHealth < maxCurrentHealth)
            {
                currentHealth++;
                Destroy(collision.gameObject);
            }
            
        }


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
            StartCoroutine(DamageColor());
        }
    }


    IEnumerator DamageColor()
    {
        for (int i = 0; i < 5; i++)
        {
            SetColor(Color.white);
            yield return new WaitForSeconds(0.1f);
            SetColor(new Color(1, 1, 1, 0));
            yield return new WaitForSeconds(0.1f);
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


        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= 90f;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, AngularSpeed * Time.deltaTime);

        if (moveForward)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;

        }
        if (moveBackward)
        {
            transform.position -= transform.up * moveSpeed * Time.deltaTime;

        }







        //if (rotateClockWise)
        //    transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        //if (rotateCounterClockWise)
        //    transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);

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




    private void SetColor(Color color)
    {
        material.SetColor("_Color", color);
    }



}
