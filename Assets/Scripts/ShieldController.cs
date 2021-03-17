using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{


    public GameObject player;
    private PlayerController ship;


    [Range(0, 1), SerializeField] private float AngleDamping;

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
        //gameObject.GetComponent<EdgeCollider2D>().
        shieldReflectAudioSource = gameObject.AddComponent<AudioSource>();
        //shieldReflectAudioSource.outputAudioMixerGroup = LevelController.instance.SFXMixer;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        shieldReflectAudioSource.outputAudioMixerGroup = LevelController.instance.SFXMixer;

        var projController = collision.GetComponent<ProjectileController>();

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

                LevelController.instance.Explosion(collision.transform.position);
                Destroy(collision.gameObject);


                StartCoroutine(Utils.ActivateBehaviour(shieldReactivationDelay, gameObject.GetComponent<Collider2D>()));
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


}
