using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    float timer;
    float timeToSpawn;

    public float minSpawnTime;
    public float maxSpawnTime;

    public bool isActivated = false;

    public SPAWN_DIRECTION spawnDir;
    INTERACTABLE_TYPE typeToSpawn;

    Spawner spawner;
    private void Start()
    {
        spawner = GetComponent<Spawner>();
    }

    private void OnEnable()
    {
        isActivated = true;
    }

    private void OnDisable()
    {
        isActivated = false;
    }

    public void Init()
    {
        minSpawnTime = SpawnManager.Instance.minSpawnTime;
        maxSpawnTime = SpawnManager.Instance.maxSpawnTime;
    }

    public void ToggleEnabled(bool _enabled)
    {
        spawner.enabled = _enabled;
    }

    public void SetTimeToSpawn(float _min, float _max)
    {
        timeToSpawn = Random.Range(_min, _max);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver || !GameManager.Instance.IsReady) return;

        if(isActivated)
        {
            if (SpawnManager.Instance.feverMode)
            {
                timer += Time.deltaTime;
                if (timer >= timeToSpawn)
                {
                    timer = 0;
                    StartCoroutine(Spawn());
                    SetTimeToSpawn(minSpawnTime, maxSpawnTime);
                }
            }
            else StartCoroutine(Spawn());
        }
    }

    INTERACTABLE_TYPE GetRandomType(int start, int end)
    {
        int index = Random.Range(start, end);
        typeToSpawn = (INTERACTABLE_TYPE)index;

        return typeToSpawn;
    }

    IEnumerator Spawn()
    {
        string type = "";
        if (SpawnManager.Instance.feverMode)
        {
            GetRandomType(0, SpawnManager.Instance.GetInteractableTypesLength() - 1);
        }
        else
        {
            GetRandomType(0, SpawnManager.Instance.GetInteractableTypesLength());
            if (typeToSpawn == INTERACTABLE_TYPE.BOMB && SpawnManager.Instance.bombList.Count < SpawnManager.MAX_BOMB)
            {
                SpawnManager.Instance.bombList.Add(typeToSpawn);
            }
            else
            {
                // last index is bomb type in inspector
                GetRandomType(0, SpawnManager.Instance.GetInteractableTypesLength() - 1);
            }
        }

        if (typeToSpawn == INTERACTABLE_TYPE.CUPCAKERED 
            || typeToSpawn == INTERACTABLE_TYPE.CUPCAKEGREEN
            || typeToSpawn ==  INTERACTABLE_TYPE.CUPCAKEORANGE
            || typeToSpawn == INTERACTABLE_TYPE.CUPCAKEPURPLE)
        {
            type = "Cupcake";
        }
        else
        {
            type = "Bomb";
        }
        //Debug.Log("spawning " + typeToSpawn.ToString());

        GameObject obj = ObjectPooler.Instance.GetPooledObject(type);

        if (obj != null)
        {
            obj.GetComponent<Interactable>().spawnDir = this.spawnDir;
            obj.transform.position = transform.position;
            obj.SetActive(true);

            SpawnManager.Instance.inPlayList.Add(obj);
            if (/*!SpawnManager.Instance.feverMode && */obj.GetComponent<Interactable>()._type != INTERACTABLE_TYPE.BOMB)
            {
                SpawnManager.Instance.cupcakeList.Add(obj);
            }
           // Debug.Log("spawned " + obj.name);
        }

        // turn off this spawner
        if (!SpawnManager.Instance.feverMode) spawner.enabled = false;
        yield return null;
    }

}
