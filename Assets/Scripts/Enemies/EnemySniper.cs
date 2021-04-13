using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniper : IEnemy
{
    // Start is called before the first frame update




    void Start()
    {
        base.Start();
        aim = true;
        aimAt = LevelController.instance.Player;
        baseScore = 200;
    }




    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (ShouldMove)
            transform.position += (direction * speed) * Time.deltaTime;

        aimAt = LevelController.instance.Player;

        NormalShot();


    }
}
