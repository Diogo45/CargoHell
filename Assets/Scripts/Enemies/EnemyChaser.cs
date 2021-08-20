﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoHell;

public class EnemyChaser : IEnemy

{
    private float animTimer = 0.0f;
    private bool canCrash = false;


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (canCrash && collision.tag == "Enemy" && collision.gameObject.GetComponent<IEnemy>().type != EnemyType.CHASER)
        {
            collision.gameObject.GetComponent<IEnemy>().health--;
            health--;
        }

        base.OnTriggerEnter2D(collision);
    }


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
            speed += 0.2f;
            canCrash = true;
        }
        else
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        animTimer += Time.deltaTime;
    }

    public override void OutOfBounds()
    {
        return;
    }

}