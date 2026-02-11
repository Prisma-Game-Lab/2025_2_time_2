using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private string menuMusic;
    [SerializeField] private string levelMusic;
    
    void Start()
    {
        Mixer.SetFloat("Volume", Mathf.Log10(0.25f) * 20);
        Mixer.SetFloat("SFXVolume", Mathf.Log10(0.25f) * 20);
        Mixer.SetFloat("MusicVolume", Mathf.Log10(0.25f) * 20);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(menuMusic);
        }
    }

    public void Play(string sceneName)
    {
        AudioManager.Instance.PlayMusic(levelMusic);
        LevelManager.LoadSceneByName(sceneName);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void EraseData() 
    {
        LevelManager.EraseLevelData();
    }

    public void OnButtonHover() 
    {
        AudioManager.Instance.PlaySFX("ButtonHover");
    }
}
