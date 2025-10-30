using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour

{

    [Header("References")]
    [SerializeField] private HintUIManager hintUIManager;

    [Header("Hint Configuration")]
    [SerializeField] private List<HintSO> hints;          
    [SerializeField] private TextMeshProUGUI text;        
    [SerializeField] private float textSpeed = 0.05f;     

    [Header("Auto Progression")]
    [SerializeField] private float autoAdvanceDelay = 2f; 
    [SerializeField] private float autoHideDelay = 2f;    

    [Header("Positioning Offset")]
    [SerializeField] private float y_offset = 0.3f;       
    [SerializeField] private float x_offset = -0.65f;    

    private int currentHintIndex = 0;    
    private int currentLineIndex = 0;   
    private bool canAdvance = true;
    private bool isShowing = false;      
    private Coroutine typingCoroutine;
    private Coroutine autoAdvanceCoroutine;

    private GameObject player;
    private Camera cam;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;

        if (text != null)
        {
            text.text = string.Empty;
            text.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isShowing || player == null || cam == null || text == null) return;

        
        Vector3 screenPos = cam.WorldToScreenPoint(
            new Vector3(player.transform.position.x + x_offset,
                        player.transform.position.y + y_offset,
                        player.transform.position.z)
        );

        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            text.canvas.transform as RectTransform,
            screenPos,
            text.canvas.worldCamera,
            out Vector2 localPoint
        );

        
        text.rectTransform.pivot = new Vector2(0.5f, 0f); 
        text.rectTransform.anchoredPosition = localPoint;
    }

   
    public void DisplayCurrentHint(int hintIndex)
    {
        if (hints == null || hints.Count == 0 || text == null ) return;
        if (isShowing) return; 

        
        isShowing = true;
        currentLineIndex = 0;
        currentHintIndex = hintIndex;
        text.gameObject.SetActive(true);
        text.text = string.Empty;

        
        typingCoroutine = StartCoroutine(TypeLine());
    }

    
    public void AdvanceText()
    {
        if (!isShowing || !canAdvance || hints == null || hints.Count == 0 || text == null) return;

        StartCoroutine(ClickCooldown());

        string[] lines = hints[currentHintIndex].dialogue;
        if (lines == null || lines.Length == 0)
        {
            FinishCurrentHint(immediate: true);
            return;
        }

        
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            text.text = lines[currentLineIndex];
            return;
        }

        
        if (currentLineIndex < lines.Length - 1)
        {
            currentLineIndex++;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            
            FinishCurrentHint(immediate: true);
        }
    }

    

    private IEnumerator TypeLine()
    {
       
        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }

        text.text = "";
        string[] lines = hints[currentHintIndex].dialogue;
        string line = (lines != null && lines.Length > 0) ? lines[currentLineIndex] : "";

        foreach (char c in line)
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        typingCoroutine = null;

        
        autoAdvanceCoroutine = StartCoroutine(AutoAdvanceAfterDelay());
    }

    private IEnumerator AutoAdvanceAfterDelay()
    {
        yield return new WaitForSeconds(autoAdvanceDelay);

        if (!isShowing) yield break;

        string[] lines = hints[currentHintIndex].dialogue;
        if (currentLineIndex < lines.Length - 1)
        {
            currentLineIndex++;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            FinishCurrentHint(immediate: false);
        }
    }

    
    private void FinishCurrentHint(bool immediate)
    {
        hintUIManager.UnlockNextHint();
        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }

        if (immediate)
        {
            
            isShowing = false;
            text.gameObject.SetActive(false);
            //currentHintIndex = (currentHintIndex + 1) % hints.Count;
        }
        else
        {
            
            StartCoroutine(FinishHintRoutine());
        }
    }

    private IEnumerator FinishHintRoutine()
    {
        yield return new WaitForSeconds(autoHideDelay);

        isShowing = false;
        text.gameObject.SetActive(false);
        currentHintIndex = (currentHintIndex + 1) % hints.Count;
    }

    private IEnumerator ClickCooldown()
    {
        canAdvance = false;
        yield return new WaitForSeconds(0.1f);
        canAdvance = true;
    }
}
