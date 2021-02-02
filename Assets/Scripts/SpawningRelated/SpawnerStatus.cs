//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SpawnerStatus : MonoBehaviour
//{
//    private void Awake()
//    {
//        spawner = GetComponent<Spawner>();
//    }

//    //float timer;
//    //float timeToChange;

//    //float minTime = 5f;
//    //float maxTime = 10f;

//    Spawner spawner;
//    //bool firstRun = true;

//    private void Start()
//    {
//        //spawner.SetTimeToSpawn(spawner.minSpawnTime, spawner.maxSpawnTime);
//    }

//    private void OnEnable()
//    {
//        //if (firstRun)
//        //{
//        //    StatusUpdate();
//        //    firstRun = false;
//        //}
//        //else SetTimeToChange();

//    }

//    private void Update()
//    {
//        if (GameManager.Instance.IsGameOver || !GameManager.Instance.IsReady) return;

//        if (SpawnManager.Instance.feverMode) return;

//        //timer += Time.deltaTime;

//        //if (timer >= timeToChange)
//        //{
//        //    ResetTimer();
//        //    SetTimeToChange();
//        //    StatusUpdate();
//        //}
//    }

//    //public void SetTimeToChange()
//    //{
//    //    timeToChange = Random.Range(minTime, maxTime);
//    //}

//    //public void ResetTimer()
//    //{
//    //    timer = 0;
//    //}

//    //public void StatusUpdate()
//    //{      
//    //    if (Random.value > 0.5f)
//    //    {
//    //        ActivateSelf();
//    //    }
//    //    else
//    //    {
//    //        DeactivateSelf();
//    //    }
//    //}

//    public  void ActivateSelf()
//    {
//        //Debug.Log("ACTIVATE");
//        spawner.enabled = true;
//    }

//    public void DeactivateSelf()
//    {
//        //Debug.Log("DEACTIVATE");
//        spawner.enabled = false;
//    }
//}
