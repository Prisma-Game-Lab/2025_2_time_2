using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour
{
    [SerializeField] private string initialCutsceneName;
    [SerializeField] private Button[] levelButtons;

    private void Start()
    {
        int unlockedLevels = LevelManager.unlockedLevels;
        int i = 0;
        foreach (Button levelButton in levelButtons) 
        {
            if (unlockedLevels >= 0)
            {
                levelButton.interactable = true;
                unlockedLevels--;
            }
            else 
            {
                levelButton.interactable = false;
                levelButton.GetComponent<LevelSelectButton>().OnBlocked();
            }

            i++;
            int localI = i;
            levelButton.onClick.AddListener(() => OnButtonPress(localI));
        }
    }

    public void OnButtonPress(int index) 
    {
        if (index == 1) 
        {
            LevelManager.LoadSceneByName(initialCutsceneName);
        }
        else 
        {
            LevelManager.LoadSceneByName("Level " + index.ToString());
        }
    }

    public void OnExitButtonPress() 
    {
        LevelManager.LoadSceneByName("MainMenu");
    }

    public void OnButtonHover()
    {
        AudioManager.Instance.PlaySFX("ButtonHover");
    }
}
