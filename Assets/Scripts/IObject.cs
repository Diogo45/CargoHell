using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IObject : MonoBehaviour
{
    // Start is called before the first frame update

    public float health;
    public GameObject explosion;
    public AudioClip explosionSound;
    public Vector3 direction;
    private bool enteredScene = false;
    public float speed;


    void Start()
    {
        
    }

    // Update is called once per frame
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
    }

    public void OutOfBounds()
    {
        Destroy(gameObject);

    }
}
