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

    public static void LoadSceneByName(string sceneName, bool fade = true) 
    {
        if (fade)
            Instance.StartCoroutine(Instance.ActivateSceneTransition(sceneName));
        else
            Instance.LoadSceneImmediatly(sceneName);
    }

    private void LoadSceneImmediatly(string sceneName) 
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            RestartLevel();
            return;
        }

        OnSceneChanged.Invoke();
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator ActivateSceneTransition(string sceneName) 
    {
        yield return new WaitForSeconds(SceneController.instance.TriggerLevelTransition());
        LoadSceneImmediatly(sceneName);
    }
}