using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class DeactivateDisplayText : MonoBehaviour
{
    Image img;
    public EffectsStr effectStr;

    private void Start()
    {
        img = GetComponent<Image>();
        effectStr = GetComponent<EffectsStr>();
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateSelf());
    }

    private void OnDisable()
    {
        StopCoroutine(DeactivateSelf());
    }

    IEnumerator DeactivateSelf()
    {
        if (effectStr.type == POWERUPS.feverbomb)
        {
            yield return new WaitForSeconds(SpawnManager.Instance.feverDuration);
        }
        else if(effectStr.type == POWERUPS.freezebomb)
        {
            yield return new WaitForSeconds(TimeManager.Instance.slowdownLength);
        }
        else
        {
            yield return new WaitForSeconds(2);
        }
        
        img.DOFade(0, 1f).OnComplete(() =>
        { gameObject.SetActive(false); });
    }

}
