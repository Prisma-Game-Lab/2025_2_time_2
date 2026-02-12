using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string scene) 
    {
        LevelManager.LoadSceneByName(scene, true);
    }
}
