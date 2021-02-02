using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    float timer;
    float timeToSpawn = 4;

    public string type = "";

    private void OnEnable()
    {
        timer = 0;
        Spawn();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver || !GameManager.Instance.IsReady) return;

        //timer += Time.deltaTime;

        //if (timer > timeToSpawn)
        //{
            //timer = 0;
            //Spawn();
        //}
    }

    void Spawn()
    {
        int index = Random.Range(0, PowerupManager.Instance.spawnPoints.Length);
        GameObject obj = ObjectPooler.Instance.GetPooledObject(type);

        obj.transform.position = new Vector3(PowerupManager.Instance.spawnPoints[index].localPosition.x, PowerupManager.Instance.spawnPoints[index].localPosition.y);
        obj.SetActive(true);

        PowerupManager.Instance.StopSpawn();
    }

}
