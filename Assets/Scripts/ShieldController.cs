using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{


    public GameObject player;
    private PlayerController ship;

    private SpriteRenderer shieldRender;

    private Vector2 debugNormal;
    private Vector2 debugPoint;

    public AudioClip shieldReflectSound;
    private AudioSource shieldReflectAudioSource;

    [SerializeField]
    private float shieldReactivationDelay;

    //controls if HPTP, the boolean variable

    // Start is called before the first frame update
    void Start()
    {
        shieldReflectAudioSource = gameObject.AddComponent<AudioSource>();
        shieldRender = gameObject.GetComponent<SpriteRenderer>();
        //shieldReflectAudioSource.outputAudioMixerGroup = LevelController.instance.SFXMixer;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        shieldReflectAudioSource.outputAudioMixerGroup = LevelController.instance.SFXMixer;

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


        if (collision.gameObject.tag == "Projectile" || collision.gameObject.tag == "ProjectileReflected" || collision.gameObject.tag == "ProjectileSpinner")
        {

            var projController = collision.GetComponent<ProjectileController>();

            if (projController.projectileType == ProjectileController.ProjectileType.HOMING)
            {

                LevelController.instance.Explosion(collision.transform.position);
                Destroy(collision.gameObject);

                StartCoroutine(ShieldFlickerDown());
                StartCoroutine(ShieldFlickerUp(shieldReactivationDelay));
                StartCoroutine(Utils.ActivateBehaviour(shieldReactivationDelay, gameObject.GetComponent<CapsuleCollider2D>()));
                StartCoroutine(Utils.ActivateRenderer(shieldReactivationDelay, gameObject.GetComponent<SpriteRenderer>()));

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<Collider2D>().enabled = false;
                return;

            }


            var hit = collision.ClosestPoint(collision.gameObject.transform.position);


            //var normalVector = hit.normal;
            //var normalVector = hit.normal;

            if (player.tag == "Spinner")
            {

                var normalVector = hit - (Vector2)(LevelController.instance.Player.transform.position - player.transform.up / 3.5f);
                debugNormal = normalVector;
                //debugPoint = hit.point;
                debugPoint = hit;
                collision.transform.up = Vector2.Reflect(collision.transform.up, normalVector.normalized);

                shieldReflectAudioSource.PlayOneShot(shieldReflectSound);
                collision.tag = "ProjectileSpinner";
            }
            else
            {
                StartCoroutine(ReflectShot(collision));
                collision.tag = "ProjectileReflected";
                projController.HPTP = true;
                projController.angleReflected = Vector2.Angle(player.transform.up, collision.transform.up);
                //Debug.Log(collision.GetComponent<ProjectileController>().angleReflected);
            }

            projController.mult += LevelController.instance.MultIncrease;


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

    IEnumerator ReflectShot(Collider2D collision)
    {
        var comp = collision.GetComponent<ProjectileController>();
        var speed = comp.projectileSpeed;
        collision.GetComponent<ProjectileController>().projectileSpeed = 0f;
        yield return new WaitForSeconds(0.1f);
        if (collision)
        {
            var hit = collision.ClosestPoint(collision.gameObject.transform.position);
            var normalVector = hit - (Vector2)(LevelController.instance.Player.transform.position - player.transform.up / 3.5f);
            debugNormal = normalVector;
            //debugPoint = hit.point;
            debugPoint = hit;
            collision.transform.up = Vector2.Reflect(collision.transform.up, normalVector.normalized);
            comp.projectileSpeed = speed;
            shieldReflectAudioSource.PlayOneShot(shieldReflectSound);

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

}
