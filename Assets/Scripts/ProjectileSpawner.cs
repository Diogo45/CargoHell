using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject projectile;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var newProjectile = Instantiate(projectile, mousePos + projectile.transform.position,Quaternion.identity );

            newProjectile.transform.up = Vector2.left;

            //newProjectile.transform.rotation = projectile.transform.rotation;
        }


    }
}
