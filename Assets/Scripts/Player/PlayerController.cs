using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CargoHell.Audio;

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


    [SerializeField] private Vector2 _moveAxis;

    //private float moveForward = 0f;
    //private float moveBackward = 0f;

    private Material material;
    public int MaxPossibleHealth { get => maxPossibleHealth; set => maxPossibleHealth = value; }

    public float velocity { get; private set; }

    public float AngularSpeed;

    public bool PlayerInvulnerable = false;

    public bool Movement = true;

    private Vector2 _lookVector;


    void Start()
    {

        int InputType = PlayerPrefs.GetInt("InputType", 0);

        if (InputType == 1)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        material = gameObject.GetComponent<SpriteRenderer>().material;

        InputManager.instance.move.performed += Move_performed;
        InputManager.instance.move.canceled += Move_canceled;

        InputManager.instance.look.performed += Look_performed;

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


    private void Look_performed(InputAction.CallbackContext obj)
    {
        var delta = obj.ReadValue<Vector2>();

        delta.x /= Screen.currentResolution.width;
        delta.y /= Screen.currentResolution.height;

        //_lookVector = new Vector3(delta.x, delta.y, 0f).normalized;
        _lookVector = delta;
    }


    private void Move_canceled(InputAction.CallbackContext obj)
    {
        _moveAxis = Vector2.zero;
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        _moveAxis = obj.ReadValue<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "PowerUpHealth")
        {
            if (currentHealth < maxCurrentHealth)
            {
                currentHealth++;
                AudioController.instance.PlayPowerupSound();
                Destroy(collision.gameObject);
            }

        }

        if (collision.gameObject.tag == "PowerUpInv")
        {
            StartCoroutine(DamageAnimationPlayer(10f));
            AudioController.instance.PlayPowerupSound();
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

        if (collision.tag == "Spinner")
        {
            if (PlayerInvulnerable)
            {
                return;
            }
            collision.gameObject.GetComponent<IObject>().health--;
            StartCoroutine(Invunerable(1f));
            currentHealth--;
        }

        if (collision.tag == "Enemy" || collision.tag == "Projectile" || collision.gameObject.tag == "ProjectileSpinner" || collision.tag == "Spinner" || collision.tag == "EnemyChaser")
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



    public void OnMove(InputValue value)
    {

        _moveAxis = value.Get<Vector2>();

    }

    // Update is called once per frame
    void Update()
    {


        #region Input

#if UNITY_STANDALONE


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


        int InputType = PlayerPrefs.GetInt("InputType", 0);

        Vector2 mouseValue = Vector2.zero;
        Vector3 direction = Vector3.zero;
        Vector3 mousePosition = Vector3.zero;

        switch (InputType)
        {
            case 0:
                mouseValue = Mouse.current.position.ReadValue();

                mousePosition = new Vector3(mouseValue.x, mouseValue.y, 0f);

                direction = (Camera.main.ScreenToWorldPoint(mousePosition) - (transform.position)).normalized;
                break;

            case 1:
                mouseValue = _lookVector;

                mousePosition = new Vector3(mouseValue.x, mouseValue.y, 0f);

                direction = mousePosition;
                break;
        }





#endif
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= 90f;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        if (!Movement)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, (AngularSpeed  * Time.deltaTime) * mousePosition.magnitude);

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
        var lastPos = transform.position;
        transform.position += transform.up * _moveAxis.y * moveSpeed * Time.deltaTime;
        velocity = (transform.position - lastPos).magnitude * (1f / Time.deltaTime);



#endif

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
