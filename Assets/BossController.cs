using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : IEnemy
{

    public Vector3 startRotation;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(startRotation);
        transform.position += transform.up * 2;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
