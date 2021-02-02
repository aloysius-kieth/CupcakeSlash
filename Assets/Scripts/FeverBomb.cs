using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverBomb : Powerup
{
    public override void OnHit()
    {
        //Brain.Instance.anim.enabled = false;
        //GameManager.Instance.feverMode.gameObject.SetActive(true)
        TrinaxAudioManager.Instance.PlayUISFX(TrinaxAudioManager.AUDIOS.FEVER, TrinaxAudioManager.AUDIOPLAYER.UI_SFX);
        SpawnManager.Instance.StartFeverMode();
        base.OnHit();
    }
}
