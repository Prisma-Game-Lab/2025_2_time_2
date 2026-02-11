using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private GameObject blockedSprite;

    public void OnBlocked() 
    {
        blockedSprite.SetActive(true);
    }
}
