using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    SIMPLE, SNIPER, SPINNER, STORMTROOPER
}

public class IEnemy : MonoBehaviour
{
    public float health;
    public EnemyType type;

    public GameObject projectile;
    public GameObject explosion;
    public AudioClip explosionSound;
    public Vector3 direction;
    private bool enteredScene = false;
    public float speed;


    public delegate void OnDestroy(GameObject obj);
    public static event OnDestroy OnDestroyEvent;

    public void Update()
    {
        var pos = Camera.main.WorldToViewportPoint(transform.position);


        if (enteredScene)
        {

            if (pos.x > 1 || pos.x < 0 || pos.y > 1 || pos.y < 0)
            {
                OutOfBounds();
            }


        }

        if (pos.x < 1 && pos.x > 0 && pos.y < 1 && pos.y > 0)
        {
            enteredScene = true;
        }

        if (health <= 0)
        {
            OnDestroyEvent(gameObject);
        }

        

    }

    public void OutOfBounds()
    {
        var dirPos = LevelController.instance.RequestRandomPos();
        gameObject.transform.position = dirPos.Item1;
        direction = dirPos.Item2;

    }


}
