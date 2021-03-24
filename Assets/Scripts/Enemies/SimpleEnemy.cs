using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : IEnemy
{
    // Start is called before the first frame update

    private Vector3 directionLocal;



    

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

        base.Update();

        transform.up = direction;


        transform.position += (direction * speed) * Time.deltaTime;

        NormalShot();

    }
}
