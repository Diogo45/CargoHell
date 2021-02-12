using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : IEnemy
{

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

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        transform.rotation = Quaternion.Euler(startRotation);
        transform.position += transform.up * 2;
        startPos = transform.position;
        side = 1f;
        StartCoroutine(spawnSniper());
        TP = false;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (turretLeft.isShooting)
            timer++;

        if (timer >= 2)
        {
            turretLeft.shouldShoot = turretRight.shouldShoot = false;

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
            timer = 0;

        }

        if (Vector3.Distance(transform.position, currGoal) < 0.1f /*&& (turretLeft.isShooting || turretRight.isShooting)*/)
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

        //if (!TP)
            transform.position = Vector3.Lerp(transform.position, currGoal, Time.deltaTime);

        //transform.position = startPos + new Vector3(Random.Range(-1f, 0f), Random.Range(-1, 1f), 0f);

    }


    IEnumerator spawnSniper()
    {
        yield return new WaitForSeconds(2f);

        if (!spawner.enemy)
        {
            spawner.Spawn = true;
        }

        yield return spawnSniper();
        yield break;
    }


    IEnumerator turnOnTurret()
    {
        //yield return new WaitForSeconds(0.5f);

        turretLeft.shouldShoot = turretRight.shouldShoot = true;

        yield break;
    }


    IEnumerator turnOnMove()
    {
       yield return new WaitForSeconds(10f);

       TP = false;

        yield break;
    }
}
