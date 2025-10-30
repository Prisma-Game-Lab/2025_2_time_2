using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private string menuMusic;
    [SerializeField] private string levelMusic;
    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(menuMusic);
        }
    }

   public void Play(string sceneName)
    {
        AudioManager.Instance.PlayMusic(levelMusic);
        SceneManager.LoadSceneAsync(sceneName);
    }
   public void Quit()
    {
        Application.Quit();
    }
}
