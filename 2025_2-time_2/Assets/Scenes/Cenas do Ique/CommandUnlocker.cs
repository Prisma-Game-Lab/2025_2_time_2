using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommandUnlocker : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private string unlockCommand = "/help";
    [SerializeField] private string unlockedText = "Text";

    private bool isUnlocked = false;

    public void OnEnterPressed(InputAction.CallbackContext context)
    {
        if (!context.performed || isUnlocked) return;

        if (inputField.text.Trim().ToLower() == unlockCommand.ToLower())
        {
            isUnlocked = true;
            inputField.text = unlockedText;
            inputField.interactable = true;
        }
    }
}
