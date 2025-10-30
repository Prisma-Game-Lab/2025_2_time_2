using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HintUIManager : MonoBehaviour
{
    [Header("Hint Buttons")]
    [SerializeField] GameObject[] hintButtons;

    private int unlockedIndex = 0;
    void Start()
    {
        for (int i = 1; i< hintButtons.Length; i++)
        {
            hintButtons[i].SetActive(false);
        }
    }

        public void UnlockNextHint()
    {
        if(unlockedIndex + 1 < hintButtons.Length)
        {
            unlockedIndex += 1;
            hintButtons[unlockedIndex].SetActive(true); 
        }
    }
}
