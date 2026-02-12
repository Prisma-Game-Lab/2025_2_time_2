using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

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
            SetSubgroupVolume(subgroup.name, savedValue);
        }
    }

    public void PlayMusic(string name)
    {
        if (currentMusic == name)
            return;

        AudioClip s = Array.Find(musicSounds, x => x.name == name);
        if (s != null)
        {
            musicSource.clip = s;
            musicSource.Play();
            currentMusic = name;
        }
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
        if (Mathf.Approximately(value, 0f)) 
        {
            value = -80;
        }
        audioMixer.SetFloat(subgroupName, Mathf.Log10(value) * dbMultiplier);
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
