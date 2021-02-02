using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class ComboDisplay : MonoBehaviour
{
    public TextMeshProUGUI comboScore;
    public TextMeshProUGUI comboText;
    //public TextMeshProUGUI bonusCupcakeText;
    public Image bonusImg;

    Vector2 initialPos;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void OnEnable()
    {
        comboScore.DOFade(0f, 0.000001f);
        comboText.DOFade(0f, 0.000001f);
        bonusImg.DOFade(0f, 0.000001f);
    }
    
    public void UpdateBonusScoreText()
    {
        transform.position = initialPos;
        bonusImg.transform.DOPunchScale(new Vector3(2f, 2f), 0.5f, 5, 0.1f);

        bonusImg.DOFade(1f, 1f).OnComplete(() => {
            bonusImg.DOFade(0f, 3f);
            bonusImg.transform.localScale = Vector3.one;
        });
    }

    public void UpdateCombo(int num, Vector3 pos)
    {
        Vector2 _pos = Camera.main.ScreenToWorldPoint(pos);
        transform.position = pos;
        comboScore.text = "+" + num.ToString();

        //if (num >= 2 && num <= 3)
        //{
        //    TrinaxAudioManager.Instance.PlaySFX(TrinaxAudioManager.AUDIOS.COMBO3, TrinaxAudioManager.AUDIOPLAYER.SFX4);
        //}
        //else if(num >= 4 && num <= 5)
        //{
        //    TrinaxAudioManager.Instance.PlaySFX(TrinaxAudioManager.AUDIOS.COMBO5, TrinaxAudioManager.AUDIOPLAYER.SFX4);
        //}
        //else if(num == 6)
        //{
        //    TrinaxAudioManager.Instance.PlaySFX(TrinaxAudioManager.AUDIOS.COMBO6, TrinaxAudioManager.AUDIOPLAYER.SFX4);
        //}
        //else if(num == 7)
        //{
            TrinaxAudioManager.Instance.PlaySFX(TrinaxAudioManager.AUDIOS.COMBO7, TrinaxAudioManager.AUDIOPLAYER.SFX4);
        //}

        //comboScore.transform.DOPunchScale(new Vector3(-3, -3), 0.5f);
        //comboNum.transform.DOPunchScale(new Vector3(-3, -3), 0.5f);
        //comboText.transform.DOPunchScale(new Vector3(-3, -3), 0.5f);

        transform.DOPunchScale(new Vector3(-2f, -2f), 0.5f, 5, 0.1f);

        comboScore.DOFade(1f, 1).OnComplete(()=> {
            comboScore.DOFade(0f, 1.5f);
            comboScore.transform.localScale = Vector3.one;
        });
        comboText.DOFade(1f, 1f).OnComplete(() => {
            comboText.DOFade(0f, 1.5f);
            comboText.transform.localScale = Vector3.one;
        });
    }
}

