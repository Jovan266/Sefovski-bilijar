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
            if (collision.GetComponent<SpriteRenderer>().sprite.name == "Ball8")
            {
                bool playerBallIncluded = false;
                foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
                {
                    if (ball.GetComponent<BallMovement>() == null && ball.GetComponent<Balls>().player == GameManager.instance.turn)
                    {
                        playerBallIncluded = true;
                    }
                }
                if (playerBallIncluded)
                {
                    GameManager.instance.Over(GameManager.instance.turn);
                }
                else
                {
                    GameManager.instance.Over(GameManager.instance.turn == 1 ? 2 : 1);
                }
            }
            GameManager.instance.inHole = true;
            if (!GameManager.instance.firstBallIn && collision.gameObject.name != "Player")
            {
                bool full = int.Parse(collision.GetComponent<SpriteRenderer>().sprite.name.Substring(4)) < 8;
                if (GameManager.instance.turn == 1 )
                {
                    GameManager.instance.firstBallTurn = 1;
                    GameManager.instance.SetupBalls(full);
                }
                else
                {
                    GameManager.instance.firstBallTurn = 2;
                    GameManager.instance.SetupBalls(!full);
                }
                GameManager.instance.firstBallIn = true;
            }
            if (collision.gameObject.name != "Player")
            {
                GameManager.instance.white = false;
                GameObject deadBall = collision.gameObject;
                deadBall.tag = "Untagged";
                deadBall.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                deadBall.GetComponent<Rigidbody2D>().angularVelocity = 0;
                deadBall.transform.position = new Vector3(((deadBall.GetComponent<Balls>().player == 2) ? 1 : -1) * 11, -5f, 0f);
                Destroy(deadBall.GetComponent<Balls>());
            }
            else
            {
                GameManager.instance.foul = true;
                collision.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
                collision.GetComponent<BallMovement>().transform.position = new Vector3(100f, 100f, 100f);
            }
            GetComponent<AudioSource>().Play();
        }
    }
}
