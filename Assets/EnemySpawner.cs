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

    public float Invert;
    void Start()
    {
        Invert = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawn)
        {
            enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemyStartPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
            Spawn = false;
            Anim = true;
            time = 0f;
        }

        if (Anim)
        {
            enemy.GetComponent<IEnemy>().Move = false;
            var x = Mathf.Clamp(X.Evaluate(time) * Invert + enemyStartPos.x, 0.2f, 0.8f);
            var y = Mathf.Clamp(Y.Evaluate(time) + enemyStartPos.y, 0.2f, 0.8f);
            enemy.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0f) + Vector3.forward * 10f);
            time += Time.deltaTime * animSpeed;
        }
    }
}
