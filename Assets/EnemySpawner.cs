using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update

    private class Control
    {
        public bool Anim;
        public float InvertEnemy;
        public float InvertY;
        public float Time;
        public Vector3 EnemyStartPos;
    }

    public AnimationCurve X;
    public AnimationCurve Y;

    public GameObject enemyPrefab;
    public List<GameObject> enemy { get; private set; }
    public Vector3 enemyStartPos;

    public bool Spawn = false;
    private Dictionary<GameObject, Control> CTRL;

    private float time = 0f;
    public float animSpeed;
    public float spawnDelay;

    public float invert;
    public float invertEnemy;
    private float invertY;

    //[SerializeField]
    private bool deathDelay;

    void Start()
    {
        enemy = new List<GameObject>();
        CTRL = new Dictionary<GameObject, Control>();
        invert = -1f;
        invertY = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawn && !deathDelay)
        {

            var newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            newEnemy.GetComponent<IEnemy>().type = EnemyType.SNIPER;
            newEnemy.GetComponent<IEnemy>().speed = 3.5f;
            Spawn = false;
            time = 0f;
            invertY *= -1f;
            invertEnemy = invert;

            enemyStartPos = Camera.main.WorldToViewportPoint(newEnemy.transform.position);

            CTRL.Add(newEnemy, new Control { Anim = true, InvertY = invertY, InvertEnemy = invertEnemy, EnemyStartPos = enemyStartPos, Time = 0f});
            enemy.Add(newEnemy);
           
        }

        if (enemy.Count > 0)
        {
            foreach (var item in enemy)
            {
                var ie = item.GetComponent<IEnemy>();

                if (CTRL[item].Anim)
                {
                    if (ie.isOutOfBounds)
                    {
                        CTRL[item].Anim = false;
                        ie.ShouldMove = true;
                        return;
                    }
                    ie.ShouldMove = false;
                    var x = (X.Evaluate(CTRL[item].Time) *  CTRL[item].InvertEnemy + CTRL[item].EnemyStartPos.x);
                    var y = Mathf.Clamp(Y.Evaluate(CTRL[item].Time) * CTRL[item].InvertY + CTRL[item].EnemyStartPos.y, 0.05f, 0.95f);
                    item.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0f) + Vector3.forward * 10f);
                    CTRL[item].Time += Time.deltaTime * animSpeed;
                }
            }

            
        }


    }

    private void OnEnable()
    {
        IEnemy.OnDestroyEvent += IEnemy_OnDestroyEvent;
    }

    private void OnDisable()
    {
        IEnemy.OnDestroyEvent -= IEnemy_OnDestroyEvent;
    }
    
    IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(10f);
        deathDelay = false;
        yield break;
    }

    private void IEnemy_OnDestroyEvent(GameObject obj, ProjectileController projectile)
    {
        //Debug.Log(obj.GetComponent<IEnemy>().type);
        if (obj.GetComponent<IEnemy>().type == EnemyType.SNIPER && enemy.Contains(obj))
        {
            enemy.Remove(obj);
            CTRL.Remove(obj);
            deathDelay = true;
            if(enemy.Count == 0)
            {
                StartCoroutine(ResetDelay());

            }
        }
            
    }


}
