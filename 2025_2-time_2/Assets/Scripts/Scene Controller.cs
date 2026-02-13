using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAnim;

    [SerializeField] private float transitionTime = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (transform.parent != null)
            {
                transform.parent = null;
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        transitionAnim.SetFloat("Speed", 1 / transitionTime);
    }

    public float TriggerLevelTransition()
    {
        StartCoroutine(LoadLevel());
        return transitionTime;
    }

    IEnumerator LoadLevel()
    {
       transitionAnim.SetTrigger("End");
       yield return new WaitForSeconds(transitionTime);
       //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
       transitionAnim.SetTrigger("Start");
    }
}
