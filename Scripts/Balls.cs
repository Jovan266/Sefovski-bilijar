using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls : MonoBehaviour
{
    public float factor;
    public int player=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector3 velocity = rb.velocity;
            velocity.x = Mathf.Abs(velocity.x);
            velocity.y = Mathf.Abs(velocity.y);
            velocity.z = 0f;
            if (transform.position.y < -4.3f || transform.position.x > 9.2f)
            {
                rb.AddForce(Vector2.Perpendicular(velocity).normalized * factor);
            }
            else
            {
                rb.AddForce(-Vector2.Perpendicular(velocity).normalized * factor);
            }
        }
    }

    //Jebeni bag koji nam je oduzeo 2 dana
    private void OnDrawGizmos()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector3 velocity = rb.velocity;
        velocity.x = Mathf.Abs(velocity.x);
        velocity.y = Mathf.Abs(velocity.y);
        velocity.z = 0f;
        Vector2 lol = Vector2.Perpendicular(velocity).normalized * factor;
        if (transform.position.y < -4.3f || transform.position.x > 9.2f)
        {
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(lol.x, lol.y, 0));
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position - new Vector3(lol.x, lol.y, 0));
        }
    }
}
