using UnityEngine;

public class SlidingDoorScript : MonoBehaviour
{
    [Header("Movement")]
   
    [SerializeField] private float travelDistance = 3f;

    [SerializeField] private float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool buttonPressed = false;

    private void Awake()
    {
        closedPosition = transform.position;

        
        openPosition = closedPosition + transform.up * travelDistance;
    }

    private void Update()
    {
        Vector3 target = buttonPressed ? openPosition : closedPosition;

        
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );
    }

    
    public void OnButtonPressed()
    {
        buttonPressed = true;
    }

    public void OnButtonReleased()
    {
        buttonPressed = false;
    }
}
