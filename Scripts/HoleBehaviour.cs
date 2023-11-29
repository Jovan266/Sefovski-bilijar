using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class HoleBehaviour : MonoBehaviour
{
    public bool debug;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")// && (collision.gameObject.GetComponent<BallMovement>() == null))
        {
            GameManager.instance.inHole = true;
            if (!GameManager.instance.firstBallIn)
            {
                bool full = int.Parse(collision.GetComponent<SpriteRenderer>().sprite.name.Substring(4)) < 8;
                if (GameManager.instance.turn == 1)
                {
                    GameManager.instance.SetupBalls(full);
                }
                else
                {
                    GameManager.instance.SetupBalls(!full);
                }
                GameManager.instance.firstBallIn = true;
            }
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.name != "Player") Destroy(collision.gameObject);
            else
            {
                GameManager.instance.foul = true;
                collision.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
                collision.GetComponent<BallMovement>().transform.position = new Vector3(100f, 100f, 100f);
                Debug.Log("kurac");
            }
        }
    }
}
