using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable] public class GameObjectDictionary : SerializableDictionary<string, GameObject> { }
[System.Serializable] public class IntObjectDictionary : SerializableDictionary<string, int> { }

public class LevelController : MonoBehaviour
{

    public class SceneArgs
    {
        [System.Serializable]
        public struct EnemyConfig
        {
            public string enemyType;

            public int side;
            public float posInSide;

        }

        [System.Serializable]
        public struct EnemyConfigList
        {
            [SerializeField]
            public List<EnemyConfig> enemyPositions;
        }

        public List<EnemyConfigList> config = new List<EnemyConfigList>();





        public int[] SpawnFrames = { 0, 600 };

        public int LevelMusic = 2;

        public SceneArgs()
        {
            config.Add(new EnemyConfigList
            {
                enemyPositions = new List<EnemyConfig>
                {
                    new EnemyConfig
                    {
                        enemyType = "EnemySniper", side = 1, posInSide = 0.7f },
                    new EnemyConfig
                    {
                        enemyType = "EnemySniper", side = 3, posInSide = 0.4f }

                }
            });

            config.Add(new EnemyConfigList
            {
                enemyPositions = new List<EnemyConfig>
                {
                    new EnemyConfig
                    {
                        enemyType = "SimpleEnemy", side = 1, posInSide = 0.7f },
                    new EnemyConfig
                    {
                        enemyType = "SimpleEnemy", side = 3, posInSide = 0.4f }

                }
            });

        }

    }

    public TextAsset sceneFile;
    public bool IsFile = false;

    public SceneArgs sceneArgs;
    public int spawnFrame = 0;

    public List<AudioClip> levelClips;

    public static LevelController instance;

    public GameObject explosionAnim;
    public GameObject Player;

    public DoubleAudioSource doubleAudio;
    public AudioSource LevelAudioSource;
    public AudioClip GameOverAudioClip;

    #region CanvasRefs

    public GameObject GameOverCanvas;
    public GameObject WinCanvas;

    #endregion

    public GameObjectDictionary enemyTypes;
    // Quantity of enemies of each type to spawn during the whole level
    public IntObjectDictionary enemySpawnCount;
    // Currently alive enemies of each type
    public IntObjectDictionary spawned;
    // Max number of enemies of each type that can present on screen at one time
    public IntObjectDictionary maxScreenEnemies;

    private bool hasWon = false;
    private bool hasLost = false;


    //private int enemyTotal;

    private float frame = 0f;
    private bool finishedSpawn = false;

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
        sceneArgs = new SceneArgs();
        System.IO.File.WriteAllText(string.Format("{0}\\Levels\\BaseLevel.json", Application.dataPath), JsonUtility.ToJson(sceneArgs));

        //Debug.Log(JsonUtility.ToJson(sceneArgs));

        if (sceneFile)
        {
            IsFile = true;
            //Debug.Log(@"Levels\" + sceneFile.name);
            var jsonText = System.IO.File.ReadAllText(Application.dataPath + @"\Levels\" + sceneFile.name + ".json");
            sceneArgs = JsonUtility.FromJson<SceneArgs>(jsonText);
            if (sceneArgs.config.Count != sceneArgs.SpawnFrames.Length)
            {
                Debug.LogError("ENEMY POS LENGTH DIFFERENT FROM SCENE FRAMES LENGTH");
            }

            enemySpawnCount = new IntObjectDictionary();

            for (int i = 0; i < sceneArgs.config.Count; i++)
            {
                for (int j = 0; j < sceneArgs.config[i].enemyPositions.Count; j++)
                {
                    var enemy = sceneArgs.config[i].enemyPositions[j].enemyType;
                    if (!enemySpawnCount.ContainsKey(enemy))
                    {
                        enemySpawnCount.Add(enemy, 0);
                    }
                    enemySpawnCount[enemy]++;
                }
            }

        }

   
        spawned = new IntObjectDictionary();

        foreach (var enemyType in enemyTypes.Keys)
        {
            spawned.Add(enemyType, 0);
        }

        LevelAudioSource.clip = levelClips[sceneArgs.LevelMusic];
        LevelAudioSource.Play();
       

        //for (int i = 0; i < maxScreenEnemies["SimpleEnemy"]; i++)
        //{
        //    StartCoroutine((SpawnEnemyRand("SimpleEnemy", Random.Range(2f, 4f))));
        //}




    }


    IEnumerator SpawnEnemyRand(string enemyType, float delay)
    {
        if (!IsFile && spawned[enemyType] >= maxScreenEnemies[enemyType])
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

        var newEnemy = Instantiate(enemyTypes[enemyType], spawnPos + Vector3.forward * 30, Quaternion.identity);
        var comp = (IEnemy)newEnemy.GetComponentInChildren(typeof(IEnemy));
        comp.direction = direction;
        //Debug.Log("instantiate:  " + Camera.main.WorldToViewportPoint(spawnPos) + " " + direction);

        enemySpawnCount[enemyType]--;
        spawned[enemyType]++;

        yield return new WaitForSeconds(delay);

    }

    IEnumerator SpawnEnemyFile(int i, float delay)
    {
        string enemyType = sceneArgs.config[spawnFrame].enemyPositions[i].enemyType;
        //if (spawned[enemyType] >= maxScreenEnemies[enemyType])
        //{
        //    yield break;
        //}
        int side = sceneArgs.config[spawnFrame].enemyPositions[i].side;
        float inSide = sceneArgs.config[spawnFrame].enemyPositions[i].posInSide;
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
        //spawn Fixo
        //se sobrevi
        //bloquinhos de spawn

        var newEnemy = Instantiate(enemyTypes[enemyType], spawnPos + Vector3.forward * 30, Quaternion.identity);
        var comp = (IEnemy)newEnemy.GetComponentInChildren(typeof(IEnemy));
        comp.direction = direction;
        //Debug.Log("instantiate:  " + Camera.main.WorldToViewportPoint(spawnPos) + " " + direction);

        enemySpawnCount[enemyType]--;
        spawned[enemyType]++;

        //yield return new WaitForSeconds(delay);
        yield return null;

    }

    IEnumerator DeathAnim()
    {
        hasLost = true;
        var newExplosion = Instantiate(explosionAnim, Player.transform.position, Quaternion.identity);
        newExplosion.transform.localScale = newExplosion.transform.localScale * 5;
        Destroy(Player.transform.GetChild(0).gameObject);

        Destroy(Player);

        yield return new WaitForSeconds(.5f);

        GameOverCanvas.SetActive(true);

        doubleAudio.CrossFade(GameOverAudioClip, 0.5f, 0.5f);

        yield break;
    }


    IEnumerator WinAnim()
    {

        yield return new WaitForSeconds(.5f);

        WinCanvas.SetActive(true);

        doubleAudio.CrossFade(GameOverAudioClip, 0.5f, 0.5f);
        hasWon = true;

        yield break;
    }


    // Update is called once per frame
    void Update()
    {

        if (IsFile)
        {
            if (sceneArgs.SpawnFrames[spawnFrame] <= frame && !finishedSpawn)
            {
                for (int i = 0; i < sceneArgs.config[spawnFrame].enemyPositions.Count; i++)
                {
                    StartCoroutine(SpawnEnemyFile(i, Random.Range(2f, 4f)));
                }
                if (spawnFrame < sceneArgs.config.Count - 1)
                {
                    spawnFrame++;
                }
                else
                {
                    finishedSpawn = true;
                }

            }


        }
        else
        {
            foreach (var enemyType in enemyTypes.Keys)
            {
                if (enemySpawnCount[enemyType] > 0)
                {
                    StartCoroutine(SpawnEnemyRand(enemyType, Random.Range(2f, 4f)));

                }
            }
        }


        frame+=Time.deltaTime;


        if (Player != null && Player.GetComponent<PlayerController>().currentHealth <= 0)
        {
            StartCoroutine(DeathAnim());
        }

        //TODO: Make the win animation generic for every enemy type
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
                StartCoroutine(WinAnim());
            }
        }


        //foreach (var enemyType in enemyTypes.Keys)
        //{
        //    if (enemySpawnCount[enemyType] != 0)
        //    {
        //        return;
        //    }

        //    if (spawned[enemyType] > 0)
        //    {
        //        return;
        //    }
        //}




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
}
