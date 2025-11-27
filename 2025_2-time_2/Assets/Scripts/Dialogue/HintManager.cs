using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class HintManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HintUIManager hintUIManager;

    [Header("Hint Configuration")]
    [SerializeField] private List<HintSO> hints;      
    [SerializeField] private List<HintSO> dialogue;   

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float textSpeed = 0.05f;

    [Header("Auto Progression")]
    [SerializeField] private float autoAdvanceDelay = 2f;
    [SerializeField] private float autoHideDelay = 2f;

    [Header("Positioning Offset (world units)")]
    [SerializeField] private float y_offset = 0.3f;
    [SerializeField] private float x_offset = -0.65f;

    private int currentHintIndex = 0;
    private int currentLineIndex = 0;

    private bool isShowing = false;
    private bool isDialogueSequence = false;   

    private Coroutine typingCoroutine;
    private Coroutine autoAdvanceCoroutine;

    private GameObject player;
    private Camera cam;

    private HintSO currentHint; // the hint currently being displayed

    public UnityEvent OnDialogueEnd;

    [SerializeField] private int dialogueEndEventIndex;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        
        if (text != null)
        {
            text.text = string.Empty;
            text.gameObject.SetActive(false);
        }

        
        StartDialogueSequence();
    }

    void Update()
    {
        if (!isShowing || player == null || cam == null || text == null) return;

        
        Vector3 screenPos = cam.WorldToScreenPoint(
            new Vector3(
                player.transform.position.x + x_offset,
                player.transform.position.y + y_offset,
                player.transform.position.z
            )
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
        if (hints == null || hints.Count == 0 || text == null) return;
        if (hintIndex < 0 || hintIndex >= hints.Count) return;
        if (isShowing) return;

        isDialogueSequence = false; 
        currentHintIndex = hintIndex;
        currentHint = hints[currentHintIndex];

        StartHint();
    }

    public void AdvanceText()
    {
        if (!isShowing || currentHint == null || text == null) return;

        string[] lines = currentHint.dialogue;
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

        
        if (currentLineIndex >= lines.Length - 1)
        {

            if (isDialogueSequence && currentHintIndex == dialogueEndEventIndex)
            {
                OnDialogueEnd?.Invoke();  
            }

            FinishCurrentHint(immediate: true);
            return;
        }

        
        currentLineIndex++;
        typingCoroutine = StartCoroutine(TypeLine());
    }

   public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

       
        if (isShowing)
        {
            AdvanceText();
            return;
        }

       
        if (cam == null || player == null) return;

        
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        // Physics2D requires a collider on the player
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        if (hit != null && hit.gameObject == player)
        {
            StartDialogueSequence();
        }
        }

  

    private void StartHint()
    {
        isShowing = true;
        currentLineIndex = 0;

        text.gameObject.SetActive(true);
        text.text = string.Empty;

        // cancel previous coroutines if any
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }

        typingCoroutine = StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }

        text.text = "";
        string[] lines = currentHint.dialogue;
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

        if (!isShowing || currentHint == null) yield break;

        string[] lines = currentHint.dialogue;
        if (currentLineIndex < lines.Length - 1)
        {
            currentLineIndex++;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            if (isDialogueSequence && currentHintIndex == dialogueEndEventIndex)
            {
                OnDialogueEnd?.Invoke();  
            }
            FinishCurrentHint(immediate: false);
        }
    }

    private void FinishCurrentHint(bool immediate)
    {
        
        if (!isDialogueSequence && hintUIManager != null)
        {
            hintUIManager.UnlockNextHint();
        }

        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }

        

        if (immediate)
        {
            EndHintSession();
        }
        else
        {
            StartCoroutine(FinishHintRoutine());
        }
    }

    private IEnumerator FinishHintRoutine()
    {
        yield return new WaitForSeconds(autoHideDelay);
        EndHintSession();
    }

    private void EndHintSession()
    {
        isShowing = false;
        text.gameObject.SetActive(false);

        if (isDialogueSequence)
        {

            currentHintIndex++;

            if (dialogue != null && currentHintIndex < dialogue.Count)
            {  

                
                
                
                currentHint = dialogue[currentHintIndex];
                
                StartHint();
            }
            else
            {
               
                currentHint = null;
                isDialogueSequence = false;
            }
            
        }
        else
        {
            
        }
    }

    public void StartDialogueSequence()
    {
    if (dialogue == null || dialogue.Count == 0 || text == null) return;

   
    if (isShowing) return;

    isDialogueSequence = true;
    currentHintIndex = 0;
    currentHint = dialogue[currentHintIndex];

    StartHint(); 
    }
}
