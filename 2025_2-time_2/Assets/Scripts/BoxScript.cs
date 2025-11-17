using UnityEngine;

public class BoxScript : MonoBehaviour
{
    
    [SerializeField] private Transform tf; 

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
       
        float scaleX = tf.localScale.x;

        bool shouldBeMovable = scaleX < 0.75f; 

        if (shouldBeMovable)
        {
            
            if (rb.bodyType != RigidbodyType2D.Dynamic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }
        else
        {
           
            if (rb.bodyType != RigidbodyType2D.Static)
            {
               
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}
