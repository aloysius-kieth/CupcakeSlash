using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using DG.Tweening;
using TMPro;

public class Timer : MonoBehaviour
{
    //public RectTransform hand;
    public Image timerBG;
    public TextMeshProUGUI timerText;

    //float secondsToDegree; 
    public float currentTime;

    public bool timerStarted = false;

    private void Start()
    {

    }

    private void OnEnable()
    {
        //currentTime = TrinaxGlobal.Instance.gameSettings.gameDuration;
        timerText.text = currentTime.ToString() + "s";
    }

    private void OnDisable()
    {
        //alert = false;
        //StopCoroutine(PlayAlert());
    }

    public void SetStartTime()
    {
        currentTime = TrinaxGlobal.Instance.gameSettings.gameDuration;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;
        //StartCoroutine(StartTimer());
        //currentTime -= Time.deltaTime;
        //timerText.text = currentTime.ToString("0");
        //hand.localRotation = Quaternion.Euler(0f, 0f, (currentTime * secondsToDegree));

        //if (currentTime <= 5 && !alert)
        //{
        //    alert = true;
        //    StartCoroutine(PlayAlert());  
        //}
    }

    IEnumerator RunTimer()
    {
        while(timerStarted && !GameManager.Instance.IsGameOver)
        {
            currentTime--;
            timerText.text = currentTime.ToString() + "s";
            yield return new WaitForSeconds(1);
        }
    }

    public void StartTimer()
    {
        timerStarted = true;
        StartCoroutine(RunTimer());
    }

    public void StopTimer()
    {
        StopCoroutine(RunTimer());
    }

    //bool alert = false;
    //Color alertColor = new Color(1, 0.32f, 0.32f);
    //IEnumerator PlayAlert()
    //{
    //    while(alert)
    //    {
    //        TrinaxAudioManager.Instance.PlaySFX(TrinaxAudioManager.AUDIOS.ALERT, TrinaxAudioManager.AUDIOPLAYER.SFX3);
    //        hand.GetComponent<Image>().DOColor(alertColor, 0.1f);
    //        timerBG.transform.DOScale(2f, 0.1f);
    //        yield return new WaitForSeconds(.5f);
    //        hand.GetComponent<Image>().DOColor(Color.white, 0.1f);
    //        timerBG.transform.DOScale(1, 0.1f);
    //    }
    //}

}
