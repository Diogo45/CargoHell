using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoHell.Animation;
using CargoHell;
using CargoHell.Audio;

public class ShieldController : MonoBehaviour
{


    public GameObject player;
    private PlayerController ship;

    private SpriteRenderer shieldRender;

    [Range(0, 1), SerializeField] private float AngleDamping;

    private Vector2 debugNormal;
    private Vector2 debugPoint;

    public AudioClip shieldReflectSound;
    private AudioSource shieldReflectAudioSource;

    [SerializeField]
    private float shieldReactivationDelay;


    void Start()
    {
        shieldReflectAudioSource = gameObject.AddComponent<AudioSource>();
        shieldRender = gameObject.GetComponent<SpriteRenderer>();
        //shieldReflectAudioSource.outputAudioMixerGroup = LevelController.instance.SFXMixer;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        shieldReflectAudioSource.outputAudioMixerGroup = AudioController.instance.SFXMixer;

        if (collision.gameObject.name.Contains("Chaser"))
        {
            collision.gameObject.GetComponent<IEnemy>().health--;
            StartCoroutine(ShieldFlickerDown());
            StartCoroutine(ShieldFlickerUp(shieldReactivationDelay * 2));
            StartCoroutine(Utils.ActivateBehaviour(shieldReactivationDelay*2, gameObject.GetComponent<CapsuleCollider2D>()));
            StartCoroutine(Utils.ActivateRenderer(shieldReactivationDelay*2, gameObject.GetComponent<SpriteRenderer>()));

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            return;
        }


        var hit = collision.ClosestPoint(collision.gameObject.transform.position);

        if ((collision.gameObject.tag == "Projectile" || collision.gameObject.tag == "ProjectileReflected" || collision.gameObject.tag == "ProjectileSpinner") && player.tag == "Spinner")
        {

            var normalVector = hit - (Vector2)(LevelController.instance.Player.transform.position - player.transform.up / 3.5f);
            debugNormal = normalVector;
            debugPoint = hit;
            collision.transform.up = Vector2.Reflect(collision.transform.up, normalVector.normalized);
            shieldReflectAudioSource.PlayOneShot(shieldReflectSound);
            collision.tag = "ProjectileSpinner";

        }
        else if (collision.gameObject.tag == "Projectile" || collision.gameObject.tag == "ProjectileSpinner")
        {

            if (projController.projectileType == ProjectileController.ProjectileType.HOMING)
            {

                AnimationController.instance.Explosion(collision.transform.position);
                Destroy(collision.gameObject);

                StartCoroutine(ShieldFlickerDown());
                StartCoroutine(ShieldFlickerUp(shieldReactivationDelay));
                StartCoroutine(Utils.ActivateBehaviour(shieldReactivationDelay, gameObject.GetComponent<CapsuleCollider2D>()));
                StartCoroutine(Utils.ActivateRenderer(shieldReactivationDelay, gameObject.GetComponent<SpriteRenderer>()));

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<Collider2D>().enabled = false;
                return;

            }

            var normalVector = hit - (Vector2)(LevelController.instance.Player.transform.position - player.transform.up / 3.5f);
            StartCoroutine(ReflectShot(collision));
            collision.tag = "ProjectileReflected";
            projController.HPTP = true;
            projController.angleReflected = Vector2.Angle(player.transform.up, collision.transform.up);


            projController.mult += LevelController.instance.MultIncrease;
        }

        


    }

    IEnumerator ReflectShot(Collider2D collision)
    {
        var comp = collision.GetComponent<ProjectileController>();
        var speed = comp.projectileSpeed;
        comp.projectileSpeed = 0f;
        var hit = collision.ClosestPoint(collision.gameObject.transform.position);
        var playerPos = player.transform.position;
        var oldPlayerUp = player.transform.up;
      
        if (collision)
        {

            var normalVector = hit - (Vector2)(playerPos - oldPlayerUp / 3.5f);
            //Debug.Log("After: " + normalVector.x + " " + normalVector.y);
            debugNormal = normalVector;
            debugPoint = hit;
            collision.transform.up = Vector2.Reflect(collision.transform.up, normalVector.normalized);
            shieldReflectAudioSource.PlayOneShot(shieldReflectSound);

        }

        yield return new WaitForSeconds(0.2f);


        if (collision)
        {
            float deltaAngle = Vector2.SignedAngle(oldPlayerUp, player.transform.up) * AngleDamping;
            collision.transform.Rotate(0, 0, Mathf.Clamp(deltaAngle, -90, 90));
            comp.projectileSpeed = speed;
        }

        yield break;
    }

    IEnumerator ShieldFlickerDown()
    {
        for (int i = 0; i < 5; i++)
        {
            shieldRender.enabled = false;
            yield return new WaitForSeconds(0.1f);
            shieldRender.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        shieldRender.enabled = false;
    }

    IEnumerator ShieldFlickerUp(float startTime)
    {
        yield return new WaitForSeconds(startTime - 1.0f);
        for (int i = 0; i < 5; i++)
        {
            shieldRender.enabled = true;
            yield return new WaitForSeconds(0.1f);
            shieldRender.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
        shieldRender.enabled = true;
    }

    IEnumerator ReflectShotDiscretized(Collider2D collision)
    {
        var hit = transform.InverseTransformPoint(collision.ClosestPoint(collision.gameObject.transform.position + (collision.transform.up / 2f)));

        float angle = 0f;

        switch (hit.x)
        {
            

            case var expression when hit.x < -1.0:
                angle = 45f;
                break;
            case var expression when hit.x > 1.0:
                angle = -45f;
                break;

            case var expression when hit.x < -.8 && hit.x >= -1.0:
                angle = 35f;
                break;
            case var expression when hit.x > .8 && hit.x <= 1.0:
                angle = -35f;
                break;

            case var expression when hit.x < -.6 && hit.x >= -.8:
                angle = 25f;
                break;
            case var expression when hit.x > .6 && hit.x <= .8:
                angle = -25f;
                break;


            case var expression when hit.x < -.4 && hit.x >= -.6:
                angle = 15f;
                break;
            case var expression when hit.x > .4 && hit.x <= .6:
                angle = -15f;
                break;

            default:
                angle = 0f;
                break;
        }


        collision.transform.up = -collision.transform.up;
        collision.transform.Rotate(0, 0, angle);


        yield return null;
    }


}
