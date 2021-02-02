//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Threading.Tasks;

//public enum BRAINSTATE
//{
//    IDLE,
//    THINK,
//    SPAWN,
//}



//public enum SPAWNMODE
//{
//    SPAWN_ALL_AT_ONCE = 0,
//    SPAWN_IN_A_ROW = 1,
//    SPAWN_FEVER = 2,
//}

//public class Brain : MonoBehaviour
//{
//    #region SINGLETON
//    public static Brain Instance { get; set; }
//    private void Awake()
//    {
//        if (Instance != null && Instance != this) Destroy(gameObject);
//        else
//        {
//            Instance = this;
//            totalNumObjects = System.Enum.GetNames(typeof(INTERACTABLE_TYPE)).Length;
//        }
//    }
//    #endregion

//    public bool isLoaded = false;
//    int maxSpawnAtOnce = 8;
//    int minSpawnAtOnce = 2;

//    int minSpawnInRow = 2;
//    int maxSpawnInRow = 5;

//    int totalNumObjects;
//    int bombCount = 0;

//    public List<INTERACTABLE_TYPE> listOfObjects = new List<INTERACTABLE_TYPE>();
//    public Transform[] spawnPoints;
//    public BRAINSTATE brainState;

//    public bool brainDoIdle;
//    public bool brainDoSpawn;

//    public bool spawnInARow;
//    public bool spawnAllAtOnce;

//    public SPAWNMODE mode;
//    public Animator anim;

//    INTERACTABLE_TYPE type;
//    float xScreenExtent;

//    float delaySpawn = 0.3f;

//    //private void OnEnable()
//    //{
//    //    maxSpawnAtOnce = TrinaxGlobal.Instance.gameSettings.maxToSpawn;
//    //    minSpawnAtOnce = TrinaxGlobal.Instance.gameSettings.minToSpawn;

//    //    minSpawnInRow = TrinaxGlobal.Instance.gameSettings.minSpawnInRow;
//    //    maxSpawnInRow = TrinaxGlobal.Instance.gameSettings.maxSpawnInRow;
//    //}

//    async void Start()
//    {
//        await new WaitUntil(() => TrinaxGlobal.Instance.loadNow);
//        Debug.Log("Loading Brain...");
//        isLoaded = false;
//        Init();
//        isLoaded = true;
//        Debug.Log("Brain is loaded!");
//    }

//    void Init()
//    {
//        anim = GetComponent<Animator>();
//        xScreenExtent = GameManager.Instance.horzExtent;
//        maxSpawnAtOnce = TrinaxGlobal.Instance.gameSettings.maxToSpawn;
//        minSpawnAtOnce = TrinaxGlobal.Instance.gameSettings.minToSpawn;

//        minSpawnInRow = TrinaxGlobal.Instance.gameSettings.minSpawnInRow;
//        maxSpawnInRow = TrinaxGlobal.Instance.gameSettings.maxSpawnInRow;
//    }

//    private void Update()
//    {
//        if (GameManager.Instance.IsGameOver) return;

//        if (brainState == BRAINSTATE.IDLE && brainDoIdle)
//        {
//            // Do Idle
//            brainDoIdle = false;
//            ClearList();
//        }

//        if (brainState == BRAINSTATE.SPAWN && brainDoSpawn)
//        {
//            brainDoSpawn = false;
//            if (Random.value > 0.5f)
//            {
//                Debug.Log("Spawn all at once!");
//                mode = SPAWNMODE.SPAWN_ALL_AT_ONCE;
//                spawnAllAtOnce = true;
//            }
//            else
//            {
//                Debug.Log("Spawn in a row!");
//                mode = SPAWNMODE.SPAWN_IN_A_ROW;
//                spawnInARow = true;
//            }
//            GameManager.Instance.modeText.text = "Mode: " + mode.ToString();
//            StartCoroutine(Spawn(mode));
//            //StartCoroutine(FeverMode());
//        }
//    }

//    void ClearList()
//    {
//        listOfObjects.Clear();
//    }

//    IEnumerator Spawn(SPAWNMODE _mode)
//    {
//        bombCount = 0;
//        List<GameObject> spawnObjs = new List<GameObject>();
//        int count = Random.Range(minSpawnInRow, maxSpawnInRow);
//        for (int i = 0; i < count; i++)
//        {
//            if (Random.value <= 0.1f && bombCount < 4)
//            {
//                bombCount++;
//                //Debug.Log("BOMBS AWAY!");
//                //if (Random.value > 0.2f)
//                //{
//                    Debug.Log("Deploying regular bomb");
//                    type = INTERACTABLE_TYPE.BOMB;
//                //}
//                //else
//                //{
//                //    //Debug.Log("Deploying freeze bomb");
//                //    type = INTERACTABLE_TYPE.FREEZEBOMB;
//                //}
//            }
//            else
//            {
//                //Debug.Log("CUPCAKES!");
//                int randType = Random.Range(0, (int)INTERACTABLE_TYPE. + 1);
//                type = (INTERACTABLE_TYPE)randType;
//            }
//            listOfObjects.Add(type);
//        }

//        for (int i = 0; i < listOfObjects.Count; i++)
//        {
//            int spawnPoint = Random.Range(0, spawnPoints.Length);
//            float randomPosX = spawnPoints[spawnPoint].localPosition.x;

//            spawnObjs.Add(ObjectPooler.Instance.GetPooledObjectByType(listOfObjects[i].ToString()));

//            //spawnObjs[i].GetComponent<Interactable>().spawnLeft = true ? Mathf.Sign(randomPosX) == -1 : false;

//            //HARDCODING LOL (dunno if can refractor when)
//            //if (spawnPoint < 6 && Mathf.Sign(randomPosX) == -1)
//            //{
//            //    spawnObjs[i].GetComponent<Interactable>().spawnLeft = true;
//            //    spawnObjs[i].GetComponent<Interactable>().spawnRight = false;
//            //    spawnObjs[i].GetComponent<Interactable>().spawnBottom = false;
//            //}

//            //if (spawnPoint < 6 && Mathf.Sign(randomPosX) == 1)
//            //{
//            //    spawnObjs[i].GetComponent<Interactable>().spawnLeft = false;
//            //    spawnObjs[i].GetComponent<Interactable>().spawnRight = true;
//            //    spawnObjs[i].GetComponent<Interactable>().spawnBottom = false;
//            //}

//            //if (spawnPoint >= 6)
//            //{
//            //    spawnObjs[i].GetComponent<Interactable>().spawnLeft = false;
//            //    spawnObjs[i].GetComponent<Interactable>().spawnRight = false;
//            //    spawnObjs[i].GetComponent<Interactable>().spawnBottom = true;
//            //}

//            if (_mode == SPAWNMODE.SPAWN_IN_A_ROW)
//            {
//                yield return new WaitForSeconds(delaySpawn);
//            }

//            spawnObjs[i].transform.position = new Vector3(spawnPoints[spawnPoint].localPosition.x, /*spawnPoints[spawnPoint].localPosition.y*/ -xScreenExtent);
//            spawnObjs[i].SetActive(true);
//        }

//        yield return null;
//    }

//    //IEnumerator FeverMode()
//    //{
//    //    while (true)
//    //    {
//    //        int spawnPoint = Random.Range(0, spawnPoints.Length);
//    //        randomPosY = spawnPoints[spawnPoint].localPosition.y;
//    //        int randType = Random.Range(0, (int)INTERACTABLE_TYPE.CUPCAKEHIGH + 1);
//    //        type = (INTERACTABLE_TYPE)randType;
//    //        GameObject obj = ObjectPooler.Instance.GetPooledObjectByType(type.ToString());
//    //        obj.GetComponent<IInteractable>().spawnLeft = true ? Mathf.Sign(spawnPoints[spawnPoint].localPosition.x) == -1 : false;

//    //        yield return new WaitForSeconds(0.5f);
//    //        obj.transform.position = new Vector3(spawnPoints[spawnPoint].localPosition.x, randomPosY);
//    //        obj.SetActive(true);
//    //    }
//    //}

//    //IEnumerator SpawnInARow()
//    //{
//    //    bombCount = 0;
//    //    List<GameObject> spawnObjs = new List<GameObject>();
//    //    int count = Random.Range(minSpawnInRow, maxSpawnInRow);
//    //    for (int i = 0; i < count; i++)
//    //    {
//    //        if (Random.value <= 0.1f && bombCount < 1)
//    //        {
//    //            bombCount++;
//    //            Debug.Log("BOMBS AWAY!");
//    //            if (Random.value > 0.5f)
//    //            {
//    //                Debug.Log("Deploying regular bomb");
//    //                type = INTERACTABLE_TYPE.BOMB;
//    //            }
//    //            else
//    //            {
//    //                Debug.Log("Deploying freeze bomb");
//    //                type = INTERACTABLE_TYPE.FREEZEBOMB;
//    //            }
//    //        }
//    //        else
//    //        {
//    //            Debug.Log("CUPCAKES!");
//    //            int randType = Random.Range(0, (int)INTERACTABLE_TYPE.CUPCAKEHIGH);
//    //            type = (INTERACTABLE_TYPE)randType;
//    //        }
//    //        listOfObjects.Add(type);
//    //    } 

//    //    for (int i = 0; i < listOfObjects.Count; i++)
//    //    {
//    //        int spawnPoint = Random.Range(0, spawnPoints.Length);
//    //        randomPosY = spawnPoints[spawnPoint].localPosition.x;
//    //        spawnObjs.Add(ObjectPooler.Instance.GetPooledObjectByType(listOfObjects[i].ToString()));
//    //        spawnObjs[i].GetComponent<IInteractable>().spawnLeft = true ? Mathf.Sign(randomPosY) == -1 : false;
//    //        yield return new WaitForSeconds(delaySpawn);
//    //        spawnObjs[i].transform.position = new Vector3(randomPosY, -yScreenExtent);
//    //        spawnObjs[i].SetActive(true);
//    //    }
//    //}

//    //void SpawnAllAtOnce()
//    //{
//    //    bombCount = 0;
//    //    List<GameObject> spawnObjs = new List<GameObject>();
//    //    int count = Random.Range(maxSpawnAtOnce, minSpawnAtOnce);
//    //    for (int i = 0; i < count; i++)
//    //    {
//    //        if (Random.value <= 0.1f && bombCount < 1)
//    //        {
//    //            bombCount++;
//    //            Debug.Log("BOMBS AWAY!");
//    //            if (Random.value > 0.5f)
//    //            {
//    //                Debug.Log("Deploying regular bomb");
//    //                type = INTERACTABLE_TYPE.BOMB;
//    //            }
//    //            else
//    //            {
//    //                Debug.Log("Deploying freeze bomb");
//    //                type = INTERACTABLE_TYPE.FREEZEBOMB;
//    //            }
//    //        }
//    //        else
//    //        {
//    //            Debug.Log("CUPCAKES!");
//    //            int randType = Random.Range(0, (int)INTERACTABLE_TYPE.CUPCAKEHIGH);
//    //            type = (INTERACTABLE_TYPE)randType;
//    //        }
//    //        listOfObjects.Add(type);
//    //    }

//    //    for (int i = 0; i < listOfObjects.Count; i++)
//    //    {
//    //        int spawnPoint = Random.Range(0, spawnPoints.Length);
//    //        randomPosY = spawnPoints[spawnPoint].localPosition.x;
//    //        spawnObjs.Add(ObjectPooler.Instance.GetPooledObjectByType(listOfObjects[i].ToString()));
//    //        spawnObjs[i].GetComponent<IInteractable>().spawnLeft = true ? Mathf.Sign(randomPosY) == -1 : false;
//    //        spawnObjs[i].transform.position = new Vector3(randomPosY, -yScreenExtent);
//    //        spawnObjs[i].SetActive(true);
//    //    }
//    //}

//}
