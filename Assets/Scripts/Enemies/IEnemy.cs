using CargoHell;
using CargoHell.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum EnemyType
{
    NONE, SIMPLE, SNIPER, SPINNER, STORMTROOPER, BOSS, BOMBER, SHOOTER, CHASER
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

    protected float projectileTimer;
    public float timerThreshold;



    public delegate void OnDestroy(GameObject obj, ProjectileController projectile);
    public static event OnDestroy OnDestroyEvent;

    public delegate void OnDamaged(GameObject obj, ProjectileController projectile);
    public static event OnDamaged OnDamagedEvent;


    private ProjectileController lastProjectile;

    protected AudioSource shotAudioSource;

    protected GameObject aimAt;
    protected bool aim;


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ProjectileReflected" || collision.gameObject.tag == "ProjectileSpinner")
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
        shotAudioSource.outputAudioMixerGroup = AudioController.instance.SFXMixer;
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


        if (aimAt && aim)
        {
            Vector2 dir = aimAt.transform.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            angle -= 90f;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);

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


        projectileTimer += Time.deltaTime;

    }

    public virtual void OutOfBounds()
    {
        var dirPos = LevelController.instance.RequestRandomPos();
        gameObject.transform.position = dirPos.Item1;
        direction = dirPos.Item2;

    }

    public void NormalShot()
    {

        if (projectileTimer > timerThreshold + Random.Range(-timerThreshold, timerThreshold) / 2f)
        {
            projectileTimer = 0f;
            //shotAudioSource.PlayOneShot(shotSound);
            var newProj = Instantiate(projectile, transform.position + (transform.up * 0.5f), Quaternion.identity);
            newProj.GetComponent<ProjectileController>().origin = gameObject;
            newProj.transform.up = transform.up;
        }

    }

    public void MultiShot(int numShots)
    {

        if (projectileTimer > timerThreshold + Random.Range(-timerThreshold, timerThreshold) / 2f)
        {
            projectileTimer = 0f;
            StartCoroutine(ShootMany(numShots));   
        }

    }

    IEnumerator ShootMany(int numShots)
    {
        for(int i=0; i<numShots; i++)
        {
            var newProj = Instantiate(projectile, transform.position + (transform.up * 0.5f), Quaternion.identity);
            newProj.GetComponent<ProjectileController>().origin = gameObject;
            newProj.transform.up = transform.up;
            yield return new WaitForSeconds(0.1f);
        }
        yield break;

    }
    public void HoamingShot()
    {

        if (projectileTimer > timerThreshold + Random.Range(-timerThreshold, timerThreshold) / 2f)
        {
            projectileTimer = 0f;
            //shotAudioSource.PlayOneShot(shotSound);
            var newProj = Instantiate(projectile, transform.position + (transform.up * 0.5f), Quaternion.identity);
            newProj.GetComponent<ProjectileController>().origin = gameObject;
            newProj.transform.up = transform.up;
        }

    }
}
