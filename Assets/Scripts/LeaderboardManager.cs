using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    #region SINGLETON
    public static  LeaderboardManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public List<TextMeshProUGUI> pName = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> pScore = new List<TextMeshProUGUI>();

    private void Start()
    {

    }

    public void PopulateData(LeaderboardReceiveJsonData rJson)
    {
        if (rJson.data != null)
        {
            for (int i = 0; i < rJson.data.Count; i++)
            {
                pName[i].text = rJson.data[i].name;
                pScore[i].text = rJson.data[i].score.ToString();
            }
        }
        else
        {
            Debug.Log("Nothing to populate leaderboard with");
        }

    }
}
