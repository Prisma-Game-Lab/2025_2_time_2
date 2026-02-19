using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCutsceneScript : MonoBehaviour
{
    [SerializeField] private HintManager hintManager;
    [SerializeField] private Image cutsceneImage;
    [SerializeField] private Animator animator;

    [SerializeField] private float timeToWeirdSound;
    [SerializeField] private float timeToEnterPcSound;
    [SerializeField] private float timeToNextDialogue;
    [SerializeField] private float timeToSceneTransition;
    [SerializeField] private string weirdSoundName;
    [SerializeField] private string enterPcSoundName;
    [SerializeField] private string level1Name;
    [SerializeField] private Sprite[] cutsceneSprites;

    private int dialogueIndex = 0;

    private void Start()
    {
        AudioManager.Instance.StopMusic();
    }

    public void OnDialogueEnd() 
    {
        switch (dialogueIndex) 
        {
            case 0:
                hintManager.dialogueEndEventIndex++;
                cutsceneImage.sprite = cutsceneSprites[1];
                break;
            case 1:
                hintManager.dialogueEndEventIndex = 3;
                StartCoroutine(CutsceneTimer());
                break;
        }
        dialogueIndex++;
    }

    public void OnHintEnd() 
    {
        StartCoroutine(LevelTransitionTimer());
    }

    private IEnumerator CutsceneTimer() 
    {
        yield return new WaitForSeconds(timeToWeirdSound);
        AudioManager.Instance.PlaySFX(weirdSoundName);
        yield return new WaitForSeconds(timeToEnterPcSound);
        AudioManager.Instance.PlaySFX(enterPcSoundName);
        animator.enabled = true;
        animator.SetTrigger("Panic");
        yield return new WaitForSeconds(timeToNextDialogue);
        hintManager.DisplayCurrentHint(0);
    }

    private IEnumerator LevelTransitionTimer()
    {
        yield return new WaitForSeconds(timeToSceneTransition);
        LevelManager.LoadSceneByName(level1Name);
    }
}
