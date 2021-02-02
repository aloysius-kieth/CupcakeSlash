using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    #region SINGLETON
    public static ScoreManager Instance { get; set; }
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

    public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI comboCounterText;
    //public TextMeshProUGUI comboText;
    //public TextMeshProUGUI counterText;
    public int Score { get; set; }

    public int THIRD_TIER_SCORE = 100;
    public int SECOND_TIER_SCORE = 1000;
    public int FIRST_TIER_SCORE = 10000;

    public ComboDisplay comboDisplay;

    private void Start()
    {
        scoreText.text = Score.ToString();
        //comboCounterText.text = count.ToString();
        //counterText.text = "+" + totalPoints.ToString();
        //comboCounterText.DOFade(0, 0.001f);
        //comboText.DOFade(0, 0.001f);
        //counterText.DOFade(0, 0.001f);
        BladeController.OnPlayerScored += OnPlayerScored;
    }

    private void OnEnable()
    {
        BladeController.OnGameOver += OnGameOver;
        OnGameOver();
    }

    private void OnDisable()
    {
        BladeController.OnGameOver -= OnGameOver;
    }

    private void Update()
    {
        if (comboStart && !comboEnded)
        {
            comboTimer += Time.deltaTime;
            UpdateCombo();
        }
    }

    void OnGameOver()
    {
        comboStart = false;
        comboEnded = true;
        //comboCounterText.DOFade(0.0f, 0.0001f);
        //comboText.DOFade(0.0f, 0.0001f);
        //counterText.DOFade(0.0f, 0.0001f);
        count = 0;
        tempCount = 0;
        comboTimer = 0;
        //comboCounterText.text = count.ToString();
    }

    int count = 0;
    int tempCount = 0;
    int totalPoints = 0;
    bool comboStart = false;
    bool comboEnded = true;
    public void StartCombo()
    {
        if (!TrinaxGlobal.Instance.gameSettings.EnableCombo) return;

        //Debug.Log("Combo started!");
        comboTimer = 0;
        comboStart = true;
        comboEnded = false;
        count++;
        tempCount = count;
        //comboCounterText.text = count.ToString();
  
        //totalPoints = count * TrinaxGlobal.Instance.gameSettings.comboBonus;
        //counterText.text = "+" + totalPoints.ToString();

        //comboText.DOFade(1f, 0.1f);
        //counterText.DOFade(1.0f, 0.1f).OnComplete(() =>
        //{
        //    counterText.transform.DOScale(1.5f, 0.1f).OnComplete(() =>
        //    {
        //        counterText.transform.DOScale(1f, 0.1f);
        //    });
        //});
        //comboCounterText.DOFade(1.0f, 0.1f).OnComplete(() =>
        //{
        //    comboCounterText.transform.DOScale(1.5f, 0.1f).OnComplete(() =>
        //    {
        //        comboCounterText.transform.DOScale(1f, 0.1f);
        //    });
        //});

        Score += tempCount * TrinaxGlobal.Instance.gameSettings.comboBonus;
        tempCount = 0;
        scoreText.text = Score.ToString();
        //Debug.Log("Combo Count: " + count);
    }

    float comboTimer = 0;
    float comboDuration = 3f;
    void UpdateCombo()
    {
        if (comboTimer > comboDuration || GameManager.Instance.blade.GetComponent<BladeController>().hasHitBomb)
        {
            GameManager.Instance.blade.GetComponent<BladeController>().hasHitBomb = false;
            // combo has ended
            comboEnded = true;
            comboStart = false;
            //Debug.Log("Combo has ended");
            comboTimer = 0;

            //comboCounterText.DOFade(0f, 0.1f).OnComplete(()=> { count = 0; comboCounterText.text = count.ToString(); });
            //counterText.DOFade(0f, 0.1f).OnComplete(() => { totalPoints = 0; counterText.text = "+" + totalPoints.ToString(); });
            //comboText.DOFade(0f, 0.1f);
        }
    }

    public void OnPlayerScored(Cupcake cupcake)
    {
        Score += cupcake.rewardAmt;
        scoreText.text = Score.ToString();
        //Debug.Log(Score);
    }

    public void UpdateText()
    {
        scoreText.text = Score.ToString();
    }

    public void ScoreRainbow(int amt)
    {
        Score += amt;
        scoreText.text = Score.ToString();
    }

    public void MinusScore(Bomb bomb)
    {
        Score -= bomb.minusAmt;
        if (Score < 0) Score = 0;
        scoreText.text = Score.ToString();
    }

}
