using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using System.Threading.Tasks;

using DG.Tweening;
using TMPro;

// Use this for storing user's data (ID)
[System.Serializable]
public class UserData
{
    public string playID = "";
    public string name = "";
    public string contact = "";
}

/// <summary>
/// Global Manager
/// </summary>
public class TrinaxGlobal : MonoBehaviour
{
    #region SINGLETON
    public static TrinaxGlobal Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    public GlobalSettings gSettings;
    public GameSettings gameSettings;
    public KinectSettings kinectSettings;

    public int GameScore { get; set; }

    public void RefreshSettings(GlobalSettings settings, GameSettings _gameSettings, KinectSettings _kinectSettings)
    {
        gSettings.IP = settings.IP;
        gSettings.idleInterval = settings.idleInterval;
        gSettings.useKeyboard = settings.useKeyboard;
        gSettings.useMouse = settings.useMouse;

        //TrinaxAsyncServerManager.Instance.ip = settings.IP;
        //TrinaxAsyncServerManager.Instance.useServer = settings.useServer;
        //TrinaxAsyncServerManager.Instance.useMocky = settings.useMocky;
        //TrinaxAudioManager.Instance.muteAllSounds = settings.muteAllSounds;

        gameSettings.gameDuration = _gameSettings.gameDuration;
        gameSettings.rewardAmt = _gameSettings.rewardAmt;
        gameSettings.EnableCombo = _gameSettings.EnableCombo;
        gameSettings.comboBonus = _gameSettings.comboBonus;
        gameSettings.maxForceBomb = _gameSettings.maxForceBomb;
        gameSettings.maxForceCupcake = _gameSettings.maxForceCupcake;
        gameSettings.maxSideForceBomb = _gameSettings.maxSideForceBomb;
        gameSettings.maxSideForceCupcake = _gameSettings.maxSideForceCupcake;
        gameSettings.maxSpawnInRow = _gameSettings.maxSpawnInRow;
        gameSettings.maxSpawnTime = _gameSettings.maxSpawnTime;
        gameSettings.maxToSpawn = _gameSettings.maxToSpawn;
        gameSettings.minForceBomb = _gameSettings.minForceBomb;
        gameSettings.minForceCupcake = _gameSettings.minForceCupcake;
        gameSettings.minSideForceBomb = _gameSettings.minSideForceBomb;
        gameSettings.minSideForceCupcake = _gameSettings.minSideForceCupcake;
        gameSettings.minSpawnInRow = _gameSettings.minSpawnInRow;
        gameSettings.minSpawnTime = _gameSettings.minSpawnTime;
        gameSettings.minToSpawn = _gameSettings.minToSpawn;

        gameSettings.freezeDuration = _gameSettings.freezeDuration;
        gameSettings.feverDuration = _gameSettings.feverDuration;
        gameSettings.third_tier_score = _gameSettings.third_tier_score;
        gameSettings.second_tier_score = _gameSettings.second_tier_score;
        gameSettings.first_tier_score = _gameSettings.first_tier_score;

        gameSettings.timeToToggleSpawners = _gameSettings.timeToToggleSpawners;

        //GameManager.Instance.useKeyboard = settings.useKeyboard;
        GameManager.Instance.blade.GetComponent<BladeController>().PopulateValues(gSettings);

    }

    public bool isReady = false;
    public bool loadNow = false;

    public PAGES state;

    //[Header("Arduino Ports")]
    //public string adnComPort1;
    //public string adnComPort2;

    public const string SUCCESS_FEEDBACK = "SUCCESSFUL!";
    public const string FAIL_FEEDBACK = "We're experiencing some technical difficulties." + "\n" + "Please try again";
    public const string OUT_OF_STOCK_fEEDBACK = "This activity is currently out of gifts!" + "\n" + "Sorry for the inconvenience";

    IEnumerator Start()
    {
#if !UNITY_EDITOR
    Cursor.visible = false;
#endif
        LoadManagers();
        //await new WaitUntil(() => loadNow);
        yield return new WaitUntil(() => loadNow);
    }

    #region Init Managers
    public IEnumerator Init()
    {
        if (!loadNow) yield break;

        Debug.Log("Initializing...");
        //await new WaitUntil(() => !loadNow);
        yield return new WaitUntil(() => !loadNow);
        Debug.Log("Initializing Done!");

        // *** Here all managers should be fully loaded. Do whatever you want now! *** //

    }

    // Use this to await on managers to be loaded before able to call methods from it
    IEnumerator RunLoadManagers()
    {
        Debug.Log("Waiting for managers to be loaded...");
        loadNow = true;
        yield return new WaitUntil(() => TrinaxSaveManager.Instance.isLoaded);
        yield return new WaitUntil(() => TrinaxAudioManager.Instance.isLoaded);
        yield return new WaitUntil(() => SpawnManager.Instance.isLoaded);
        //await new WaitUntil(() => TrinaxSaveManager.Instance.isLoaded);
        //await new WaitUntil(() => TrinaxAudioManager.Instance.isLoaded);
        //await new WaitUntil(() => SpawnManager.Instance.isLoaded);
        //await new WaitUntil(() => LocalLeaderboardJson.Instance.isLoaded);
        loadNow = false;

        // Indicate that everything is ready
        isReady = true;
        Debug.Log("All managers loaded!");
    }

    void LoadManagers()
    {
        StartCoroutine(RunLoadManagers());
    }
    #endregion

    private void Update()
    {
        if (Application.isPlaying)
        {
            if (Time.frameCount % 30 == 0) System.GC.Collect();
        }

    }
 
}
