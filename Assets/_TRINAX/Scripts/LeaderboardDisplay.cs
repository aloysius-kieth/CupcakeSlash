using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class LeaderboardDisplay : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    public void PopulateData(LocalPlayerInfoJson info)
    {
        if (info != null)
        {
            nameText.text = info.name;
            scoreText.text = info.score.ToString().Trim();
        }
        else
        {
            PopulateDefault();
        }
    }

    public void PopulateDefault()
    {
        nameText.text = "-";
        scoreText.text = "-";
    }

    void DoHighlight()
    {
        StartCoroutine("DoBlink");
    }

    //public void StopHighlight()
    //{
    //    StopCoroutine("DoBlink");
    //}

    private void Update()
    {
        if(doBlink)
        {
            //Debug.Log("Did i come in here?");
            DoHighlight();
        }
    }

    Color lerpedColor;
    public bool doBlink;
    public IEnumerator DoBlink()
    {
        while (true)
        {
            //Debug.Log("blink blink");
            lerpedColor = Color.Lerp(Color.white, new Color(0f, 0.86f, 1f), Mathf.PingPong(Time.time * 1f, 1.0f));
            gameObject.GetComponentInChildren<Image>().color = lerpedColor;
            yield return new WaitForSeconds(1f); 
        }
    }

    public void ResetColor()
    {
        gameObject.GetComponentInChildren<Image>().color = Color.white;
    }
}
