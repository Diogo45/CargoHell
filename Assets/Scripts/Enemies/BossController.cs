using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoHell
{
    public class BossController : IEnemy
    {

        public enum AnimStates
        {
            None = 0, Smalling, Teleport, Bigenning
        }

        public Vector3 startRotation;

        private Vector3 startPos;
        private Vector3 currGoal;

        public TurretController turretLeft;
        public TurretController turretRight;

        public bool height;
        public bool TP;
        public float side;

        public int timer;

        public EnemySpawner spawner;
        private Material material;
        private bool finishedAnimation;
        private float animationTime = 3f;

        private bool ShouldTP = false;
        private AnimStates animStates = AnimStates.None;

        public bool SecondPhase { get; private set; } = false;

        public int TPThreashold { get; private set; } = 5;
        public Vector3 TPOffset { get; private set; } = Vector3.zero;
        public int SideChange { get; private set; } = 0;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            material = gameObject.GetComponent<SpriteRenderer>().material;
            transform.rotation = Quaternion.Euler(startRotation);
            transform.position += transform.up * 2;
            startPos = transform.position;
            side = 1f;
            StartCoroutine(spawnSniper());
            TP = false;
            finishedAnimation = true;

            // -> BeginState

        }

        //OnBeginState
        // if -> state01
        // else -> state02




        // Update is called once per frame
        void Update()
        {

            if(health == 0)
            {
                Destroy(spawner);
            }

            base.Update();

            if (turretLeft.isShooting)
                timer++;

            if (health < initialHealth / 2f)
            {
                SecondPhase = true;
                TPThreashold = 1;
            }

            if(timer >= TPThreashold)
            {
                StartCoroutine(StartTeleport());
                timer = 0;
            }

            if (ShouldTP )
            {

                turretLeft.shouldShoot = turretRight.shouldShoot = false;
                ShouldMove = false;
                ShouldShoot = false;

                if (animStates == AnimStates.None)
                    animStates = AnimStates.Smalling;

                if (transform.localScale.x <= 0.1f && !TP)
                {
                    animStates = AnimStates.Teleport;

                }

                if (animStates == AnimStates.Smalling)
                    transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * animationTime);

                if (animStates == AnimStates.Teleport)
                {

                    if (SecondPhase)
                    {

                        TPOffset = new Vector3(Random.Range(0.05f, 0.2f), Random.Range(-0.3f, 0.3f), 0f);

                        SideChange++;
                        if (SideChange >= Random.Range(0, 3))
                        {
                            side = -side;
                            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, -transform.rotation.z, transform.rotation.w);
                            SideChange = 0;
                        }
                    }
                    else
                    {
                        side = -side;
                        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, -transform.rotation.z, transform.rotation.w);
                        TPOffset = Vector3.zero;
                    }

                    if (side == 1f)
                    {
                        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0) + TPOffset) + Vector3.forward * 10f;
                        spawner.invert = -1f;
                    }
                    else if (side == -1f)
                    {
                        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0) + TPOffset) + Vector3.forward * 10f;
                        spawner.invert = 1f;
                    }
                    startPos = transform.position + transform.up * 2;

                    if (height)
                    {
                        currGoal = startPos + new Vector3(Random.Range(-2f, -1f) * side, Random.Range(1.5f, 2f), 0f);
                        height = false;
                    }
                    else
                    {
                        currGoal = startPos + new Vector3(Random.Range(-2f, -1f) * side, Random.Range(-2f, -1.5f), 0f);
                        height = true;
                    }

                    TP = true;
                    animStates = AnimStates.Bigenning;


                }

                if (animStates == AnimStates.Bigenning)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * animationTime);

                    if (transform.localScale.x >= 0.9f)
                    {
                        animStates = AnimStates.None;
                        StartCoroutine(ResetTeleport());
                        TP = false;
                        timer = 0;
                    }
                }



            }

            //Vector3 rand;
            ////if (SPHeight)
            ////{
            ////   rand = new Vector3(Random.Range(-1f, 0f) * side, Random.Range(0f, 1f), 0f);
            ////    SPHeight = false;
            ////}
            ////else
            ////{
            ////    rand = new Vector3(Random.Range(-1f, 0f) * side, Random.Range(-1f, 0f), 0f);
            ////    SPHeight = true;
            ////}



            if (ShouldShoot && Vector3.Distance(transform.position, currGoal) < 0.8f  /*&& (turretLeft.isShooting || turretRight.isShooting)*/)
            {
                //Maybe in the direction of player when theres no snipers
                if (height)
                {
                    currGoal = startPos + new Vector3(Random.Range(-1.5f, -1f) * side, Random.Range(1.5f, 1f), 0f);
                    height = false;
                }
                else
                {
                    currGoal = startPos + new Vector3(Random.Range(-1.5f, -1f) * side, Random.Range(-3f, -1f), 0f);
                    height = true;
                }


                turretLeft.shouldShoot = turretRight.shouldShoot = true;
            }

            if (ShouldMove)
                transform.position = Vector3.Lerp(transform.position, currGoal, Time.deltaTime);




            //rand = new Vector3(/*Random.Range(0f, 0.8f) * SPSide*/ 0f, Random.Range(0f, 0.8f) * SPHeight, 0f);
            //SPSide *= -1f;
            //SPHeight *= -1f;

            ////if (SecondPhase)
            //if(Vector3.Distance(transform.position, transform.position + rand) <= 1f)
            //{
            //    transform.position = transform.position + rand;
            //}




        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            if (collision.tag == "Player" || collision.tag == "ProjectileReflected")
            {
                StartCoroutine(StrobeColor(Color.white));
            }

        }

        IEnumerator StartTeleport()
        {
            ShouldMove = false;
            ShouldShoot = false;
            turretLeft.shouldShoot = turretRight.shouldShoot = false;
            //Debug.Log("WAIT TP");
            yield return new WaitForSeconds(.5f);
            //Debug.Log("START TP");

            ShouldTP = true;
            yield break;
        }


        IEnumerator ResetTeleport()
        {
            ShouldTP = false;
            yield return new WaitForSeconds(0.5f);
            ShouldMove = true;
            ShouldShoot = true;
            yield break;
        }

        IEnumerator spawnSniper()
        {

            yield return new WaitForSeconds(0.5f);

            if (spawner.enemy.Count < 2)
            {
                //yield return new WaitForSeconds(2f);
                spawner.Spawn = true;
            }




            yield return spawnSniper();
            yield break;
        }


        IEnumerator StrobeColor(Color c)
        {
            for (int i = 0; i < 1; i++)
            {
                c.a = 0.8f;
                SetColor(c);
                yield return new WaitForSeconds(0.1f);
                c.a = 0f;
                SetColor(c);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void SetColor(Color color)
        {
            material.SetColor("_Color", color);
        }


        public override void OutOfBounds()
        {
            var pos = Camera.main.WorldToViewportPoint(transform.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            transform.position = Camera.main.ViewportToWorldPoint(pos);

        }
    }
}