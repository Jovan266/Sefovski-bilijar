using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float minSpeed = 0.1f;
    public float strength = 1000;
    public float distanceThreshold;
    public LayerMask hitMask;
    public GameObject fake;
    bool wasStatic = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        float distance = Vector3.Distance(mousePosition, transform.position);
        LineRenderer hitStrengthRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        LineRenderer aimRenderer = transform.GetChild(1).GetComponent<LineRenderer>();
        bool isStatic = true;
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            if (ball.GetComponent<Rigidbody2D>().velocity.magnitude > minSpeed) isStatic = false;
        }
        int lastTurn = GameManager.instance.turn;
        if (!wasStatic && isStatic)
        {
            if (!GameManager.instance.inHole)
            {
                GameManager.instance.turn = (GameManager.instance.turn == 1 ? 2 : 1);
                UIManager.instance.SetTurn(GameManager.instance.turn);
            }
            if(GameManager.instance.firstBallTurn == -1 && GameManager.instance.firstBallIn)
            {
                GameManager.instance.foul = true;
            }
            if (GameManager.instance.foul)
            {
                UIManager.instance.Foul();
                if (lastTurn == GameManager.instance.turn)
                {
                    GameManager.instance.turn = (GameManager.instance.turn == 1 ? 2 : 1);
                    UIManager.instance.SetTurn(GameManager.instance.turn);
                }
                GameManager.instance.wasFoul = true;
                rb.velocity = new Vector3(0f, 0f, 0f);
                transform.position = new Vector3(100f, 100f, 100f);
                GameManager.instance.foul = false;
            }
            GameManager.instance.inHole = false;
            GameManager.instance.firstBallTurn = -1;
        }
        wasStatic = isStatic;
        // Crtam UI za jacinu udarca
        bool actualHit = Input.GetMouseButton(0) && distance > 0.9f && isStatic;
        if (actualHit && !GameManager.instance.wasFoul)
        {
            hitStrengthRenderer.enabled = true;
            hitStrengthRenderer.SetPosition(0, transform.position + (mousePosition - transform.position).normalized * 0.5f);
            if (distance > distanceThreshold)
            {
                hitStrengthRenderer.SetPosition(1, transform.position + (mousePosition - transform.position).normalized * distanceThreshold);
            }
            else
            {
                hitStrengthRenderer.SetPosition(1, mousePosition);
            }
        }
        else
        {
            hitStrengthRenderer.enabled = false;
        }
        // Crtam UI za smer udarca
        if (actualHit && !GameManager.instance.wasFoul)
        {
            aimRenderer.enabled = true;
            aimRenderer.SetPosition(0, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (transform.position - mousePosition).normalized, 20f, hitMask);
            //Debug.DrawRay(transform.position, (transform.position - mousePosition).normalized * 10f, Color.white);
            if (hit.collider != null)
            {
                aimRenderer.SetPosition(1, hit.point);
            }
        }
        else
        {
            aimRenderer.enabled = false;
        }
        if (GameManager.instance.wasFoul)
        {
            fake.SetActive(true);
            fake.transform.position = mousePosition;
        }
        else
        {
            fake.SetActive(false);
        }
        // Ostalo
        if (Input.GetMouseButtonUp(0) && isStatic)
        {
            if (GameManager.instance.wasFoul)
            {
                if (Mathf.Abs(fake.transform.position.x) < 10f && Mathf.Abs(fake.transform.position.y) < 5f)
                {
                    GameManager.instance.wasFoul = false;
                    transform.position = mousePosition;
                }
            }
            else if (distance < distanceThreshold)
            {
                rb.AddForce((transform.position - mousePosition) * strength);
                GetComponent<AudioSource>().Play();
            }
            else
            {
                rb.AddForce((transform.position - mousePosition).normalized * distanceThreshold * strength);
                GetComponent<AudioSource>().Play();
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            if(GameManager.instance.firstBallTurn == -1 && GameManager.instance.firstBallIn)
            {
                int number = int.Parse(collision.gameObject.GetComponent<SpriteRenderer>().sprite.name.Substring(4));
                GameManager.instance.firstBallTurn = GameManager.instance.balls[number-1].GetComponent<Balls>().player;
                if(GameManager.instance.firstBallTurn != GameManager.instance.turn)
                {
                    GameManager.instance.foul = true;
                }
            }
        }
        if (collision.gameObject.tag == "Wall")
        {

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector3 velocity = rb.velocity;
            velocity.x = Mathf.Abs(velocity.x);
            velocity.y = Mathf.Abs(velocity.y);
            velocity.z = 0f;
            if (transform.position.y < -4.3f || transform.position.x > 9.2f)
            {
                rb.AddForce(Vector2.Perpendicular(velocity).normalized * 5f);
            }
            else
            {
                rb.AddForce(-Vector2.Perpendicular(velocity).normalized * 5f);
            }
        }
    }
    void FixedUpdate()
    {

    }
}
