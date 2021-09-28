using CargoHell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySniper : IEnemy
{

    void Start()
    {
        base.Start();
        aim = true;
        aimAt = LevelController.instance.Player;
        baseScore = 200;
    }


    void Update()
    {
        base.Update();

        if (ShouldMove)
            transform.position += (direction * speed) * Time.deltaTime;

        aimAt = LevelController.instance.Player;

        NormalShot();


    }
}
