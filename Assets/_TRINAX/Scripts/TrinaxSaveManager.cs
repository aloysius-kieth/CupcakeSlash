using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Threading.Tasks;

public class TrinaxSaveManager : MonoBehaviour
{
    #region SINGLETON
    public static TrinaxSaveManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else Instance = this;
    }
    #endregion

    public bool isLoaded = false;

    public TrinaxSaves saveObj;
    const string ADMINSAVEFILE = "settings.json";

    IEnumerator Start()
    {
        //await new WaitUntil(()=> TrinaxGlobal.Instance.loadNow);
        yield return new WaitUntil(() => TrinaxGlobal.Instance.loadNow);
        Debug.Log("Loading SaveManager...");
        isLoaded = false;
        Init();
        isLoaded = true;
        Debug.Log("SaveManager is loaded!");
    }

    void Init()
    {
        LoadJson();
    }

    TrinaxSaves CreateAdminSave()
    {
        GameSettings saveGameSettings = new GameSettings
        {
            gameDuration = TrinaxGlobal.Instance.gameSettings.gameDuration,
            rewardAmt = TrinaxGlobal.Instance.gameSettings.rewardAmt,
            EnableCombo = TrinaxGlobal.Instance.gameSettings.EnableCombo,
            comboBonus = TrinaxGlobal.Instance.gameSettings.comboBonus,

            minSpawnTime = TrinaxGlobal.Instance.gameSettings.minSpawnTime,
            maxSpawnTime = TrinaxGlobal.Instance.gameSettings.maxSpawnTime,

            minToSpawn = TrinaxGlobal.Instance.gameSettings.minToSpawn,
            maxToSpawn = TrinaxGlobal.Instance.gameSettings.maxToSpawn,

            minSpawnInRow = TrinaxGlobal.Instance.gameSettings.minSpawnInRow,
            maxSpawnInRow = TrinaxGlobal.Instance.gameSettings.maxSpawnInRow,

            minForceCupcake = TrinaxGlobal.Instance.gameSettings.minForceCupcake,
            maxForceCupcake = TrinaxGlobal.Instance.gameSettings.maxForceCupcake,

            minSideForceCupcake = TrinaxGlobal.Instance.gameSettings.minSideForceCupcake,
            maxSideForceCupcake = TrinaxGlobal.Instance.gameSettings.maxSideForceCupcake,

            minForceBomb = TrinaxGlobal.Instance.gameSettings.minForceBomb,
            maxForceBomb = TrinaxGlobal.Instance.gameSettings.maxForceBomb,

            minSideForceBomb = TrinaxGlobal.Instance.gameSettings.minSideForceBomb,
            maxSideForceBomb = TrinaxGlobal.Instance.gameSettings.maxSideForceBomb,

            feverDuration = TrinaxGlobal.Instance.gameSettings.feverDuration,
            freezeDuration = TrinaxGlobal.Instance.gameSettings.freezeDuration,
            third_tier_score = TrinaxGlobal.Instance.gameSettings.third_tier_score,
            second_tier_score = TrinaxGlobal.Instance.gameSettings.second_tier_score,
            first_tier_score = TrinaxGlobal.Instance.gameSettings.first_tier_score,

            timeToToggleSpawners = TrinaxGlobal.Instance.gameSettings.timeToToggleSpawners,
        };

        GlobalSettings saveGlobalSettings = new GlobalSettings
        {
            IP = TrinaxGlobal.Instance.gSettings.IP,
            photoPath = TrinaxGlobal.Instance.gSettings.photoPath,
            idleInterval = TrinaxGlobal.Instance.gSettings.idleInterval,

            COMPORT1 = TrinaxGlobal.Instance.gSettings.COMPORT1,

            //useServer = TrinaxAsyncServerManager.Instance.useServer,
            useServer = TrinaxGlobal.Instance.gSettings.useServer,
            //useMocky = TrinaxAsyncServerManager.Instance.useMocky,
            useMocky = TrinaxGlobal.Instance.gSettings.useMocky,
            useKeyboard = TrinaxGlobal.Instance.gSettings.useKeyboard,
            muteAllSounds = TrinaxAudioManager.Instance.muteAllSounds,
            useMouse = TrinaxGlobal.Instance.gSettings.useMouse,
        };

        AudioSettings saveAudioSettings = new AudioSettings
        {
            masterVolume = TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.MASTER].slider.value,
            musicVolume = TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.MUSIC].slider.value,
            SFXVolume = TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.SFX].slider.value,
            SFX2Volume = TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.SFX2].slider.value,
            SFX3Volume = TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.SFX3].slider.value,
            SFX4Volume = TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.SFX4].slider.value,
            UI_SFXVolume = TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.UI_SFX].slider.value,
            UI_SFX2Volume = TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.UI_SFX2].slider.value,
        };

        KinectSettings saveKinectSettings = new KinectSettings
        {
            isTrackingBody = TrinaxGlobal.Instance.kinectSettings.isTrackingBody,
            isTrackingHead = TrinaxGlobal.Instance.kinectSettings.isTrackingHead,
        };

        TrinaxSaves save = new TrinaxSaves
        {
            gameSettings = saveGameSettings,
            globalSettings = saveGlobalSettings,
            audioSettings = saveAudioSettings,
            kinectSettings  = saveKinectSettings,
        };    
        return save;
    }

    public void SaveJson()
    {
        saveObj = CreateAdminSave();
        Debug.Log(saveObj);

        string saveJsonString = JsonUtility.ToJson(saveObj, true);

        JsonFileUtility.WriteJsonToFile(ADMINSAVEFILE, saveJsonString, JSONSTATE.STREAMING_ASSETS);
        Debug.Log("Saving as JSON " + saveJsonString);
    }

    public void LoadJson()
    {
        string loadJsonString = JsonFileUtility.LoadJsonFromFile(ADMINSAVEFILE, JSONSTATE.STREAMING_ASSETS);
        saveObj = JsonUtility.FromJson<TrinaxSaves>(loadJsonString);

        if (saveObj != null)
        {
            // Assign our values back
            TrinaxGlobal.Instance.gSettings.IP = saveObj.globalSettings.IP;
            TrinaxGlobal.Instance.gSettings.photoPath = saveObj.globalSettings.photoPath;
            TrinaxGlobal.Instance.gSettings.idleInterval = saveObj.globalSettings.idleInterval;

            TrinaxGlobal.Instance.gSettings.COMPORT1 = saveObj.globalSettings.COMPORT1;

            TrinaxGlobal.Instance.gSettings.useServer = saveObj.globalSettings.useServer;
            TrinaxGlobal.Instance.gSettings.useMocky = saveObj.globalSettings.useMocky;
            TrinaxGlobal.Instance.gSettings.useKeyboard = saveObj.globalSettings.useKeyboard;
            TrinaxGlobal.Instance.gSettings.muteAllSounds = saveObj.globalSettings.muteAllSounds;
            TrinaxGlobal.Instance.gSettings.useMouse = saveObj.globalSettings.useMouse;

            TrinaxGlobal.Instance.gameSettings.gameDuration = saveObj.gameSettings.gameDuration;
            TrinaxGlobal.Instance.gameSettings.EnableCombo = saveObj.gameSettings.EnableCombo;
            TrinaxGlobal.Instance.gameSettings.comboBonus = saveObj.gameSettings.comboBonus;
            TrinaxGlobal.Instance.gameSettings.rewardAmt = saveObj.gameSettings.rewardAmt;
            TrinaxGlobal.Instance.gameSettings.minSpawnTime = saveObj.gameSettings.minSpawnTime;
            TrinaxGlobal.Instance.gameSettings.maxSpawnTime = saveObj.gameSettings.maxSpawnTime;
            TrinaxGlobal.Instance.gameSettings.minToSpawn = saveObj.gameSettings.minToSpawn;
            TrinaxGlobal.Instance.gameSettings.maxToSpawn = saveObj.gameSettings.maxToSpawn;
            TrinaxGlobal.Instance.gameSettings.minSpawnInRow = saveObj.gameSettings.minSpawnInRow;
            TrinaxGlobal.Instance.gameSettings.maxSpawnInRow = saveObj.gameSettings.maxSpawnInRow;
            TrinaxGlobal.Instance.gameSettings.minForceCupcake = saveObj.gameSettings.minForceCupcake;
            TrinaxGlobal.Instance.gameSettings.maxForceCupcake = saveObj.gameSettings.maxForceCupcake;
            TrinaxGlobal.Instance.gameSettings.minSideForceCupcake = saveObj.gameSettings.minSideForceCupcake;
            TrinaxGlobal.Instance.gameSettings.maxSideForceCupcake = saveObj.gameSettings.maxSideForceCupcake;
            TrinaxGlobal.Instance.gameSettings.minForceBomb = saveObj.gameSettings.minForceBomb;
            TrinaxGlobal.Instance.gameSettings.maxForceBomb = saveObj.gameSettings.maxForceBomb;
            TrinaxGlobal.Instance.gameSettings.minSideForceBomb = saveObj.gameSettings.minSideForceBomb;
            TrinaxGlobal.Instance.gameSettings.maxSideForceBomb = saveObj.gameSettings.maxSideForceBomb;

            TrinaxGlobal.Instance.gameSettings.feverDuration = saveObj.gameSettings.feverDuration;
            TrinaxGlobal.Instance.gameSettings.freezeDuration = saveObj.gameSettings.freezeDuration;
            TrinaxGlobal.Instance.gameSettings.third_tier_score = saveObj.gameSettings.third_tier_score;
            TrinaxGlobal.Instance.gameSettings.second_tier_score = saveObj.gameSettings.second_tier_score;
            TrinaxGlobal.Instance.gameSettings.first_tier_score = saveObj.gameSettings.first_tier_score;

            TrinaxGlobal.Instance.gameSettings.timeToToggleSpawners = saveObj.gameSettings.timeToToggleSpawners;

            TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.MASTER].slider.value = saveObj.audioSettings.masterVolume;
            TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.MUSIC].slider.value = saveObj.audioSettings.musicVolume;
            TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.SFX].slider.value = saveObj.audioSettings.SFXVolume;
            TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.SFX2].slider.value = saveObj.audioSettings.SFX2Volume;
            TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.SFX3].slider.value = saveObj.audioSettings.SFX3Volume;
            TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.SFX4].slider.value = saveObj.audioSettings.SFX4Volume;
            TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.UI_SFX].slider.value = saveObj.audioSettings.UI_SFXVolume;
            TrinaxAudioManager.Instance.audioSettings[(int)TrinaxAudioManager.AUDIOPLAYER.UI_SFX2].slider.value = saveObj.audioSettings.UI_SFX2Volume;

            TrinaxGlobal.Instance.kinectSettings.isTrackingBody = saveObj.kinectSettings.isTrackingBody;
            TrinaxGlobal.Instance.kinectSettings.isTrackingHead = saveObj.kinectSettings.isTrackingHead;
        }
        else
        {
            Debug.Log("Json file is empty! Creating a new save file...");
            SaveJson();
        }
    }
}
