using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform tf; 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CommandTarget target;

    [Header("Variables")]
    [SerializeField] private float smallMass;
    [SerializeField] private float mediumMass;
    [SerializeField] private float bigMass;
    [SerializeField] private float alteringMass;
    [SerializeField] private float frictionCoeficient;
    [SerializeField] private float minSpeed;
    [SerializeField] private GameObject playerPushNegator;

    private bool stopped = false;

    private void Start()
    {
        SetReferences();
        CalculatePushMovement();
    }

    private void FixedUpdate()
    {
        if (stopped) return;

        if (Mathf.Abs(rb.velocity.x) > minSpeed)
            rb.velocity -= Vector2.right * rb.velocity.x * (frictionCoeficient * Time.deltaTime);
        //else
            //rb.velocity *= Vector2.up;
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

    public void CalculatePushMovement()
    {
        SetReferences();

        switch (target.GetTargetSize()) 
        {
            case TargetSize.Small:
                rb.mass = smallMass;
                playerPushNegator.SetActive(false);
                break;
            case TargetSize.Medium:
                rb.mass = mediumMass;
                playerPushNegator.SetActive(true);
                break;
            case TargetSize.Big:
                rb.mass = bigMass;
                playerPushNegator.SetActive(true);
                break;
            case TargetSize.Altering:
                rb.mass = alteringMass;
                playerPushNegator.SetActive(true);
                break;
        }
    }

    private void SetReferences() 
    {
        if (tf == null)
            tf = transform;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (target != null)
            target = GetComponent<CommandTarget>();
    }
}
