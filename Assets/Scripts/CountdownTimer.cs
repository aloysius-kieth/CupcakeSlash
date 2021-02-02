using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public static System.Action OnCountdownFinished;

    int count;
    Animator anim;

    public TextMeshProUGUI numText;
    public Sprite[] sprites;
    public Sprite goSprite;
    public Image targetimage;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        //targetimage.sprite = sprites[2];
       
    }

    public void StartCount(int num)
    {
        count = num + 1;
        Invoke("Countdown", 1f);
        numText.text = num.ToString();
    }

    public void Countdown()
    {
        --count;
        numText.text = count.ToString();
        //if ((count - 1) < sprites.Length && (count - 1) >= 0)
        //{
        //    targetimage.sprite = sprites[count - 1];
        //}

    }

    public void PlayTick()
    {
        TrinaxAudioManager.Instance.PlaySFX(TrinaxAudioManager.AUDIOS.TICK_TICK, TrinaxAudioManager.AUDIOPLAYER.SFX);
    }

    public void CheckState()
    {

        if (anim != null) anim.SetTrigger(count == 1 ? "Exit" : "Pulse");
        else anim = GetComponent<Animator>();
    }

    public void CountdownFinished()
    {
        //targetimage.sprite = goSprite;
        if (OnCountdownFinished != null) OnCountdownFinished();
    }

    public void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }

}
