using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public PlayerControls controls;
    public CommandUnlocker unlocker;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls = new PlayerControls();
        controls.UI.Enable(); // Ativa o mapa de ações UI

        if (unlocker != null)
        {
            controls.UI.Enter.performed += unlocker.OnEnterPressed;
        }
        else
        {
            Debug.LogError("Unlocker não está atribuído no InputReader!");
        }
    }

    private void OnDisable()
    {
        controls.UI.Enter.performed -= unlocker.OnEnterPressed;
        controls.Disable();
    }
}