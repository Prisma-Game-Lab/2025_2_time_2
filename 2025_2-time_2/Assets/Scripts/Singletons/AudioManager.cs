using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private SFXLibrary sfxLibrary;
    [SerializeField] private MusicLibrary musicLibrary;

    public AudioClip[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSubgroup[] audioSubgroups;

    private const float dbMultiplier = 40;
    private string currentMusic;

    [Serializable]
    class AudioSubgroup
    {
        public string name;
        public float defaultVolume;
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        foreach (var subgroup in audioSubgroups)
        {
            float savedValue = PlayerPrefs.GetFloat(subgroup.name, subgroup.defaultVolume);
            SetAudioMixer(subgroup.name, savedValue);
        }
    }

    public void PlayMusic(string name)
    {
        if (currentMusic == name)
            return;

        MusicGroup music = musicLibrary.GetMusicClip(name);

        if (music == null) return;

        musicSource.clip = music.musicClip;
        musicSource.volume = music.volume;
        musicSource.Play();
        currentMusic = music.musicName;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        AudioClip s = Array.Find(sfxSounds, x => x.name == name);
        if (s != null)
        {
            sfxSource.PlayOneShot(s);
        }
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void SetSubgroupVolume(string subgroupName, float value) 
    {
        SetAudioMixer(subgroupName, value);
        SaveAudioVolume(subgroupName, value);
    }

    private void SetAudioMixer(string subgroupName, float value) 
    {
        float transformedValue;

        if (Mathf.Approximately(value, 0f))
        {
            transformedValue = -80;
        }
        else
        {
            transformedValue = Mathf.Log10(value) * dbMultiplier;
        }

        audioMixer.SetFloat(subgroupName, transformedValue);
    }

    private void SaveAudioVolume(string subgroupName, float value) 
    {
        PlayerPrefs.SetFloat(subgroupName, value);
    }

    public bool GetSubgroupVolume(string subgroupName, out float transformedValue) 
    {
        float rawValue;

        if (!audioMixer.GetFloat(subgroupName, out rawValue)) 
        {
            transformedValue = -69;
            return false;
        }

        transformedValue = Mathf.Pow(10f, rawValue / dbMultiplier);
        return true;
    }
}
