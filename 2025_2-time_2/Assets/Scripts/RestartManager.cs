using UnityEngine;
using UnityEngine.InputSystem;

public class RestartManager : MonoBehaviour
{
    public static bool blocked; 

    public void RestartScene(InputAction.CallbackContext context)
    {
        if (context.performed && !blocked)
            LevelManager.RestartLevel();
    }
}
