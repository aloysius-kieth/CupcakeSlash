using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrinaxSaves
{
    public GlobalSettings globalSettings;
    public GameSettings gameSettings;
    public AudioSettings audioSettings;
    public KinectSettings kinectSettings;
}

[System.Serializable]
public struct GlobalSettings
{
    public string IP;
    public string photoPath;
    public float idleInterval;

    public string COMPORT1;

    public bool useServer;
    public bool useMocky;
    public bool useKeyboard;
    public bool muteAllSounds;
    public bool useMouse;
}

[System.Serializable]
public struct AudioSettings
{
    public float masterVolume;
    public float musicVolume;
    public float SFXVolume;
    public float SFX2Volume;
    public float SFX3Volume;
    public float SFX4Volume;
    public float UI_SFXVolume;
    public float UI_SFX2Volume;
}

[System.Serializable]
public struct GameSettings
{
    public int gameDuration;
    public int rewardAmt;
    //public float timeToLoop;

    public bool EnableCombo;

    public int comboBonus;

    public float minSpawnTime;
    public float maxSpawnTime;
    public int minToSpawn;
    public int maxToSpawn;

    public int minSpawnInRow;
    public int maxSpawnInRow;

    public float minForceCupcake;
    public float maxForceCupcake;

    public float minSideForceCupcake;
    public float maxSideForceCupcake;

    public float minForceBomb;
    public float maxForceBomb;

    public float minSideForceBomb;
    public float maxSideForceBomb;

    public float feverDuration;
    public int freezeDuration;
    public int third_tier_score;
    public int second_tier_score;
    public int first_tier_score;

    public float timeToToggleSpawners;
}

[System.Serializable]
public struct KinectSettings
{
    public bool isTrackingBody;
    public bool isTrackingHead;
}

#region ADD INTERACTION
public struct AddinteractionSendData
{
    public string application;
}

public struct AddinteractionReceiveData
{
    //public requestClass request;
    //public errorClass error;
    public string api;
    public string errorCode;
    public string errorMessage;
    public bool data;
}
#endregion

#region REGISTER SEND
[System.Serializable]
public struct RegisterSendJsonData
{
    public string playID;
    public string name;
    public string contact;
}

[System.Serializable]
public struct RegisterReceiveJsonData
{
    public requestClass request;
    public errorClass error;
    public bool data;
}
#endregion

#region ADD RESULT
[System.Serializable]
public struct AddResultSendJsonData
{
    public int score;
    public string playID;
}

[System.Serializable]
public struct AddResultReceiveJsonData
{
    public requestClass request;
    public errorClass error;
    public string data;
}
#endregion

#region LEADERBOARD
[System.Serializable]
public struct LeaderboardReceiveJsonData
{
    public requestClass request;
    public errorClass error;
    public List<LeaderboardData> data;
}

[System.Serializable]
public struct LeaderboardData
{
    public string name;
    public int score;
}
#endregion

[System.Serializable]
public struct requestClass
{
    public string api;
    public string result;
}

[System.Serializable]
public struct errorClass
{
    public string errorCode;
    public string errorMessage;
}

/// <summary>
/// For using playerPrefs with.
/// </summary>
public interface ITrinaxPersistantVars
{
    string ip { get; set; }
    string photoPath { get; set; }
    bool useServer { get; set; }
}

