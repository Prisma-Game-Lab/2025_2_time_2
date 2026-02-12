using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartMusicPlayer : MonoBehaviour
{
    [SerializeField] private string musicName;

    private void Start()
    {
        PlayMusic();
    }

    private void PlayMusic() 
    {
        AudioManager.Instance.PlayMusic(musicName);
    }
}
