using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    public GameObject projectilePrefab;
    public AudioClip projectileSound;

    public int shots;
    public float timeInterval;

    private GameObject aimAt;
    private float projectileTimer;

    // Start is called before the first frame update
    void Start()
    {
        aimAt = LevelController.instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (aimAt)
        {
            Vector2 dir = aimAt.transform.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            angle -= 90f;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);

        }

        if (projectileTimer > timeInterval + Random.Range(-timeInterval, timeInterval) / 2f)
        {
            projectileTimer = 0f;

            for (int i = 0; i < shots; i++)
            {
                //shotAudioSource.PlayOneShot(shotSound);
                var newProj = Instantiate(projectilePrefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                newProj.GetComponent<ProjectileController>().origin = gameObject;
                newProj.transform.up = transform.up;

            }
        }

        projectileTimer += Time.deltaTime;
    }
}
