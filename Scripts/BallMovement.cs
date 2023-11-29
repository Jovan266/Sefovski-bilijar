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
                Debug.Log(GameManager.instance.turn);
            }
            if (GameManager.instance.foul)
            {
                if (lastTurn == GameManager.instance.turn)
                {
                    GameManager.instance.turn = (GameManager.instance.turn == 1 ? 2 : 1);
                }
                GameManager.instance.wasFoul = true;
                GameManager.instance.foul = false;
            }
            GameManager.instance.inHole = false;
        }
        wasStatic = isStatic;
        // Crtam UI za jacinu udarca
        bool actualHit = Input.GetMouseButton(0) && distance > 0.9f && isStatic;
        if (actualHit)
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
        if (actualHit)
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
        // Ostalo
        if (Input.GetMouseButtonUp(0) && isStatic)
        {
            if (GameManager.instance.wasFoul)
            {
                GameManager.instance.wasFoul = false;
                transform.position = mousePosition;
            }
            else if (distance < distanceThreshold)
            {
                rb.AddForce((transform.position - mousePosition) * strength);
            }
            else
            {
                rb.AddForce((transform.position - mousePosition).normalized * distanceThreshold * strength);
            }
        }
    }
    void FixedUpdate()
    {

    }
}
