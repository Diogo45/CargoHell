using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[Flags]
public enum AnimStates
{
    None = 0, PlayerCentering = 1, PlayerStretch = 2, PlayerGoToInfinityAndBeyond = 4, NebulaTilling = 8, NebulaSpeed = 16
}


[System.Serializable] public class GameObjectDictionary : SerializableDictionary<string, GameObject> { }
[System.Serializable] public class IntObjectDictionary : SerializableDictionary<string, int> { }

[System.Serializable] public struct float2 { public float x; public float y; }
[System.Serializable] public struct float3 { public float x; public float y; public float z; }
[System.Serializable] public struct float4 { public float x; public float y; public float z; public float w; }

public class LevelController : MonoBehaviour
{

    public delegate void OnSpawnEnemy(IEnemy controller);
    public static event OnSpawnEnemy onSpawnEnemy;

    public delegate void OnEndLevel(bool win);
    public static event OnEndLevel onEndLevel;

    public class SceneArgs
    {
        [System.Serializable]
        public struct EnemyConfig
        {
            public string enemyType;

            public int side;
            public float posInSide;
            public float delay;
        }

        [System.Serializable]
        public struct ShaderVar
        {
            public int R;
            public int G;
            public int B;

            //public float2 offset;

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
            //config.Add(new EnemyConfigList
            //{
            //    enemyPositions = new List<EnemyConfig>
            //    {
            //        new EnemyConfig
            //        {
            //            enemyType = "EnemySniper", side = 1, posInSide = 0.7f
            //        },
            //        new EnemyConfig
            //        {
            //            enemyType = "EnemySniper", side = 3, posInSide = 0.4f
            //        }

            //    }
            //});

            //config.Add(new EnemyConfigList
            //{
            //    enemyPositions = new List<EnemyConfig>
            //    {
            //        new EnemyConfig
            //        {
            //            enemyType = "SimpleEnemy", side = 1, posInSide = 0.7f },
            //        new EnemyConfig
            //        {
            //            enemyType = "SimpleEnemy", side = 3, posInSide = 0.4f }

            //    }
            //});

        }

        public ShaderVar ShaderVariables = new ShaderVar
        {
            R = 0,
            G = 0,
            B = 200
        };

    }

    public int sceneFile;
    public bool IsFile = false;

    public SceneArgs sceneArgs;
    public int spawnFrame = 0;

    public List<AudioClip> levelClips;

    public static LevelController instance;

    public GameObject explosionAnim;
    public GameObject Player;
    public GameObject Shield;

    public DoubleAudioSource doubleAudio;
    public AudioSource LevelAudioSource;
    public AudioSource AuxAudioSource;
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

    public List<GameObject> enemyAlive;

    //PowerUp List
    public List<GameObject> powerUpPrefabs;

    public bool hasWon = false;
    private bool hasLost = false;


    //private int enemyTotal;

    private float frame = 0f;
    private bool finishedSpawn = false;

    public Material nebulaMat;
    public GameObject nearStars;
    public GameObject farStars;

    public GameObject finalScoreCounter;
    private int scoreCount;

    private bool startedCoroutine;


    public int Score = 0;
    public AudioClip scoreCountAudio;
    public float MultIncrease = 1;


    public AudioMixerGroup MasterMixer;
    public AudioMixerGroup SFXMixer;

    public AnimationCurve tillingCurve;
    public AnimationCurve scrollSpeedCurve;



    public AnimStates animationState = AnimStates.PlayerCentering;

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

        Application.targetFrameRate = 60;

        sceneArgs = new SceneArgs();
#if UNITY_EDITOR
        System.IO.File.WriteAllText(string.Format("{0}\\Levels\\BaseLevel.json", Application.streamingAssetsPath), JsonUtility.ToJson(sceneArgs));
#endif
        //Debug.Log(JsonUtility.ToJson(sceneArgs));


        IsFile = true;
        //Debug.Log(@"Levels\" + sceneFile.name);
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
            var jsonText = System.IO.File.ReadAllText(Application.streamingAssetsPath + @"\Levels\Level_" + sceneFile + ".json");
            sceneArgs = JsonUtility.FromJson<SceneArgs>(jsonText);
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

        //if (sceneArgs.config.Count != sceneArgs.SpawnFrames.Length)
        //{
        //    Debug.LogError("ENEMY POS LENGTH DIFFERENT FROM SCENE FRAMES LENGTH");
        //}

        enemySpawnCount = new IntObjectDictionary();
        for (int i = 0; i < sceneArgs.config.Count; i++)
        {
            for (int j = 0; j < sceneArgs.config[i].enemyPositions.Count; j++)
            {
                var enemy = sceneArgs.config[i].enemyPositions[j].enemyType;
                if (enemy == "Spinner") continue;
                if (!enemySpawnCount.ContainsKey(enemy))
                {
                    enemySpawnCount.Add(enemy, 0);
                }
                enemySpawnCount[enemy]++;
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

        enemyAlive = new List<GameObject>();
        nebulaMat.SetColor("_Color", new Color(sceneArgs.ShaderVariables.R / 255f, sceneArgs.ShaderVariables.G / 255f, sceneArgs.ShaderVariables.B / 255f));
        nebulaMat.SetVector("_Tilling", new Vector4(1, 1, 1, 1));
        nebulaMat.SetVector("_ScrollSpeed", new Vector4(0.05f, 0.05f, 1, 1));
        StartCoroutine(SpawnPowerUp());
        StartCoroutine(CheckEndGame());



    }


    // Update is called once per frame
    void Update()
    {

        if (IsFile)
        {

            if (enemyAlive.Count <= 0 && !startedCoroutine)
            {
                startedCoroutine = true;
                StartCoroutine(Spawn(2));
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





        if (Player != null && Player.GetComponent<PlayerController>().currentHealth <= 0)
        {
            StartCoroutine(DeathAnim());
            onEndLevel?.Invoke(false);

        }

        if (hasWon)
        {

            if (animationState == AnimStates.PlayerCentering)
            {
                Player.GetComponent<PlayerController>().Movement = false;
                Shield.SetActive(false);
                var pos = Player.transform.position;
                var FinalPos = Vector3.zero;

                Player.transform.position = Vector3.Lerp(pos, FinalPos, frame);
                Player.transform.up = (FinalPos - pos).normalized;

                if (Vector3.Distance(Player.transform.position, FinalPos) < 0.1f)
                {
                    animationState = AnimStates.NebulaTilling | AnimStates.NebulaSpeed;
                    Player.transform.rotation = Quaternion.Euler(0, 0, -90);
                    nearStars.SetActive(false);
                    farStars.SetActive(false);
                    frame = 0f;
                }


            }
            else if (animationState == (AnimStates.NebulaTilling | AnimStates.NebulaSpeed))
            {

                AnimateNebula();

                if (frame > 0.06f)
                {
                    animationState = animationState | AnimStates.PlayerStretch;

                }

            }
            else if (animationState == (AnimStates.NebulaTilling | AnimStates.NebulaSpeed | AnimStates.PlayerStretch))
            {
                AnimateNebula();

                Player.GetComponent<SpriteRenderer>().material.SetColor("_Color", nebulaMat.GetColor("_Color") * 3f);

                Player.transform.localScale = Vector3.Lerp(Player.transform.localScale, new Vector3(0.5f, 7f, 1f), frame);

                Player.transform.position = Vector3.Lerp(Player.transform.position, new Vector3(15f, 0, 0), scrollSpeedCurve.Evaluate((frame - 0.06f) * 25f));

                if (Camera.main.WorldToViewportPoint(Player.transform.position).x > 1.2f)
                {
                    animationState = AnimStates.None;
                    WinCanvas.SetActive(true);
                    scoreCount = 0;
                    var score = Score.ToString().ToCharArray();

                    intCount = new int[score.Length];
                    finishedCount = new bool[score.Length];
                    StartCoroutine(countScore());



                    for (int i = 0; i < score.Length; i++)
                    {
                        StartCoroutine(CountUp(i));
                    }
                    AuxAudioSource.clip = scoreCountAudio;
                    AuxAudioSource.loop = true;
                    AuxAudioSource.Play();

                }


            }
            else if (animationState == AnimStates.PlayerGoToInfinityAndBeyond)
            {
                AuxAudioSource.Stop();
                AnimateNebula();
            }

            frame += Time.deltaTime / 50f;


        }


    }


    private void AnimateNebula()
    {
        var nebula = nebulaMat.GetVector("_Tilling");
        var nebulaScrollSpeed = nebulaMat.GetVector("_ScrollSpeed");

        var nebulaFinal = new Vector4(nebula.x, 500f, nebula.z, nebula.w);
        var nebulaFinalScrollSpeed = new Vector4(30f, 0f, nebulaScrollSpeed.z, nebulaScrollSpeed.w);

        nebulaMat.SetVector("_Tilling", Vector4.Lerp(nebula, nebulaFinal, tillingCurve.Evaluate(frame)));
        nebulaMat.SetVector("_ScrollSpeed", Vector4.Lerp(nebulaScrollSpeed, nebulaFinalScrollSpeed, scrollSpeedCurve.Evaluate(frame)));

    }

    private int[] intCount;
    private bool[] finishedCount;
    IEnumerator countScore()
    {
        AnimateNebula();

        yield return new WaitForSeconds(0.005f);

        var score = Score.ToString().ToCharArray();

        string strCount = "";

        if(!Array.Exists(finishedCount, x => x == false))
        {
            animationState = AnimStates.PlayerGoToInfinityAndBeyond;
            yield break;
        }

        for (int i = 0; i < score.Length; i++)
        {
            strCount += intCount[i].ToString();
        }

        finalScoreCounter.GetComponent<TMPro.TMP_Text>().text = "SCORE \n" + strCount;

        yield return countScore();
        yield break;
    }

    IEnumerator CountUp(int index)
    {
        var score = Score.ToString().ToCharArray();
        var counter = intCount[index];
        if (counter.ToString().ToCharArray()[0] == score[index] && ((index - 1 >= 0 && finishedCount[index - 1]) || index == 0))
        {
            finishedCount[index] = true;
            yield break;
        }

        if (counter >= 9)
        {
            counter = 0;

        }
        else
        {
            counter += 1;
        }
        intCount[index] = counter;
        yield return new WaitForSeconds(0.1f * (score.Length - (index + 1)));
        yield return CountUp(index);
        yield break;
    }


    IEnumerator Spawn(float delay)
    {


        yield return new WaitForSeconds(delay);

        if (/*sceneArgs.SpawnFrames[spawnFrame] <= frame &&*/ !finishedSpawn)
        {
            for (int i = 0; i < sceneArgs.config[spawnFrame].enemyPositions.Count; i++)
            {
                StartCoroutine(SpawnEnemyFile(i, sceneArgs.config[spawnFrame].enemyPositions[i].delay));
            }
            if (spawnFrame < sceneArgs.config.Count - 1)
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

    public void Explosion(Vector3 pos)
    {
        var newExplosion = Instantiate(explosionAnim, pos, Quaternion.identity);
        newExplosion.transform.localScale = newExplosion.transform.localScale * 5;
    }

    IEnumerator DeathAnim()
    {
        hasLost = true;
        Explosion(Player.transform.position);

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

        yield return null;

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

        yield return new WaitForSeconds(delay);


        var newEnemy = Instantiate(enemyTypes[enemyType], spawnPos + Vector3.forward * 30, Quaternion.identity);

        if (enemyType == "Spinner")
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

            switch (enemyType)
            {
                case "SimpleEnemy":
                    comp.type = EnemyType.SIMPLE;
                    break;
                case "EnemySniper":
                    comp.type = EnemyType.SNIPER;
                    break;
                case "Boss":
                    comp.type = EnemyType.BOSS;
                    break;
                case "EnemyBomber":
                    comp.type = EnemyType.BOMBER;
                    break;
                case "EnemyShooter":
                    comp.type = EnemyType.SHOOTER;
                    break;
                case "EnemyChaser":
                    comp.type = EnemyType.CHASER;
                    break;
            }
            comp.direction = direction;

            enemySpawnCount[enemyType]--;
            spawned[enemyType]++;

        }

        startedCoroutine = false;


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
    }

    //Strobe material for multi health enemies
    private void IEnemy_OnDamagedEvent(GameObject obj, ProjectileController projectile)
    {
        if (projectile) Destroy(projectile.gameObject);
    }

    private void IEnemy_OnDestroyEvent(GameObject obj, ProjectileController projectile)
    {
        //Debug.Log("MORREU " + obj.name + obj.GetComponent<IEnemy>().type);
        //TODO: UI OUCH OOF aqle get component
        var enemy = obj.GetComponent<IEnemy>();
        var newExplosion = Instantiate(enemy.explosion, obj.transform.position, Quaternion.identity);
        newExplosion.transform.localScale = newExplosion.transform.localScale * 5;

        //TEM Q RESOLVER ESSES NOMESSS
        //USAR INTS PRA BATER COM O ENUM
        switch (enemy.type)
        {
            case EnemyType.SIMPLE:
                instance.spawned["SimpleEnemy"]--;
                break;
            case EnemyType.SNIPER:
                instance.spawned["EnemySniper"]--;
                break;
            case EnemyType.BOSS:
                instance.spawned["Boss"]--;
                break;
            case EnemyType.BOMBER:
                instance.spawned["EnemyBomber"]--;
                break;
        }


        if (projectile && projectile.HPTP)
        {
            Score += Mathf.FloorToInt(enemy.baseScore * projectile.mult + Mathf.RoundToInt(projectile.angleReflected / 10) * 10);

            if (obj == projectile)
            {
                Score += 50;
            }

            //Debug.Log(String.Format("{0} {1} {2}", enemy.baseScore, projectile.mult, Mathf.FloorToInt(enemy.baseScore * projectile.mult)));
        }

        if (projectile) Destroy(projectile.gameObject);

        Destroy(obj);
        enemyAlive.Remove(obj);
    }
}
