using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CargoHell
{

    using Animation;
    using Audio;

    public class LevelController : MonoBehaviour
    {
        #region Events
        public delegate void OnSpawnEnemy(IEnemy controller);
        public static event OnSpawnEnemy onSpawnEnemy;

        public delegate void OnEndLevel(bool win);
        public static event OnEndLevel onEndLevel;

        #endregion

        public static LevelController instance;

        [SerializeField]
        public static int _levelID;

        private bool IsFile = false;

        [SerializeField]
        private LevelList _levelList;
        public Level _level { get; private set; }

        [SerializeField]
        private EnemyList _enemyTypes;

        public int spawnFrame = 0;

        public GameObject Player;
        public GameObject Shield;



        #region CanvasRefs

        //public GameObject GameOverCanvas;
        public GameObject WinCanvas;

        #endregion

        // Quantity of enemies of each type to spawn during the whole level
        public EnemyObjectDictionary enemySpawnCount;
        // Currently alive enemies of each type
        public EnemyObjectDictionary spawned;
        // Max number of enemies of each type that can present on screen at one time
        public EnemyObjectDictionary maxScreenEnemies;

        public List<GameObject> enemyAlive;

        public List<GameObject> powerUpPrefabs;

        public bool hasWon { get; private set; } = false;
        public bool hasLost { get; private set; } = false;

        private bool finishedSpawn = false;
        private bool startedSpawn;

        public int Score = 0;
        public float MultIncrease = 1;




        // Start is called before the first frame update
        void Start()
        {
            #region Singleton, é isso

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            #endregion

            Application.targetFrameRate = 144;

            IsFile = true;


            //ISSO VAI PRA INPUT

            try
            {
#if UNITY_ANDROID
            WWW reader = new WWW(Application.streamingAssetsPath + @"\Levels\Level_" + sceneFile + ".json");
            while (!reader.isDone)
            {
            }
            sceneArgs = JsonUtility.FromJson<SceneArgs>(reader.text);
#endif


#if UNITY_STANDALONE

                _level = _levelList[_levelID];

                var Joystick1 = GameObject.Find("LookJoystick");
                var Joystick2 = GameObject.Find("MoveJoystick");
                Joystick1.SetActive(false);
                Joystick2.SetActive(false);

#endif
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                var errorObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
                errorObj.transform.position = Player.transform.position;
            }

            enemySpawnCount = new EnemyObjectDictionary();
            for (int i = 0; i < _level.LevelConfig.Count; i++)
            {
                for (int j = 0; j < _level.LevelConfig[i].enemies.Count; j++)
                {
                    var enemy = _level.LevelConfig[i].enemies[j].enemyType;
                    if (enemy == EnemyType.SPINNER) continue;
                    if (!enemySpawnCount.ContainsKey((int)enemy))
                    {
                        enemySpawnCount.Add((int)enemy, 0);
                    }
                    enemySpawnCount[(int)enemy]++;
                }
            }

            spawned = new EnemyObjectDictionary();

            foreach (var enemyType in _enemyTypes.Keys)
            {
                spawned.Add((int)enemyType, 0);
            }

            //VAI PRA AUDIO


            enemyAlive = new List<GameObject>();



            StartCoroutine(SpawnPowerUp());
            StartCoroutine(CheckEndGame());

        }


        // Update is called once per frame
        void Update()
        {

            if (IsFile)
            {

                if (enemyAlive.Count <= 0 && !startedSpawn)
                {
                    startedSpawn = true;
                    StartCoroutine(Spawn(2));
                }

            }
            else
            {
                foreach (var enemyType in _enemyTypes.Keys)
                {
                    if (enemySpawnCount[(int)enemyType] > 0)
                    {
                        StartCoroutine(SpawnEnemyRand(enemyType, Random.Range(2f, 4f)));

                    }
                }
            }

            if (!hasLost && Player != null && Player.GetComponent<PlayerController>().currentHealth <= 0)
            {
                StartCoroutine(DeathAnim());
                onEndLevel?.Invoke(false);
            }


        }

        IEnumerator Spawn(float delay)
        {

            yield return new WaitForSeconds(delay);

            if (!finishedSpawn)
            {
                for (int i = 0; i < _level.LevelConfig[spawnFrame].enemies.Count; i++)
                {
                    StartCoroutine(SpawnEnemyFile(i, _level.LevelConfig[spawnFrame].enemies[i].delay));
                }
                if (spawnFrame < _level.LevelConfig.Count - 1)
                {
                    spawnFrame++;
                }
                else
                {
                    //Para travar o spawn na ultima wave senao da bosta
                    finishedSpawn = true;
                }

            }

            yield break;
        }

        IEnumerator CheckEndGame()
        {

            yield return new WaitForSeconds(1);

            bool isThereEnemiesLeft = false;

            foreach (var item in enemySpawnCount.Values)
            {
                if (item > 0)
                {
                    isThereEnemiesLeft = true;
                    break;
                }

            }

            if (!isThereEnemiesLeft)
            {

                foreach (var item in enemyAlive)
                {
                    if (item)
                    {
                        isThereEnemiesLeft = true;
                        break;
                    }
                }

                foreach (var item in spawned.Values)
                {
                    if (item > 0)
                    {
                        isThereEnemiesLeft = true;
                        break;
                    }
                }


                if (!hasWon && !hasLost && !isThereEnemiesLeft)
                {
                    Score += instance.Player.GetComponent<PlayerController>().currentHealth * 100;
                    onEndLevel?.Invoke(true);
                    hasWon = true;
                    //StartCoroutine(WinAnim());
                    yield break;
                }

            }

            yield return CheckEndGame();
        }

        IEnumerator DeathAnim()
        {
            hasLost = true;
            AnimationController.instance.Explosion(Player.transform.position);

            Destroy(Player.transform.GetChild(0).gameObject);
            Destroy(Player);

            yield break;
        }

        //ANIM
        IEnumerator WinAnim()
        {

            yield return new WaitForSeconds(.5f);

            WinCanvas.SetActive(true);

            AudioController.instance.PlayGameOverAudioClip();

            hasWon = true;
            yield break;
        }

        IEnumerator SpawnEnemyRand(EnemyType enemyType, float delay)
        {
            if (!IsFile && spawned[(int)enemyType] >= maxScreenEnemies[(int)enemyType])
            {
                yield break;
            }
            int side = Random.Range(0, 4);
            float inSide = Random.Range(0.1f, 0.9f);
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

            var newEnemy = Instantiate(_enemyTypes[enemyType], spawnPos + Vector3.forward * 30, Quaternion.identity);


            var comp = (IEnemy)newEnemy.GetComponentInChildren(typeof(IEnemy));
            comp.direction = direction;
            //Debug.Log("instantiate:  " + Camera.main.WorldToViewportPoint(spawnPos) + " " + direction);

            enemySpawnCount[(int)enemyType]--;
            spawned[(int)enemyType]++;

            yield return null;

        }

        IEnumerator SpawnEnemyFile(int i, float delay)
        {

            EnemyType enemyType = _level.LevelConfig[spawnFrame].enemies[i].enemyType;

            Vector3 spawnPos = Level.CorrectViewportPosition(_level.LevelConfig[spawnFrame].enemies[i].viewportPosition);
            spawnPos = Camera.main.ViewportToWorldPoint(spawnPos);
            Vector3 direction = _level.LevelConfig[spawnFrame].enemies[i].direction;

            float speed = _level.LevelConfig[spawnFrame].enemies[i].speed;

            bool shouldMove = _level.LevelConfig[spawnFrame].enemies[i].shouldMove;

            yield return new WaitForSeconds(delay);

            var newEnemy = Instantiate(_enemyTypes[enemyType], spawnPos + Vector3.forward * 30, Quaternion.identity);

            if (enemyType == EnemyType.SPINNER)
            {
                var comp = (IObject)newEnemy.GetComponentInChildren(typeof(IObject));
                comp.direction = direction;
            }
            else
            {
                enemyAlive.Add(newEnemy);

                var comp = (IEnemy)newEnemy.GetComponentInChildren(typeof(IEnemy));

                //Spawn event
                onSpawnEnemy?.Invoke(comp);

                comp.type = enemyType;
                comp.direction = direction;
                comp.speed = speed;
                comp.ShouldMove = shouldMove;

                enemySpawnCount[(int)enemyType]--;
                spawned[(int)enemyType]++;

            }

            startedSpawn = false;


            yield return null;

        }

        IEnumerator SpawnPowerUp()
        {
            yield return new WaitForSeconds(Random.Range(25f, 30f));
            if (!hasWon)
            {
                Vector2 pos = new Vector2(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f));
                int probability = Random.Range(0, 2);
                Instantiate(powerUpPrefabs[probability], Camera.main.ViewportToWorldPoint(new Vector3(pos.x, pos.y, 0) + Vector3.forward * 30), Quaternion.identity);

                yield return SpawnPowerUp();
            }
        }

        public (Vector2, Vector2) RequestRandomPos()
        {
            int side = Random.Range(0, 4);
            float inSide = Random.Range(0.1f, 0.9f);
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




        private void OnEnable()
        {
            IEnemy.OnDestroyEvent += IEnemy_OnDestroyEvent;
            IEnemy.OnDamagedEvent += IEnemy_OnDamagedEvent;
            IEnemy.onOutOfBounds += IEnemy_onOutOfBounds;
        }

        private void IEnemy_onOutOfBounds(GameObject obj)
        {
            var enemy = obj.GetComponent<IEnemy>();

            instance.spawned[(int)enemy.type]--;
            Destroy(obj);
            enemyAlive.Remove(obj);
        }

        //Strobe material for multi health enemies
        private void IEnemy_OnDamagedEvent(GameObject obj, ProjectileController projectile)
        {
            if (projectile) Destroy(projectile.gameObject);
        }

        private void IEnemy_OnDestroyEvent(GameObject obj, ProjectileController projectile)
        {

            var enemy = obj.GetComponent<IEnemy>();
            var newExplosion = Instantiate(enemy.explosion, obj.transform.position, Quaternion.identity);
            newExplosion.transform.localScale = newExplosion.transform.localScale * 5;

            instance.spawned[(int)enemy.type]--;

            if (projectile && projectile.HPTP)
            {
                Score += Mathf.FloorToInt(enemy.baseScore * projectile.mult + Mathf.RoundToInt(projectile.angleReflected / 10) * 10);

                if (obj == projectile)
                {
                    Score += 50;
                }

            }

            if (projectile) Destroy(projectile.gameObject);

            Destroy(obj);
            enemyAlive.Remove(obj);
        }
    }

}


[System.Serializable] public class GameObjectDictionary : SerializableDictionary<EnemyType, GameObject> { }
[System.Serializable] public class EnemyObjectDictionary : SerializableDictionary<int, int> { }

