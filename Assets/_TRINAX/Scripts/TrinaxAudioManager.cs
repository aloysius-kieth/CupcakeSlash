﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Audio;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class AudioSetting
{
    public Slider slider;
    public string exposedParam;

    public void Init()
    {
        TrinaxAudioManager.Instance.masterMixer.SetFloat(exposedParam, Mathf.Log(slider.value) * 20);
        slider.onValueChanged.AddListener(delegate { SetExposedParam(slider.value); });
    }

    public void SetExposedParam(float value)
    {
        TrinaxAudioManager.Instance.masterMixer.SetFloat(exposedParam, Mathf.Log(value) * 20);
    }
}

// Only 2D audio supported at the moment, 3D to be done in future.
public class TrinaxAudioManager : MonoBehaviour
{
    #region SINGLETON
    public static TrinaxAudioManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            //if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public bool isLoaded = false;
    [HideInInspector]
    public bool muteAllSounds;

    [Header("Dont Destroy on load")]
    // default set to true for now
    public bool dontDestroyOnLoad = true;

    [Serializable]
    public struct Sound
    {
        public AUDIOS soundName;
        public AudioClip soundClip;
    }

    [Header("Audio Settings")]
    public AudioSetting[] audioSettings;

    [Header("Audios")]
    public Sound[] sounds;

    Dictionary<AUDIOS, AudioClip> audioMap;
    AudioSource[] channels;

    int numberOfChannels;

    [Header("Audio Mixers")]
    public AudioMixer masterMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup sfx2Mixer;
    public AudioMixerGroup sfx3Mixer;
    public AudioMixerGroup sfx4Mixer;
    public AudioMixerGroup ui_sfxMixer;
    public AudioMixerGroup ui_sfx2Mixer;

    public enum AUDIOS
    {
        IDLE,
        GAME,

        BUTTON_CLICK,
        TYPING,
        COUNT3,
        COUNT2,
        COUNT1,
        GAME_END,
        BEEP,
        THANK_YOU,
        APPLAUSE,
        SWING1,
        SWING2,
        SWING3,
        CUPCAKEHIT,
        START_APP,
        TICK_TICK,
        BOMB,
        FREEZE,
        RAINBOW,
        FEVER,
        COMBO3,
        COMBO5,
        COMBO6,
        COMBO7,
        BRONZEMEDAL,
        SILVERMEDAL,
        GOLDMEDAL,
    };

    public enum AUDIOPLAYER
    {
        MUSIC,
        SFX,
        SFX2,
        SFX3,
        SFX4,
        UI_SFX,
        UI_SFX2,

        MASTER,
    }

    IEnumerator Start()
    {
        //await new WaitUntil(() => TrinaxSaveManager.Instance.isLoaded);
        yield return new WaitUntil(() => TrinaxSaveManager.Instance.isLoaded);
        Debug.Log("Loading AudioManager...");
        isLoaded = false;
        Init();
        isLoaded = true;
        Debug.Log("AudioManager is loaded!");
    }

    void Init()
    {
        audioMap = new Dictionary<AUDIOS, AudioClip>();

        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (audioMap.ContainsKey(sound.soundName)) continue;
            else audioMap.Add(sound.soundName, sound.soundClip);
        }

        int enumCount = Enum.GetValues(typeof(AUDIOPLAYER)).Length;
        numberOfChannels = enumCount;
        Debug.Log("Number of audio channels: " + numberOfChannels);

        channels = new AudioSource[numberOfChannels];

        for (int ii = 0; ii < numberOfChannels - 1; ii++)
        {
            GameObject obj = new GameObject();
            obj.name = "Channel" + ii;
            obj.transform.parent = transform;
            AudioSource aSource = obj.AddComponent<AudioSource>();
            aSource.rolloffMode = AudioRolloffMode.Linear;
            aSource.dopplerLevel = 0f;
            aSource.panStereo = 0f;
            aSource.loop = false;
            channels[ii] = obj.GetComponent<AudioSource>();
        }

        channels[(int)AUDIOPLAYER.MUSIC].outputAudioMixerGroup = musicMixer;
        channels[(int)AUDIOPLAYER.SFX].outputAudioMixerGroup = sfxMixer;
        channels[(int)AUDIOPLAYER.SFX2].outputAudioMixerGroup = sfx2Mixer;
        channels[(int)AUDIOPLAYER.SFX3].outputAudioMixerGroup = sfx3Mixer;
        channels[(int)AUDIOPLAYER.SFX4].outputAudioMixerGroup = sfx4Mixer;
        channels[(int)AUDIOPLAYER.UI_SFX].outputAudioMixerGroup = ui_sfxMixer;
        channels[(int)AUDIOPLAYER.UI_SFX2].outputAudioMixerGroup = ui_sfx2Mixer;

        // Set audioListener volume
        AudioListener.volume = muteAllSounds ? 0 : 1;

        // Init audio settings
        for (int i = 0; i < audioSettings.Length; i++)
        {
            audioSettings[i].Init();
        }
    }

    #region PLAY CHANNEL FUNCS
    /// <summary>
    /// Plays music from music channel
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="loop"></param>
    /// <param name="doFade"></param>
    /// <param name="vol"></param>
    /// <param name="pitch"></param>
    public void PlayMusic(AUDIOS audio, bool doFade, float fadeDuration = 3.0f, bool loop = true, float vol = 1f, float pitch = 1f)
    {
        AudioClip audioClip;
        if (!audioMap.ContainsKey(audio))
        {
            Debug.LogWarning(transform.gameObject.name + ": " + "Tried to play undefined sound: " + audio.ToString());
            return;
        }

        channels[(int)AUDIOPLAYER.MUSIC].pitch = pitch;
        channels[(int)AUDIOPLAYER.MUSIC].volume = vol;
        channels[(int)AUDIOPLAYER.MUSIC].loop = loop;

        if (audioMap.TryGetValue(audio, out audioClip))
        {
            channels[(int)AUDIOPLAYER.MUSIC].clip = audioClip;
            channels[(int)AUDIOPLAYER.MUSIC].Play();

            if (doFade)
            {
                channels[(int)AUDIOPLAYER.MUSIC].volume = 0f;
                channels[(int)AUDIOPLAYER.MUSIC].DOFade(1.0f, fadeDuration);
            }

            if (channels[(int)AUDIOPLAYER.MUSIC].isPlaying)
                Debug.Log("Playing: " + channels[(int)AUDIOPLAYER.MUSIC].clip.name);
        }
    }

    IEnumerator AppendMusicCallback(AudioClip audioClip, float delay = 0.0f, Action callback = null)
    {
        yield return new WaitForSeconds(audioClip.length + delay);

        if (callback != null)
            callback();
    }

    /// <summary>
    ///  Do fade transition between different music
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="durationOut"></param>
    /// <param name="durationIn"></param>
    public void TransitMusic(AUDIOS audioToTransit, float durationOut, float durationIn)
    {
        StartCoroutine(DoTransitionMusic(audioToTransit, durationOut, durationIn));
    }

    IEnumerator DoTransitionMusic(AUDIOS audioToTransit, float durationOut, float durationIn)
    {
        AudioClip audioClip;
        if (!audioMap.ContainsKey(audioToTransit))
        {
            Debug.LogWarning(transform.gameObject.name + ": " + "Tried to play undefined sound: " + audioToTransit.ToString());
            yield break;
        }

        channels[(int)AUDIOPLAYER.MUSIC].DOFade(0.0f, durationOut).OnComplete(() =>
        {
            channels[(int)AUDIOPLAYER.MUSIC].Stop();
            if (audioMap.TryGetValue(audioToTransit, out audioClip))
            {
                channels[(int)AUDIOPLAYER.MUSIC].clip = audioClip;
                channels[(int)AUDIOPLAYER.MUSIC].Play();
                channels[(int)AUDIOPLAYER.MUSIC].DOFade(1.0f, durationIn).SetEase(Ease.InQuad);
            }

        }).SetEase(Ease.OutQuad);
        yield return null;
    }

    /// <summary>
    /// Plays a SFX from SFX channel.
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="vol"></param>
    /// <param name="pitch"></param>
    public void PlaySFX(AUDIOS audio, AUDIOPLAYER channel, float vol = 1f, float pitch = 1f)
    {
        AudioClip audioClip;
        if (!audioMap.ContainsKey(audio))
        {
            Debug.LogWarning(transform.gameObject.name + ": " + "Tried to play undefined sound: " + audio.ToString());
            return;
        }

        channels[(int)channel].pitch = pitch;
        channels[(int)channel].volume = vol;

        if (audioMap.TryGetValue(audio, out audioClip))
        {
            // channels[(int)AUDIOPLAYER.SFX].clip = audioClip;
            channels[(int)channel].PlayOneShot(audioClip);
            // channels[(int)AUDIOPLAYER.SFX].Play();
            //if (channels[(int)AUDIOPLAYER.SFX].isPlaying)
            //    Debug.Log("Playing: " + channels[(int)AUDIOPLAYER.SFX].clip.name);
        }
    }

    /// <summary>
    /// Plays SFX from SFX channel with a callback appended at the end.
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="vol"></param>
    /// <param name="pitch"></param>
    /// <param name="callback"></param>
    public void PlaySFX(AUDIOS audio, AUDIOPLAYER channel, Action callback, float vol = 1f, float pitch = 1f, float delay = 0.0f)
    {
        AudioClip audioClip;
        if (!audioMap.ContainsKey(audio))
        {
            Debug.LogWarning(transform.gameObject.name + ": " + "Tried to play undefined sound: " + audio.ToString());
            return;
        }

        channels[(int)channel].pitch = pitch;
        channels[(int)channel].volume = vol;

        if (audioMap.TryGetValue(audio, out audioClip))
        {
            // channels[(int)AUDIOPLAYER.SFX].clip = audioClip;
            channels[(int)channel].PlayOneShot(audioClip);

            StartCoroutine(AppendSFXCallback(audioClip, delay, callback));

            // channels[(int)AUDIOPLAYER.SFX].Play();
            //if (channels[(int)AUDIOPLAYER.SFX].isPlaying)
            //    Debug.Log("Playing: " + channels[(int)AUDIOPLAYER.SFX].clip.name);
        }
    }

    IEnumerator AppendSFXCallback(AudioClip audioClip, float delay = 0.0f, Action callback = null)
    {
        yield return new WaitForSeconds(audioClip.length + delay);

        if (callback != null)
            callback();
    }

    /// <summary>
    /// Plays UI SFX from UI SFX channel.
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="vol"></param>
    /// <param name="pitch"></param>
    public void PlayUISFX(AUDIOS audio, AUDIOPLAYER channel, float vol = 1f, float pitch = 1f)
    {
        AudioClip audioClip;
        if (!audioMap.ContainsKey(audio))
        {
            Debug.LogWarning(transform.gameObject.name + ": " + "Tried to play undefined sound: " + audio.ToString());
            return;
        }

        channels[(int)channel].pitch = pitch;
        channels[(int)channel].volume = vol;

        if (audioMap.TryGetValue(audio, out audioClip))
        {
            // channels[(int)AUDIOPLAYER.SFX].clip = audioClip;
            channels[(int)channel].PlayOneShot(audioClip);
            // channels[(int)AUDIOPLAYER.SFX].Play();
            //if (channels[(int)AUDIOPLAYER.SFX].isPlaying)
            //    Debug.Log("Playing: " + channels[(int)AUDIOPLAYER.SFX].clip.name);
        }
    }
    #endregion

    #region STOP CHANNEL FUNCS
    /// <summary>
    /// Stops playing music channel immediately.
    /// </summary>
    public void ImmediateStopMusic()
    {
        if (channels[(int)AUDIOPLAYER.MUSIC].isPlaying)
            channels[(int)AUDIOPLAYER.MUSIC].Stop();
        else
            Debug.Log("Channel: " + channels[(int)AUDIOPLAYER.MUSIC] + " has already stopped playing!");
    }

    /// <summary>
    /// Stops playing music channel with a fade.
    /// </summary>
    /// <param name="duration"></param>
    public void StopMusic(float duration)
    {
        if (channels[(int)AUDIOPLAYER.MUSIC].isPlaying)
            channels[(int)AUDIOPLAYER.MUSIC].DOFade(0.0f, duration).SetEase(Ease.OutQuad);
        else
            Debug.Log("Channel: " + channels[(int)AUDIOPLAYER.MUSIC] + " has already stopped playing!");
    }

    /// <summary>
    /// Stops playing SFX channel immediately.
    /// </summary>
    public void ImmediateStopSFX()
    {
        channels[(int)AUDIOPLAYER.SFX].Stop();
    }

    /// <summary>
    /// Stops playing SFX channel with a fade.
    /// </summary>
    /// <param name="duration"></param>
    public void StopSFX(float duration)
    {
        channels[(int)AUDIOPLAYER.SFX].DOFade(0.0f, duration).SetEase(Ease.OutQuad);
    }

    /// <summary>
    /// Stops playing UISFX channel immediately.
    /// </summary>
    public void ImmediateStopUISFX()
    {
        channels[(int)AUDIOPLAYER.UI_SFX].Stop();
    }

    /// <summary>
    /// Stops playing UISFX channel with a fade.
    /// </summary>
    /// <param name="duration"></param>
    public void StopUISFX(float duration)
    {
        channels[(int)AUDIOPLAYER.UI_SFX].DOFade(0.0f, duration).SetEase(Ease.OutQuad);
    }
    #endregion

    #region PAUSE CHANNEL FUNCS
    /// <summary>
    /// Pauses music channel.
    /// </summary>
    public void PauseMusic()
    {
        channels[(int)AUDIOPLAYER.MUSIC].Pause();
    }

    /// <summary>
    /// Unpauses music channel.
    /// </summary>
    public void UnPauseMusic()
    {
        channels[(int)AUDIOPLAYER.MUSIC].UnPause();
    }

    /// <summary>
    /// Pauses SFX channel.
    /// </summary>
    public void PauseSFX()
    {
        channels[(int)AUDIOPLAYER.SFX].Pause();
    }

    /// <summary>
    /// Unpauses SFX channel.
    /// </summary>
    public void UnPauseSFX()
    {
        channels[(int)AUDIOPLAYER.SFX].UnPause();
    }

    /// <summary>
    /// Pauses UISFX channel.
    /// </summary>
    public void PauseUISFX()
    {
        channels[(int)AUDIOPLAYER.UI_SFX].Pause();
    }

    /// <summary>
    /// Unpauses UISFX channel.
    /// </summary>
    public void UnPauseUISFX()
    {
        channels[(int)AUDIOPLAYER.UI_SFX].UnPause();
    }
    #endregion

    /// <summary>
    /// Mutes audiolistener
    /// </summary>
    /// <param name="_mute"></param>
    public void MuteAllSounds(bool _mute)
    {
        AudioListener.volume = _mute ? 0 : 1;
    }

    public void RestoreIdleBGM()
    {
        PlayMusic(AUDIOS.IDLE, true);
    }

    //public void SetMusicVolume(float value)
    //{
    //    audioSettings[(int)AUDIOPLAYER.MUSIC].SetExposedParam(value);
    //}

    //public void SetSFXVolume(float value)
    //{
    //    audioSettings[(int)AUDIOPLAYER.SFX].SetExposedParam(value);
    //}


}
