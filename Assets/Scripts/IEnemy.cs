using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum EnemyType
{
    SIMPLE, SNIPER, SPINNER, STORMTROOPER
}

public class IEnemy : MonoBehaviour
{
    public float health;
    public EnemyType type;

    public GameObject projectile;
    public GameObject explosion;
    public AudioClip explosionSound;
    public AudioClip shotSound;

    public Vector3 direction;
    private bool enteredScene = false;
    public float speed;
    private bool hasCollided;

    public int baseScore = 1;

    public delegate void OnDestroy(GameObject obj, ProjectileController projectile);
    public static event OnDestroy OnDestroyEvent;

    private ProjectileController lastProjectile;

    protected AudioSource shotAudioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ProjectileReflected")
        {
            if (!hasCollided)
            {
                health--;
                //Destroy(collision.gameObject);
                hasCollided = true;
                lastProjectile = collision.GetComponent<ProjectileController>();
            }

        }
    }

    public void Start()
    {
        shotAudioSource = gameObject.AddComponent<AudioSource>();
        shotAudioSource.outputAudioMixerGroup = LevelController.instance.SFXMixer;
    }


    public void Update()
    {
        var pos = Camera.main.WorldToViewportPoint(transform.position);


        if (enteredScene)
        {

            if (pos.x > 1 || pos.x < 0 || pos.y > 1 || pos.y < 0)
            {
                OutOfBounds();
            }


        }

        if (pos.x < 1 && pos.x > 0 && pos.y < 1 && pos.y > 0)
        {
            enteredScene = true;
        }

        if (health <= 0)
        {
            OnDestroyEvent(gameObject, lastProjectile);
        }

        

    }

    public void OutOfBounds()
    {
        var dirPos = LevelController.instance.RequestRandomPos();
        gameObject.transform.position = dirPos.Item1;
        direction = dirPos.Item2;

    }


}
