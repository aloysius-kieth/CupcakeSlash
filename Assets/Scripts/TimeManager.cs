using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : MonoBehaviour
{
    #region SINGLETON
    public static TimeManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }
    #endregion

    public float slowdownFactor = 0.05f;
    public int slowdownLength = 5;

    int duration = 0;
    float normalTimeFactor = 1f;

    float normalGravity = -12.81f;
    float offsetGravityFac = 6f;

    public bool isSlowMotionActive = false;
    bool slowMotionStarted = false;

    private void Start()
    {
        Physics2D.gravity = new Vector2(0, normalGravity);
    }

    private void Update()
    {
        if (isSlowMotionActive && !slowMotionStarted)
        {
            slowMotionStarted = true;
            StartCoroutine(StartSlowDuration());
        }

        if (isSlowMotionActive && duration <= 0)
        {
            StopCoroutine(StartSlowDuration());
            ReturnToNormalTime();
        }
    }

    IEnumerator StartSlowDuration()
    {
        duration = slowdownLength;

        while(isSlowMotionActive && duration > 0)
        {
            yield return new WaitForSeconds(1);
            duration--;
        }
    }

    public void StartSlowMotion()
    {
        isSlowMotionActive = true;
        PowerupManager.Instance.ResetTimer();
        Physics2D.gravity = new Vector2(0, normalGravity + offsetGravityFac);
        //Debug.Log("Slow mo gravity: " + Physics2D.gravity);

        TrinaxAudioManager.Instance.PlayUISFX(TrinaxAudioManager.AUDIOS.FREEZE, TrinaxAudioManager.AUDIOPLAYER.UI_SFX2);
        GameManager.Instance.pDisplayText.DoAnim(POWERUPS.freezebomb);

        GameManager.Instance.bgChanger.FadeBG(BACKGROUND.FREEZE, 2f);

        for (int i = 0; i < ObjectPooler.Instance.interactablesList.Count; i++)
        {
            ObjectPooler.Instance.interactablesList[i].timeScale = slowdownFactor;
        }

        GameManager.Instance.timer.timerStarted = false;
        GameManager.Instance.timer.StopTimer();
    }

    public void ReturnToNormalTime()
    {
        isSlowMotionActive = false;
        Physics2D.gravity = new Vector2(0, normalGravity);
        //Debug.Log("gravity: " + Physics2D.gravity);

        GameManager.Instance.bgChanger.FadeBG(BACKGROUND.NONE, 1f);

        for (int i = 0; i < ObjectPooler.Instance.interactablesList.Count; i++)
        {
            ObjectPooler.Instance.interactablesList[i].timeScale = normalTimeFactor;
        }

        GameManager.Instance.timer.timerStarted = true;
        GameManager.Instance.timer.StartTimer();
        slowMotionStarted = false;
    }
}
