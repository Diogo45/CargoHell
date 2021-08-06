using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoHell
{
    public class TurretController : MonoBehaviour
    {

        public GameObject projectilePrefab;
        private AudioSource audioSource;

        public int shots;
        private int shootedShots;
        public float timeInterval;

        private GameObject aimAt;
        private float projectileTimer;

        public bool isShooting = false;

        [HideInInspector]
        public bool shouldShoot;

        private bool fire = true;

        // Start is called before the first frame update
        void Start()
        {
            aimAt = LevelController.instance.Player;
            shouldShoot = false;
            audioSource = GetComponent<AudioSource>();
            shootedShots = 0;
        }

        // Update is called once per frame
        void Update()
        {
            isShooting = false;
            if (aimAt)
            {
                Vector2 dir = (aimAt.transform.position) - transform.position;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                angle -= 90f;

                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);

            }

            if (projectileTimer > timeInterval + Random.Range(-timeInterval, timeInterval) / 2f && shouldShoot)
            {



                if (fire && shouldShoot)
                {
                    if (!audioSource.isPlaying)
                        audioSource.Play();

                    fire = false;
                    var newProj = Instantiate(projectilePrefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                    newProj.GetComponent<ProjectileController>().origin = gameObject;
                    newProj.transform.up = transform.up;
                    shootedShots++;

                    StartCoroutine(wait());
                }
                if (shootedShots >= shots)
                {
                    audioSource.Stop();
                    isShooting = true;
                    shootedShots = 0;
                    projectileTimer = 0f;
                }

                //StartCoroutine(TripleShot());
            }

            projectileTimer += Time.deltaTime;
        }

        IEnumerator TripleShot()
        {
            for (int i = 0; i < shots; i++)
            {
                if (!shouldShoot)
                    yield break;
                //shotAudioSource.PlayOneShot(shotSound);
                yield return new WaitForSeconds(0.1f);
                var newProj = Instantiate(projectilePrefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                newProj.GetComponent<ProjectileController>().origin = gameObject;
                newProj.transform.up = transform.up;

            }
            shouldShoot = false;

            yield break;
        }

        IEnumerator wait()
        {
            yield return new WaitForSeconds(0.1f);
            fire = true;
        }

    }
}