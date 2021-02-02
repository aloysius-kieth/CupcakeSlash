using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PowerupDisplayText : MonoBehaviour
{
    public Transform[] effects;

    private void Start()
    {
        //DeactivateSelf();
    }

    public void DeactivateSelf()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].gameObject.SetActive(false);
        }
    }

    public void DoAnim(string power)
    {
        //int index = 0;
        for (int i = 0; i < effects.Length; i++)
        {
            Transform t = effects[i];
            if (power == effects[i].GetComponent<EffectsStr>().type)
            {
                t.gameObject.SetActive(true);
                t.GetComponent<Image>().DOFade(1, 0.25f).OnComplete(() => { t.DOLocalJump(t.localPosition, 100, 1, 0.4f); });
            }
            else
            {
                t.GetComponent<Image>().DOFade(0, 0.00001f).OnComplete(() =>
                { t.gameObject.SetActive(false); });
            }
        }
        //effects[(int)power].DOLocalJump(effects[(int)power].transform.localPosition, 100f, 1, 0.4f);
    }

}
