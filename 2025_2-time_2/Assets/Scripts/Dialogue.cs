using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string[] lines;

    [SerializeField] private float textSpeed;

    private bool canAdvance = true;

    private int index;
    private Coroutine typingCoroutine;

    IEnumerator TypeLine()
    {
        text.text = "";
        foreach (char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void Start()
        {
        text.text = string.Empty;
        StartDialogue();
        }
        
    public void AdvanceText()
    {
        if (!canAdvance) return;
        StartCoroutine(ClickCooldown());

        if (text.text == lines[index])
        {
            NextLine();
        }
        else
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine); 
            text.text = lines[index];
        }
    }

    void StartDialogue()
    {
        index = 0;
        typingCoroutine = StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    IEnumerator ClickCooldown()
        {   
        canAdvance = false;
        yield return new WaitForSeconds(0.1f);
        canAdvance = true;
    }
}

