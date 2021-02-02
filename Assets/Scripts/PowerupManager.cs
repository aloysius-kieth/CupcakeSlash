using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class POWERUPS
{
    public static string freezebomb = "FreezeBomb";
    public static string feverbomb = "FeverBomb";
    public static string cupcakeRainbow = "CupcakeRainbow";
}

public class PowerupManager : MonoBehaviour
{
    #region SINGLETON
    public static PowerupManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);   
        else Instance = this;     
    }
    #endregion

    string type;
    bool isActive = true;
    float timer = 0;
    float timeToActivate = 5f;

    float minTime = 5f;
    float maxTime = 10f;

    public Transform[] spawnPoints;
    public PowerupSpawner[] spawners;
    //public POWERUPS[] powerUps;
    public string[] powerUps;

    string powerUp;

    private void Start()
    {
        SetTime();
    }

    void SetTime()
    {
        timeToActivate = Random.Range(minTime, maxTime);
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    public void SetActive(bool _enabled)
    {
        isActive = _enabled;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver || !GameManager.Instance.IsReady) return;
        if (SpawnManager.Instance.feverMode || TimeManager.Instance.isSlowMotionActive) return;

        if (!isActive)
        {
            timer += Time.deltaTime;
            if (timer > timeToActivate)
            {
                timer = 0;
                SetTime();
                StartSpawn();
            }
        }
    }

    public void StopSpawn()
    {
        //Debug.Log("Deactivating all spawners");

        foreach (PowerupSpawner spawner in spawners)
        {
            spawner.gameObject.SetActive(false);
        }

        isActive = false;
    }

    int index;
    public void StartSpawn()
    {
        isActive = true;
        //Debug.Log("Selecting what to spawn..");

         index = Random.Range(0, powerUps.Length);

        type = powerUps[index];

        //Debug.Log("Going to spawn " + type);
        SetSpawner(type);
    }

    void SetSpawner(string powerup)
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            if (powerup == spawners[i].type)
            {
                spawners[i].gameObject.SetActive(true);
                //GameManager.Instance.modeText.text = "Mode: " + spawners[i].powerup.ToString();
            }
            else
            {
                spawners[i].gameObject.SetActive(false);
            }
        }
    }
}
