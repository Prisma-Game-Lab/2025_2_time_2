using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public void Play(string sceneName)
    {
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
