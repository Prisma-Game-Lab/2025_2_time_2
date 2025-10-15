using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void LoadLevel(string levelName) 
    {
        LevelManager.LoadSceneByName(levelName);
    }

    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }
}
