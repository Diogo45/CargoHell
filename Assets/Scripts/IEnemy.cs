using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum EnemyType
{
    SIMPLE, SNIPER, SPINNER, STORMTROOPER, BOSS
}

public class IEnemy : MonoBehaviour
{
    public float health;
    public float initialHealth { get; private set; }
    public EnemyType type;

    public GameObject projectile;
    public GameObject explosion;
    public AudioClip shotSound;

    public Vector3 direction;
    private bool enteredScene = false;
    public float speed;
    private bool hasCollided;
    public bool isOutOfBounds;

    public int baseScore = 1;

    public bool ShouldMove;
    public bool ShouldShoot = true;


    public delegate void OnDestroy(GameObject obj, ProjectileController projectile);
    public static event OnDestroy OnDestroyEvent;

    public delegate void OnDamaged(GameObject obj, ProjectileController projectile);
    public static event OnDamaged OnDamagedEvent;


    private ProjectileController lastProjectile;

    protected AudioSource shotAudioSource;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ProjectileReflected")
        {
            if (!hasCollided)
            {
                health--;
                hasCollided = true;
                lastProjectile = collision.GetComponent<ProjectileController>();
            }

        }
    }

    public void Start()
    {
        shotAudioSource = gameObject.AddComponent<AudioSource>();
        shotAudioSource.outputAudioMixerGroup = LevelController.instance.SFXMixer;
        ShouldMove = true;
        ShouldShoot = true;
        initialHealth = health;
    }


    public void Update()
    {
        var pos = Camera.main.WorldToViewportPoint(transform.position);


        if (enteredScene)
        {
            isOutOfBounds = false;
            if (pos.x > 1 || pos.x < 0 || pos.y > 1 || pos.y < 0)
            {
                OutOfBounds();
                isOutOfBounds = true;
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
        else if (hasCollided)
        {
            hasCollided = false;
            OnDamagedEvent(gameObject, lastProjectile);
        }
 

       

    }

    public virtual void OutOfBounds()
    {
        var dirPos = LevelController.instance.RequestRandomPos();
        gameObject.transform.position = dirPos.Item1;
        direction = dirPos.Item2;

    }


}
