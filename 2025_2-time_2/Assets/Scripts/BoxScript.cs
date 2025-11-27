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

    private void Start()
    {
        SetReferences();
        CalculatePushMovement();
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
        }
        else 
        {
            rb.mass = bigMass;
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
