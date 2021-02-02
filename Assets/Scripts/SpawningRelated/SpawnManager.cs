using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//using System.Threading.Tasks;

using Random = System.Random;

public enum INTERACTABLE_TYPE
{
    CUPCAKERED,
    CUPCAKEPURPLE,
    CUPCAKEORANGE,
    CUPCAKEGREEN,
    BOMB,
}

public enum SPAWN_DIRECTION
{
    LEFT,
    RIGHT,
    BOTTOM_LEFT,
    BOTTOM_CENTER,
    BOTTOM_RIGHT,
}

public class SpawnManager : MonoBehaviour
{
    #region SINGLETON
    public static SpawnManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    #endregion
    public bool isLoaded = false;
    public bool feverMode = false;

    public GameObject spawnPointParent;

    [Header("Cupcake Sprites")]
    public Sprite[] cupcakeSprites;

    [SerializeField]
    List<Spawner> spawners;
    public List<INTERACTABLE_TYPE> bombList;
    public List<GameObject> cupcakeList;
    public List<GameObject> inPlayList;

    public INTERACTABLE_TYPE[] interactableTypes;

    // Used for spawners
    public float minSpawn;
    public float maxSpawn;

    // Used for spawners
    public float minSpawnTime;
    public float maxSpawnTime;

    public const int MAX_BOMB = 3;

    float timer;
    public float feverDuration = 5f;

    IEnumerator Start()
    {
        //await new WaitUntil(() => TrinaxSaveManager.Instance.isLoaded);
        yield return new WaitUntil(() => TrinaxSaveManager.Instance.isLoaded);
        Debug.Log("Loading SpawnManager");
        isLoaded = false;
        Init();
        isLoaded = true;
        Debug.Log("SpawnManager is loaded!");
    }

    void Init()
    {
        spawners = new List<Spawner>();
        inPlayList = new List<GameObject>();
        cupcakeList = new List<GameObject>();
        bombList = new List<INTERACTABLE_TYPE>();
        foreach (Spawner child in spawnPointParent.GetComponentsInChildren<Spawner>())
        {
            spawners.Add(child);
        }

        minSpawn = TrinaxGlobal.Instance.gameSettings.minToSpawn;
        maxSpawn = TrinaxGlobal.Instance.gameSettings.maxToSpawn;

        SetTimeToSpawn(TrinaxGlobal.Instance.gameSettings.minSpawnTime, TrinaxGlobal.Instance.gameSettings.maxSpawnTime);
        InitSpawners();
        ToggleAllSpawners(false);
    }

    // Get value from save settings
    void SetTimeToSpawn(float _min, float _max)
    {
        minSpawnTime = _min;
        maxSpawnTime = _max;
    }

    public void InitSpawners()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.Init();
            spawner.SetTimeToSpawn(minSpawnTime, minSpawnTime);
        }
    }

    float toggleSpawnerTimer = 0;
    public float timeToToggleSpawners = 0.1f;
    private void Update()
    {
        if (GameManager.Instance.IsGameOver || !GameManager.Instance.IsReady) return;

        if (CheckIfPlayListEmpty() && !feverMode)
        {
            toggleSpawnerTimer += Time.deltaTime;
            if (toggleSpawnerTimer > timeToToggleSpawners)
            {
                RandomToggleSpawners();
            }
        }

        if (feverMode)
        {
            timer += Time.deltaTime;
            if (timer >= feverDuration)
            {
                timer = 0;
                StopFeverMode();
            }
        }
    }

    public bool CheckIfPlayListEmpty()
    {
        if (inPlayList.Count <= 2) return true;
        else
        {
            return false;
        }
    }

    public void ClearInPlayList()
    {
        inPlayList.Clear();
    }

    //@TODO put this min and max value into adminpanel
    const int MIN_SPAWNERS = 3;
    const int MAX_SPAWNERS = 7;
    public List<int> randomNumbers = new List<int>();
    public void RandomToggleSpawners()
    {
        toggleSpawnerTimer = 0;
        if (randomNumbers.Count != 0) randomNumbers.Clear();
        if (cupcakeList.Count != 0) cupcakeList.Clear();
        int index = UnityEngine.Random.Range(MIN_SPAWNERS, MAX_SPAWNERS);

        Random rnd = new Random();
        randomNumbers = Enumerable.Range(0, spawners.Count).OrderBy(x => rnd.Next()).Take(index).ToList();
        randomNumbers.Sort();
        //Debug.Log("Spawning " + randomNumbers.Count);

        for (int i = 0; i < randomNumbers.Count; i++)
        {
            spawners[randomNumbers[i]].enabled = true;
            //Debug.Log("random number: " + randomNumbers[i]);
        }
    }

    void ToggleAllSpawners(bool _active)
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            if (_active)
                spawners[i].ToggleEnabled(true);
            else spawners[i].ToggleEnabled(false);
        }
    }

    public void StartFeverMode()
    {
        Debug.Log("Fever Mode!!");
        cupcakeList.Clear();
        feverMode = true;
        SetTimeToSpawn(1, 1);
        InitSpawners();
        PowerupManager.Instance.ResetTimer();
        GameManager.Instance.sfxEffects.EnableAll();
        GameManager.Instance.pDisplayText.DoAnim(POWERUPS.feverbomb);
        GameManager.Instance.bgChanger.FadeBG(BACKGROUND.FEVER, 2f);

        foreach (Spawner spawner in spawners)
        {
            spawner.enabled = true;
        }
    }

    public void StopFeverMode()
    {
        Debug.Log("Stopping fever mode!");
        feverMode = false;
        SetTimeToSpawn(TrinaxGlobal.Instance.gameSettings.minSpawnTime, TrinaxGlobal.Instance.gameSettings.maxSpawnTime);
        InitSpawners();
        GameManager.Instance.sfxEffects.DisableAll();
        GameManager.Instance.bgChanger.FadeBG(BACKGROUND.NONE, 1f);

        //RandomToggleSpawners();
    }

    public int GetInteractableTypesLength()
    {
        return interactableTypes.Length;
    }
}
