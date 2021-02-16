using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public AnimationCurve X;
    public AnimationCurve Y;

    public GameObject enemyPrefab;
    public GameObject enemy;
    public Vector3 enemyStartPos;

    public bool Spawn = false;
    public bool Anim = false;

    private float time = 0f;
    public float animSpeed;
    public float spawnDelay;

    public float Invert;
    public float InvertEnemy;
    private float InvertY;
    private bool delayed;

    void Start()
    {
        Invert = -1f;
        InvertY = 1f;
        delayed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawn)
        {
            //if (!delayed)
            //{
            enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemyStartPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
            Spawn = false;
            Anim = true;
            time = 0f;
            InvertY *= -1f;
            InvertEnemy = Invert;
            //delayed = true;
            //}

        }

        if (enemy)
        {
            var ie = enemy.GetComponent<IEnemy>();

            if (Anim)
            {
                if (ie.isOutOfBounds)
                {
                    Anim = false;
                    ie.ShouldMove = true;
                    return;
                }
                ie.ShouldMove = false;
                var x = (X.Evaluate(time) * InvertEnemy + enemyStartPos.x);
                var y = Mathf.Clamp(Y.Evaluate(time) * InvertY + enemyStartPos.y, 0.05f, 0.95f);
                enemy.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0f) + Vector3.forward * 10f);
                time += Time.deltaTime * animSpeed;
            }
        }
        //else 
        //{
        //    StartCoroutine(Delay());
        //}

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(spawnDelay);
        delayed = false;
        yield break;
    }
}
