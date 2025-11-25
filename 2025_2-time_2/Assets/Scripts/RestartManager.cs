using UnityEngine;
using UnityEngine.InputSystem;

public class RestartManager : MonoBehaviour
{
    public void RestartScene(InputAction.CallbackContext context)
    {
        if (context.performed)
            LevelManager.RestartLevel();
    }
}
