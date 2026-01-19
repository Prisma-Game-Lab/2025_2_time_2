using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform tf; 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CommandTarget target;

    [Header("Variables")]
    [SerializeField] private float smallMass;
    [SerializeField] private float bigMass;
    [SerializeField] private float alteringMass;
    [SerializeField] private float frictionCoeficient;

    private void Start()
    {
        SetReferences();
        CalculatePushMovement();
    }

    private void FixedUpdate()
    {
        rb.velocity -= Vector2.right * rb.velocity.x * (frictionCoeficient * Time.deltaTime);
    }

    //private void FixedUpdate()
    //{

    //    float scaleX = tf.localScale.x;

    //    bool shouldBeMovable = scaleX < 0.75f; 

    //    if (shouldBeMovable)
    //    {

    //        if (rb.bodyType != RigidbodyType2D.Dynamic)
    //        {
    //            rb.bodyType = RigidbodyType2D.Dynamic;
    //            rb.constraints = RigidbodyConstraints2D.None;
    //        }
    //    }
    //    else
    //    {

    //        if (rb.bodyType != RigidbodyType2D.Static)
    //        {

    //            rb.velocity = Vector2.zero;
    //            rb.angularVelocity = 0f;
    //            rb.bodyType = RigidbodyType2D.Static;
    //        }
    //    }
    //}
    //private void FixedUpdate()
    //{

    //    float scaleX = tf.localScale.x;

    //    bool shouldBeMovable = scaleX < 0.75f; 

    //    if (shouldBeMovable)
    //    {

    //        if (rb.bodyType != RigidbodyType2D.Dynamic)
    //        {
    //            rb.bodyType = RigidbodyType2D.Dynamic;
    //            rb.constraints = RigidbodyConstraints2D.None;
    //        }
    //    }
    //    else
    //    {

    //        if (rb.bodyType != RigidbodyType2D.Static)
    //        {

    //            rb.velocity = Vector2.zero;
    //            rb.angularVelocity = 0f;
    //            rb.bodyType = RigidbodyType2D.Static;
    //        }
    //    }
    //}

    public void CalculatePushMovement()
    {
        SetReferences();

        if (target.GetTargetSize() == TargetSize.Small)
        {
            rb.mass = smallMass;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if (target.GetTargetSize() == TargetSize.Altering) 
        {
            rb.mass = alteringMass;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else 
        {
            rb.mass = bigMass;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
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
