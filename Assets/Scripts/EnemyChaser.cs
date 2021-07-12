using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : IEnemy

{
    private float animTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        baseScore = 250;
        aim = false;
        transform.up = direction;

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (!aim && animTimer < 1.5)
        {
            //TODO: There should be a charge-up animation before it chases the player - for now it will just move forward for a bit
            transform.position += (direction * speed) * Time.deltaTime;
        }
        else if (animTimer >= 1.5 && animTimer < 3)
        {
            aim = true;
            aimAt = LevelController.instance.Player;
            transform.position -= (direction * (speed * 0.6f)) * Time.deltaTime;
        }
        else if (animTimer >= 3 && animTimer < 3.5)        
        {
            //aim = true;
            //aimAt = LevelController.instance.Player;
            speed += 0.2f;
        }
        else
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        animTimer += Time.deltaTime;
    }
}
