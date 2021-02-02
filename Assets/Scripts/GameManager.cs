using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//using System.Threading.Tasks;

using TMPro;
using DG.Tweening;

public class GameManager : BaseClass
{
    #region SINGLETON
    public static GameManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            CalculateCameraBounds();
        }
    }
    #endregion
    public bool IsReady { get; set; }
    public bool IsGameOver { get; set; }
    bool firstRun = true;

    [Header("Result page")]
    public GameObject resultContent;
    public Animator medalAnim;

    [Header("Buttons")]
    public Button startBtn;
    public Button ToGameBtn;
    public Button homeBtn;

    [Header("Text")]
    public TextMeshProUGUI resultText;

    [Header("Component References")]
    public CountdownTimer countdown;
    public Timer timer;
    public GameObject blade;
    public GameObject timeupsAnim;
    public BackgroundChanger bgChanger;
    public PowerupDisplayText pDisplayText;
    public SFXMethods sfxEffects;

    [HideInInspector]
    public float horzExtent;
    [HideInInspector]
    public float vertExtent;

    float idleTimer;
    bool returningFromGame = false;

    public override IEnumerator Start()
    {
        IsReady = false;

        //await base.Start();
        yield return base.Start();
        Init();
    }

    public override void Init()
    {
        base.Init();

        IsGameOver = true;
        IsReady = true;

        pDisplayText = GetComponent<PowerupDisplayText>();
        CountdownTimer.OnCountdownFinished += OnCountdownFinished;
        blade.SetActive(false);

        InitButtonListeners();
        ToScreensaver();
    }

    public override void RefreshSettings(GlobalSettings settings, GameSettings _gameSettings, KinectSettings _kinectSettings)
    {
        base.RefreshSettings(settings, _gameSettings, _kinectSettings);

        SpawnManager.Instance.InitSpawners();
        ScoreManager.Instance.THIRD_TIER_SCORE = _gameSettings.third_tier_score;
        ScoreManager.Instance.SECOND_TIER_SCORE = _gameSettings.second_tier_score;
        ScoreManager.Instance.FIRST_TIER_SCORE = _gameSettings.first_tier_score;
        TimeManager.Instance.slowdownLength = _gameSettings.freezeDuration;
        SpawnManager.Instance.feverDuration = _gameSettings.feverDuration;
        SpawnManager.Instance.timeToToggleSpawners = _gameSettings.timeToToggleSpawners;
    }

    private void OnEnable()
    {
        BladeController.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        BladeController.OnGameOver -= OnGameOver;
    }

    void OnGameOver()
    {
        pDisplayText.DeactivateSelf();
       //RunAddInteraction().WrapErrors();
        TrinaxAudioManager.Instance.ImmediateStopMusic();
        TrinaxAudioManager.Instance.PlaySFX(TrinaxAudioManager.AUDIOS.GAME_END, TrinaxAudioManager.AUDIOPLAYER.SFX3);
        if (SpawnManager.Instance.feverMode) SpawnManager.Instance.StopFeverMode();

        timeupsAnim.SetActive(true);
        timer.timerStarted = false;
        timer.StopTimer();

        SpawnManager.Instance.ClearInPlayList();

        //returningFromGame = true;
        IsGameOver = true;

        blade.GetComponent<BladeController>().StopCutting();
        blade.SetActive(false);
        SpawnManager.Instance.bombList.Clear();

        TrinaxGlobal.Instance.GameScore = ScoreManager.Instance.Score;
        resultText.text = TrinaxGlobal.Instance.GameScore.ToString();

        //RunAddResult().WrapErrors();

        StartCoroutine(ToResults());
    }

    void CalculateCameraBounds()
    {
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;

        //Debug.Log("VertExtent: " + vertExtent + " | " + "HorzExtent: " + horzExtent);
    }

    #region APIS
    //async Task RunPopulateLeaderboard()
    //{
    //    await TrinaxAsyncServerManager.Instance.PopulateLeaderboard((bool success, LeaderboardReceiveJsonData rJson) =>
    //    {
    //        if (success)
    //        {
    //            Debug.Log("Populating leaderboard...");
    //            if (rJson.data != null)
    //            {
    //                LeaderboardManager.Instance.PopulateData(rJson);
    //                Debug.Log("Populating leaderboard success!");
    //            }
    //            else
    //            {
    //                Debug.Log("Unable to grab data!");
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("Probably no data acquired <leaderboard API>");
    //        }
    //    });
    //}

    //async Task RunRegister()
    //{
    //    RegisterSendJsonData sJson = new RegisterSendJsonData
    //    {
    //        name = TrinaxGlobal.Instance.gName,
    //        email = TrinaxGlobal.Instance.gEmail,
    //        contact = TrinaxGlobal.Instance.gMobile,
    //    };

    //    await TrinaxAsyncServerManager.Instance.Register(sJson, (bool success, RegisterReceiveJsonData rJson) =>
    //    {
    //        if (success)
    //        {
    //            Debug.Log(rJson.data);
    //            if (!string.IsNullOrEmpty(rJson.data))
    //            {
    //                TrinaxGlobal.Instance.userID = rJson.data;
    //                Debug.Log("Welcome userID: " + TrinaxGlobal.Instance.userID);
    //                //ToInstructions();
    //            }
    //            else
    //            {
    //                Debug.Log("Server gave me empty???");
    //                submitDetailsBtn.interactable = true;
    //                backDetailsBtn.interactable = true;
    //                //TrinaxAudioManager.Instance.PlayUISFX(TrinaxAudioManager.AUDIOS.FEEDBACK_ERROR, TrinaxAudioManager.AUDIOPLAYER.UI_SFX);
    //                StartCoroutine(DisplayTechnicalDifficultiesOverlay());
    //                StartCoroutine(TrinaxGlobal.Instance.ShowFeedbackMsg(feedbackText, TrinaxGlobal.FAIL_FEEDBACK, 1, 2));
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("Server fail <register API>");
    //            submitDetailsBtn.interactable = true;
    //            backDetailsBtn.interactable = true;
    //            //TrinaxAudioManager.Instance.PlayUISFX(TrinaxAudioManager.AUDIOS.FEEDBACK_ERROR, TrinaxAudioManager.AUDIOPLAYER.UI_SFX);
    //            StartCoroutine(DisplayTechnicalDifficultiesOverlay());
    //            StartCoroutine(TrinaxGlobal.Instance.ShowFeedbackMsg(feedbackText, TrinaxGlobal.FAIL_FEEDBACK, 1, 2));
    //        }
    //    });
    //}

    //IEnumerator DisplayTechnicalDifficultiesOverlay()
    //{
    //    enterDetailsOverlay.gameObject.SetActive(true);
    //    enterDetailsOverlay.DOFade(1, 0.5f);

    //    yield return new WaitForSeconds(5.0f);
    //    enterDetailsOverlay.DOFade(0.0f, 0.5f).OnComplete(() => { enterDetailsOverlay.gameObject.SetActive(false); });

    //}

    //async Task RunAddResult()
    //{
    //    AddResultSendJsonData sJson = new AddResultSendJsonData
    //    {
    //        score = TrinaxGlobal.Instance.GameScore,
    //        playID = TrinaxGlobal.Instance.userID,
    //    };

    //    await TrinaxAsyncServerManager.Instance.AddResult(sJson, (bool success, AddResultReceiveJsonData rJson) =>
    //    {
    //        if (success)
    //        {
    //            if (rJson.data)
    //            {
    //                Debug.Log("Added " + TrinaxGlobal.Instance.GameScore + " score to " + TrinaxGlobal.Instance.userID);
    //            }
    //            else
    //            {
    //                Debug.Log("Failed to add score");
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("Server fail <addScore API>");
    //        }
    //    });
    //}
    #endregion

    private void Update()
    {
        if (!IsReady) return;
        if (TrinaxGlobal.Instance.state != PAGES.SCREENSAVER
            && TrinaxGlobal.Instance.state != PAGES.GAME
            && TrinaxGlobal.Instance.state != PAGES.RESULT)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > TrinaxGlobal.Instance.gSettings.idleInterval)
            {
                idleTimer = 0;
                ToScreensaver();
            }
        }

        //if (TrinaxGlobal.Instance.gSettings.useKeyboard && !IsGameOver)
        //{
        //    if (Input.GetKeyDown(KeyCode.F1))
        //    {
        //        TimeManager.Instance.StartSlowMotion();
        //    }
        //    else if (Input.GetKeyDown(KeyCode.F2))
        //    {
        //        TimeManager.Instance.ReturnToNormalTime();
        //    }
        //}

        if (Input.anyKeyDown) idleTimer = 0;

        //if (timer.currentTime != TrinaxGlobal.Instance.gameSettings.gameDuration && timer.currentTime % 20 == 0 && !IsGameOver)
        //{
        //    //Debug.Log("Fever mode!");
        //    Brain.Instance.anim.enabled = false;
        //    feverMode.gameObject.SetActive(true);
        //}

        if (timer.currentTime <= 0 && !IsGameOver)
        {
            //BladeController.OnGameOver?.Invoke();
            if (BladeController.OnGameOver != null) BladeController.OnGameOver();
        }
    }

    void PlayBtnSound()
    {
        TrinaxAudioManager.Instance.PlayUISFX(TrinaxAudioManager.AUDIOS.BUTTON_CLICK, TrinaxAudioManager.AUDIOPLAYER.UI_SFX);
    }

    void InitButtonListeners()
    {
        startBtn.onClick.AddListener(() =>
        {
            startBtn.interactable = false;
            PlayBtnSound();
            ToInstructions();
        });

        ToGameBtn.onClick.AddListener(() =>
        {
            ToGameBtn.interactable = false;
            PlayBtnSound();
            ToGame();
        });

        homeBtn.onClick.AddListener(() =>
        {
            homeBtn.interactable = false;
            PlayBtnSound();
            ToScreensaver();
        });
    }

    void ResetGameValues()
    {
        //timer.gameObject.SetActive(false);
        medalAnim.Rebind();
        resultContent.SetActive(false);
        startBtn.interactable = true;
        ToGameBtn.interactable = true;
        homeBtn.interactable = true;

        resultText.text = ScoreManager.Instance.Score.ToString();
        ScoreManager.Instance.Score = 0;
        ScoreManager.Instance.scoreText.text = "0";
    }

    float durationToTransit = 0.25f;
    #region PAGES
    void ToScreensaver()
    {
        if(firstRun)
        {
            firstRun = false;
            TrinaxAudioManager.Instance.PlayMusic(TrinaxAudioManager.AUDIOS.IDLE, true);
        }
        //RunPopulateLeaderboard().WrapErrors();
        //feedbackText.alpha = 0;
        sfxEffects.DisableAll();
        sfxEffects.ToggleSFX(SFXEFFECTS.CONFETTI);
        ResetGameValues();

        canvasController.TransitToCanvas((int)PAGES.SCREENSAVER, durationToTransit, () => { TrinaxGlobal.Instance.state = PAGES.SCREENSAVER; });

        blade.SetActive(false);
    }

    void ToInstructions()
    {
        canvasController.TransitToCanvas((int)PAGES.INSTRUCTIONS, durationToTransit, () => { TrinaxGlobal.Instance.state = PAGES.INSTRUCTIONS; });
    }

    void OnCountdownFinished()
    {
        IsGameOver = false;

        //Brain.Instance.anim.enabled = true;
        blade.SetActive(true);
        timer.gameObject.SetActive(true);
        timer.StartTimer();

        PowerupManager.Instance.SetActive(false);
        SpawnManager.Instance.RandomToggleSpawners();
    }

    void ToGame()
    {
        timer.SetStartTime();
        bgChanger.FadeBG(BACKGROUND.NONE, 0.0001f);
        TrinaxAudioManager.Instance.TransitMusic(TrinaxAudioManager.AUDIOS.GAME, 1, 1);
        sfxEffects.DisableAll();
        timer.currentTime = TrinaxGlobal.Instance.gameSettings.gameDuration;
        canvasController.TransitToCanvas((int)PAGES.GAME, durationToTransit, () =>
        {
            TrinaxGlobal.Instance.state = PAGES.GAME;
            countdown.gameObject.SetActive(true);
            countdown.StartCount(3);
        });
    }

    IEnumerator ToResults()
    {
        ObjectPooler.Instance.ReturnAllToPool();

        yield return new WaitForSeconds(5);
        canvasController.TransitToCanvas((int)PAGES.RESULT, durationToTransit, () =>
        {
            TrinaxAudioManager.Instance.PlayMusic(TrinaxAudioManager.AUDIOS.IDLE, true);
            timeupsAnim.SetActive(false);
            sfxEffects.ToggleSFX(SFXEFFECTS.CONFETTI);
            resultContent.SetActive(true);
            ShowMedal(ScoreManager.Instance.Score);

            TrinaxGlobal.Instance.state = PAGES.RESULT;
        });
    }

    void ShowMedal(int _score)
    {
        // gold medal
        if (_score >= ScoreManager.Instance.FIRST_TIER_SCORE)
        {
            medalAnim.SetTrigger("GoldMedalEnter");
        }
        // silver medal
        else if (_score >= ScoreManager.Instance.SECOND_TIER_SCORE)
        {
            medalAnim.SetTrigger("SilverMedalEnter");
        }
        // bronze medal
        else if (_score >= ScoreManager.Instance.THIRD_TIER_SCORE)
        {
            medalAnim.SetTrigger("BronzeMedalEnter");
        }
        else
        {
            medalAnim.SetTrigger("BronzeMedalEnter");
            Debug.LogWarning("Player scored below 3rd tier! Show third tier anyway!");
        }
    }
    #endregion

    [ContextMenu("AUTOENDGAME")]
    void AutoEndGame()
    {
        OnGameOver();
    }
}
