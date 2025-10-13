using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

   
    private GameObject playerReference;

    public UnityEvent<bool> OnPause;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public GameObject GetPlayerRef()
    {
        if (playerReference == null)
        {
            playerReference = GameObject.FindWithTag("Player");
        }
        return playerReference;
    }



    public void SetPause(bool state)
    {
        if (state)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        OnPause.Invoke(state);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

   
}