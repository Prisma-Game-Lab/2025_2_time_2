using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public static UnityEvent OnSceneChanged = new UnityEvent();

    public static int unlockedLevels { get; private set; }
    private static int reachedBuildIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        unlockedLevels = PlayerPrefs.GetInt("unlockedLevels", 0);
        reachedBuildIndex = PlayerPrefs.GetInt("reachedBuildIndex", 0);
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
            LoadSceneImmediatly(sceneName);
    }

    public static void UnlockNextLevel() 
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex > reachedBuildIndex) 
        {
            reachedBuildIndex = currentScene.buildIndex;
            unlockedLevels++;

            SaveLevelData();
        }

        print(unlockedLevels.ToString() + " " + reachedBuildIndex.ToString());
    }

    public static void EraseLevelData() 
    {
        unlockedLevels = 0;
        reachedBuildIndex = 0;

        SaveLevelData();
    }

    private static void LoadSceneImmediatly(string sceneName) 
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

    private static void SaveLevelData() 
    {
        PlayerPrefs.SetInt("unlockedLevels", unlockedLevels);
        PlayerPrefs.SetInt("reachedBuildIndex", reachedBuildIndex);
        PlayerPrefs.Save();
    }
}