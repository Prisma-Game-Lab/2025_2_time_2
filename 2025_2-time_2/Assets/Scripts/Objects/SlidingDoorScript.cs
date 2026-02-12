using UnityEngine;

public class SlidingDoorScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform destination;
    [SerializeField] private float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool buttonPressed = false;
    private bool stopped;

    private void Awake()
    {
        closedPosition = transform.position;
        openPosition = destination.position;
    }

    private void FixedUpdate()
    {
        if (stopped) return;

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

    public void OnCommandStart(CommandEffectType effect) 
    {
        if (effect == CommandEffectType.Stop) 
        {
            stopped = true;
        }
    }

    public void OnCommandEnd(CommandEffectType effect)
    {
        if (effect == CommandEffectType.Stop)
        {
            stopped = false;
        }
    }
}
