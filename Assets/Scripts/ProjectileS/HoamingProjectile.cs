using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoamingProjectile : ProjectileController
{
    // Start is called before the first frame update

    private GameObject aimAt;
    public float RotationSpeed;
    public float RotationSpeedIncrease;

    [SerializeField]
    private float Fuel;
    private float startFuel;
    private float fuelStrobePercentage = 0.2f;
    [SerializeField]
    private float FuelConsumptionRate;
    [SerializeField]
    private int respawnTimes;
    private int respawnCount;

   [field: SerializeField] public GameObject Flame { get; private set; }

    void Start()
    {
        aimAt = GameObject.Find("Player");
        startFuel = Fuel;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (aimAt && Fuel > 0)
        {

            if (Fuel < startFuel * fuelStrobePercentage)
            {
                StartCoroutine(Utils.StrobeColor(gameObject, Color.white, 10));
            }

            float dist = Vector2.Distance(transform.position, aimAt.transform.position);

            Vector2 dir = aimAt.transform.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            angle -= 90f;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (dist < 3f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, (RotationSpeed + RotationSpeedIncrease) * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
            }

            Fuel -= FuelConsumptionRate;
        }
        else
        {
            Flame.SetActive(false);
            tag = "ProjectileSpinner";
        }
       
        


    }

    public override void OutOfBounds()
    {
        if (respawnCount <= respawnTimes)
        {
            var pos = Camera.main.WorldToViewportPoint(transform.position);
            if (pos.x > 1 || pos.x < 0 || pos.y > 1 || pos.y < 0)
            {
                transform.position = FlowPos().Item1;
            }                
        }
        else
        {
            Destroy(this);
        }
        
    }


    public (Vector2, Vector2) FlowPos()
    {
        int side = 0;
        float inSide = 0;
        var pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.y > 1)
        {
            side = 2;
            inSide = 1 - (pos.x);
        }
        else if (pos.y < 0)
        {
            side = 0;
            inSide = 1 - (pos.x);
        }
        else if (pos.x > 1)
        {
            side = 3;
            inSide = 1 - (pos.y);
        }
        else if (pos.x < 0)
        {
            side = 1;
            inSide = 1 - (pos.y);
        }


        Vector3 spawnPos = Vector3.zero;
        Vector2 direction = Vector2.zero;
        switch (side)
        {
            case 0:
                spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(inSide, 1, 0));
                direction = Vector2.down;
                break;
            case 1:
                spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1, inSide, 0));
                direction = Vector2.left;
                break;
            case 2:
                spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(inSide, 0, 0));
                direction = Vector2.up;
                break;
            case 3:
                spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0, inSide, 0));
                direction = Vector2.right;
                break;
        }

        return (spawnPos, direction);
    }

}
