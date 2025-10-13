using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public static UnityEvent OnSceneChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
        }
        else 
        {
            Instance = this;
            OnSceneChanged = new UnityEvent();
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void RestartLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadSceneByName(string sceneName) 
    {
        if (SceneManager.GetActiveScene().name == sceneName) 
        {
            RestartLevel();
            return;
        }
        
        OnSceneChanged.Invoke();
        SceneManager.LoadScene(sceneName);
    }
}