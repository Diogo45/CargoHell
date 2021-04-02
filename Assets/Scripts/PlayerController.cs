using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MOVEMENT
{
    TWO_JOYSTICK, ONE_JOYSTICK, PUSH
}

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
#if UNITY_ANDROID
    public FixedJoystick moveJoystick;
    public FixedJoystick lookJoystick;

    private MOVEMENT movType = MOVEMENT.TWO_JOYSTICK;

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

    private Material material;
    public int MaxPossibleHealth { get => maxPossibleHealth; set => maxPossibleHealth = value; }


    public float AngularSpeed;

    public bool PlayerInvulnerable = false;

    public bool Movement = true;

    void Start()
    {

        material = gameObject.GetComponent<SpriteRenderer>().material;

#if UNITY_ANDROID

        movType = (MOVEMENT)PlayerPrefs.GetInt("InputType", 0);

        if (movType == MOVEMENT.TWO_JOYSTICK)
        {
            lookJoystick.gameObject.SetActive(true);
            moveJoystick.gameObject.SetActive(true);
        }
        else if(movType == MOVEMENT.ONE_JOYSTICK)
        {
            lookJoystick.gameObject.SetActive(false);
            moveJoystick.Reset = false;
        }
        else
        {
            lookJoystick.gameObject.SetActive(false);
            moveJoystick.gameObject.SetActive(false);
        }
#endif        
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
            StartCoroutine(DamageAnimationPlayer(10f));
            
            Destroy(collision.gameObject);
        }

       

        if (collision.gameObject.tag == "Projectile" || collision.gameObject.tag == "ProjectileSpinner")
        {

            if (PlayerInvulnerable)
            {
                Destroy(collision.gameObject);
                return;
                
            }

            if (!hasCollided)
            {
                currentHealth--;
                StartCoroutine(Invunerable(1f));
                Destroy(collision.gameObject);
                hasCollided = true;
            }
        }

        if (collision.tag == "Enemy")
        {
            if (PlayerInvulnerable)
            {
                return;

            }
            collision.gameObject.GetComponent<IEnemy>().health--;
            StartCoroutine(Invunerable(1f));
            currentHealth--;
        }

        if (collision.tag == "Object")
        {
            if (PlayerInvulnerable)
            {
                //Destroy(collision.gameObject);
                return;
            }
            collision.gameObject.GetComponent<IObject>().health--;
            StartCoroutine(Invunerable(1f));
            currentHealth--;
        }

        if (collision.tag == "Enemy" || collision.tag == "Projectile" || collision.gameObject.tag == "ProjectileSpinner")
        {
            StartCoroutine(StrobeColor(Color.white));
        }
    }

    IEnumerator Invunerable(float time)
    {
        PlayerInvulnerable = true;
        yield return new WaitForSeconds(time);
        PlayerInvulnerable = false;
        yield break;
    }

    private IEnumerator DamageAnimationPlayer(float duration)
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

        if (!Movement)
            return;

#region Input

#if UNITY_STANDALONE

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

        //if(movType == MOVEMENT.TWO_JOYSTICK)
        //{

        //}
        //else if (movType == MOVEMENT.ONE_JOYSTICK)
        //{

        //}
        //else
        //{

        //}
        Vector2 direction = Vector2.zero;

        if (movType == MOVEMENT.TWO_JOYSTICK)
        {
            direction = Vector3.up * lookJoystick.Vertical + Vector3.right * lookJoystick.Horizontal;
        }
        else if (movType == MOVEMENT.ONE_JOYSTICK)
        {
            direction = Vector3.up * moveJoystick.Vertical + Vector3.right * moveJoystick.Horizontal;
        }
        else
        {

        }
        


#endif

#if UNITY_STANDALONE

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        

#endif
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= 90f;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, AngularSpeed * Time.deltaTime);

#if UNITY_ANDROID

        if (movType == MOVEMENT.TWO_JOYSTICK)
        {
            transform.position += (Vector3.up * moveJoystick.Vertical + Vector3.right * moveJoystick.Horizontal) * moveSpeed * Time.deltaTime;
        }
        else if(movType == MOVEMENT.ONE_JOYSTICK)
        {
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
        else
        {

        }
        


#else


        if (moveForward > 0f)
        {
            //var lPos = transform.position;

            

            transform.position += transform.up * moveSpeed * moveForward * Time.deltaTime;

            //var mov = transform.position - lPos;

            //LevelController.instance.nebulaMat.SetVector("_ScrollSpeed", new Vector4(mov.x * 10f, mov.y * 10f, 1, 1));

        }
        if (moveBackward > 0f)
        {
            transform.position -= transform.up * moveSpeed * moveBackward * Time.deltaTime;

        }


#endif




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
