﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{


    public GameObject player;
    private PlayerController ship;


    private Vector2 debugNormal;
    private Vector2 debugPoint;

    public AudioClip shieldReflectSound;
    private AudioSource shieldReflectAudioSource;

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


        if (collision.gameObject.tag == "Projectile" || collision.gameObject.tag == "ProjectileReflected" || collision.gameObject.tag == "ProjectileSpinner")
        {
            //RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, collision.transform.up, 100);



            var projController = collision.GetComponent<ProjectileController>();

            if(projController.projectileType == ProjectileController.ProjectileType.HOMING)
            {

                Instantiate(LevelController.instance.explosionAnim, collision.transform.position, Quaternion.identity);
                Destroy(collision);
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


}
