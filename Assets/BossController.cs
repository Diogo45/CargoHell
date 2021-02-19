using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : IEnemy
{

    public enum AnimStates
    {
        None = 0, Smalling, Teleport, Bigenning
    }

    public Vector3 startRotation;

    private Vector3 startPos;
    private Vector3 currGoal;

    public TurretController turretLeft;
    public TurretController turretRight;

    public bool height;
    public bool TP;
    public float side;

    public int timer;

    public EnemySpawner spawner;
    private Material material;
    private bool finishedAnimation;
    private float animationTime = 3f;

    private bool ShouldTP = true;
    private AnimStates animStates = AnimStates.None;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        material = gameObject.GetComponent<SpriteRenderer>().material;
        transform.rotation = Quaternion.Euler(startRotation);
        transform.position += transform.up * 2;
        startPos = transform.position;
        side = 1f;
        StartCoroutine(spawnSniper());
        TP = false;
        finishedAnimation = true;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (turretLeft.isShooting)
            timer++;

        

        if (ShouldTP && timer >= 5)
        {

            turretLeft.shouldShoot = turretRight.shouldShoot = false;
            ShouldMove = false;
            ShouldShoot = false;

            if(animStates == AnimStates.None)
                animStates = AnimStates.Smalling;

            if (transform.localScale.x <= 0.1f && !TP)
            {
                animStates = AnimStates.Teleport;

            }

            if (animStates == AnimStates.Smalling)
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * animationTime);

            if (animStates == AnimStates.Teleport)
            {

                side = -side;
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, -transform.rotation.z, transform.rotation.w);
                if (side == 1f)
                {
                    transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0)) + Vector3.forward * 10f;
                    spawner.Invert = -1f;
                }
                else if (side == -1f)
                {
                    transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0)) + Vector3.forward * 10f;
                    spawner.Invert = 1f;
                }
                startPos = transform.position + transform.up * 2;

                if (height)
                {
                    currGoal = startPos + new Vector3(Random.Range(-2f, -1f) * side, Random.Range(1.5f, 2f), 0f);
                    height = false;
                }
                else
                {
                    currGoal = startPos + new Vector3(Random.Range(-2f, -1f) * side, Random.Range(-2f, -1.5f), 0f);
                    height = true;
                }

                TP = true;
                animStates = AnimStates.Bigenning;


            }

            if(animStates == AnimStates.Bigenning)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * animationTime);

                if (transform.localScale.x >= 0.9f)
                {
                    animStates = AnimStates.None;
                    StartCoroutine(ResetTeleport());
                    TP = false;
                    timer = 0;
                }
            }


           
        }

        if (ShouldShoot && Vector3.Distance(transform.position, currGoal) < 0.1f  /*&& (turretLeft.isShooting || turretRight.isShooting)*/)
        {

            if (height)
            {
                currGoal = startPos + new Vector3(Random.Range(-2f, -1f) * side, Random.Range(1.5f, 2f), 0f);
                height = false;
            }
            else
            {
                currGoal = startPos + new Vector3(Random.Range(-2f, -1f) * side, Random.Range(-2f, -1.5f), 0f);
                height = true;
            }

            turretLeft.shouldShoot = turretRight.shouldShoot = true;
        }

        if (ShouldMove)
            transform.position = Vector3.Lerp(transform.position, currGoal, Time.deltaTime);

        //transform.position = startPos + new Vector3(Random.Range(-1f, 0f), Random.Range(-1, 1f), 0f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Player" || collision.tag == "ProjectileReflected")
        {
            StartCoroutine(StrobeColor(Color.white));
        }

    }


    IEnumerator ResetTeleport()
    {
        yield return new WaitForSeconds(0.5f);
        ShouldMove = true;
        ShouldShoot = true;
        yield break;
    }

    IEnumerator spawnSniper()
    {
        yield return new WaitForSeconds(2f);

        if (!spawner.enemy)
        {
            yield return new WaitForSeconds(5f);
            spawner.Spawn = true;
        }

        yield return spawnSniper();
        yield break;
    }


    IEnumerator StrobeColor(Color c)
    {
        for (int i = 0; i < 1; i++)
        {
            c.a = 0.8f;
            SetColor(c);
            yield return new WaitForSeconds(0.1f);
            c.a = 0f;
            SetColor(c);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SetColor(Color color)
    {
        material.SetColor("_Color", color);
    }

}
