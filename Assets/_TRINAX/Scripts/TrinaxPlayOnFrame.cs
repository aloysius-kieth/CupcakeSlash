using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for executing stuff on animation frames
/// </summary>
public class TrinaxPlayOnFrame : MonoBehaviour
{
    //public void PlayConfetti()
    //{ 
    //    if (GameManager.Instance.confettiPS != null)
    //    {
    //        GameManager.Instance.confettiPS.gameObject.SetActive(true);
    //        if (!GameManager.Instance.confettiPS.isPlaying)
    //        {
    //            GameManager.Instance.confettiPS.Play();
    //            GameManager.Instance.confettiShowerPS.gameObject.SetActive(true);
    //        }
    //    }
    //}

    public void PlayBronzeMedal()
    {
        TrinaxAudioManager.Instance.PlayUISFX(TrinaxAudioManager.AUDIOS.BRONZEMEDAL, TrinaxAudioManager.AUDIOPLAYER.UI_SFX);
    }

    public void PlaySilverMedal()
    {
        TrinaxAudioManager.Instance.PlayUISFX(TrinaxAudioManager.AUDIOS.SILVERMEDAL, TrinaxAudioManager.AUDIOPLAYER.UI_SFX);
    }

    public void PlayGoldMedal()
    {
        TrinaxAudioManager.Instance.PlayUISFX(TrinaxAudioManager.AUDIOS.GOLDMEDAL, TrinaxAudioManager.AUDIOPLAYER.UI_SFX);
    }

}
