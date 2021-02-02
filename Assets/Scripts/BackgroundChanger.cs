using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZCameraShake;

public enum BACKGROUND
{
    NONE,
    FREEZE,
    FEVER,
}

public class BackgroundChanger : MonoBehaviour
{
    public CanvasGroup[] backgrounds;

    public void DoShake()
    {
        CameraShaker.Instance.ShakeOnce(4f, 2f, 0.1f, 0.8f);
    }

    public void FadeBG(BACKGROUND power, float duration)
    {
        int index = (int)power;
        for (int i = 0; i < backgrounds.Length; i++)
        {
            CanvasGroup canvasGrp = backgrounds[i];
            if (i == index)
            {
                canvasGrp.DOFade(1f, duration);
            }
            else
            {
                canvasGrp.DOFade(0f, duration);
            }
        }
    }
}
