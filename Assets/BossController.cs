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

    public bool gambiarra;
    public float side;

    public int timer;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        transform.rotation = Quaternion.Euler(startRotation);
        transform.position += transform.up * 2;
        startPos = transform.position;
        side = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        if(Vector3.Distance(transform.position, currGoal) < 0.1f && (turretLeft.isShooting || turretRight.isShooting))
        {
            timer++;
            if (gambiarra)
            {
                currGoal = startPos + new Vector3(Random.Range(-2f, -1f) * side, Random.Range(1.5f, 2f), 0f);
                gambiarra = false;
            }
            else
            {
                currGoal = startPos + new Vector3(Random.Range(-2f, -1f) * side, Random.Range(-2f, -1.5f), 0f);
                gambiarra = true;
            }

        }

        if(timer > 2)
        {
            side = -side;
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, -transform.rotation.z, transform.rotation.w);
            if(side == 1f)
            {
                transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0)) + Vector3.forward * 10f;
            }
            else if(side == -1f)
            {
                transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0)) + Vector3.forward * 10f;
                
            }
            startPos = transform.position + transform.up * 2;

            if (gambiarra)
            {
                currGoal = startPos + new Vector3(Random.Range(-2f, -1f) * side, Random.Range(1.5f, 2f), 0f);
                gambiarra = false;
            }
            else
            {
                currGoal = startPos + new Vector3(Random.Range(-2f, -1f) * side, Random.Range(-2f, -1.5f), 0f);
                gambiarra = true;
            }

            timer = 0;
        }

        transform.position = Vector3.Lerp(transform.position, currGoal, Time.deltaTime);

        //transform.position = startPos + new Vector3(Random.Range(-1f, 0f), Random.Range(-1, 1f), 0f);

    }
}
