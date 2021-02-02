//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FeverMode : MonoBehaviour
//{
//    public bool isEnabled = false;

//    float duration = 10;

//    float timer;
//    INTERACTABLE_TYPE type;
//    public Transform[] spawnPoints;

//    private void OnEnable()
//    {
//        isEnabled = true;
//        StartFeverMode();
//    }

//    private void Update()
//    {
//        if (!isEnabled) return;

//        timer += Time.deltaTime;
//        if (timer > duration)
//        {
//            Debug.Log("fever mode over!");
//            timer = 0;
//            StopFeverMode();
//            //Brain.Instance.anim.enabled = true;
//        }  
//    }

//    public void StartFeverMode()
//    {
//        GameManager.Instance.pDisplayText.DoAnim(POWERUPS.feverbomb);
//        StartCoroutine(RunFeverMode());
//    }

//    public void StopFeverMode()
//    {
//        isEnabled = false;
//        StopCoroutine(RunFeverMode());
//        DeactivateSelf();
//    }

//    void DeactivateSelf()
//    {
//        gameObject.SetActive(false);
//    }

//    IEnumerator RunFeverMode()
//    {
//        //Brain.Instance.mode = SPAWNMODE.SPAWN_FEVER;
//        while (isEnabled)
//        {
//            //Debug.Log("RUNNING FEVER MODE");
//            int spawnPoint = Random.Range(0, 5);
//            float x = spawnPoints[spawnPoint].position.x;
//            float y = spawnPoints[spawnPoint].position.y;

//            Debug.Log(x);

//            int randType = Random.Range(0, (int)INTERACTABLE_TYPE.CUPCAKEHIGH);
//            type = (INTERACTABLE_TYPE)randType;

//            GameObject obj = ObjectPooler.Instance.GetPooledObjectByType(type.ToString());

//            //obj.GetComponent<Interactable>().spawnLeft = true ? x == -1 : false;

//            yield return new WaitForSeconds(0.5f);

//            obj.transform.position = new Vector3(x, y);
//            obj.SetActive(true);
//        }
//    }

//}
