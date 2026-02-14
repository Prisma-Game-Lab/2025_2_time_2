using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private SFXLibrary sfxLibrary;
    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Variables")]
    [SerializeField] private AudioSubgroup[] audioSubgroups;
    [SerializeField] private GameObject sfxSourceObj;

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
        float volume = 0;
        float pitch = 0;

        AudioClip clip = sfxLibrary.GetClipRandomVariation(name, ref volume, ref pitch);
        if (clip == null) return;

        GameObject SourceObj = Instantiate(sfxSourceObj, Vector3.zero, Quaternion.identity, transform);
        AudioSource audioSource = SourceObj.GetComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = 1 + pitch;

        audioSource.Play();

        //Destroy(audioSource, clip.length + 0.5f);
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
