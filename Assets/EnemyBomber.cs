using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : IEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        transform.up = direction;
        aim = false;
        baseScore = 100;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (direction * speed) * Time.deltaTime;

        transform.up = direction;

        NormalShot();
        base.Update();
    }
}
