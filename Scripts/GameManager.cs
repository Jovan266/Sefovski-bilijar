using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int winner;
    public float drag;
    public float angularDrag;
    public float mass;
    public GameObject ballPrefab;
    public Vector3 startPosition;
    public float ballSize;
    public static GameManager instance;
    
    public int turn = 2;
    public bool foul = false;
    public bool wasFoul = false;
    public bool inHole = false;
    public int firstBallTurn = -1;
    public bool whiteInHole = false;
    public bool white = true;

    public bool firstBallIn = false;
    public GameObject[] balls;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void SetupBalls(bool what)
    {
        
        for(int i = 0; i < 15; i++)
        {
            if (i < 7) balls[i].GetComponent<Balls>().player = (what ? 1 : 2);
            else if (i == 7) balls[i].GetComponent<Balls>().player = 0;
            else balls[i].GetComponent<Balls>().player = (what ? 2 : 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        balls = new GameObject[15];
        int k = 1;
        for (int i = 0; i < 5; i++)
        {
            for (float j = i / -2f; j <= i / 2f; j++)
            {
                GameObject ball = Instantiate(ballPrefab, new Vector3(startPosition.x + ballSize * i, startPosition.y + ballSize * j, 0f), Quaternion.identity);
                ball.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Ball{k}");
                balls[k - 1] = ball;
                /*if (k < 8) ball.GetComponent<Balls>().player = 1;
                else if(k==8) ball.GetComponent<Balls>().player = 0;
                else ball.GetComponent<Balls>().player = 2;*/
                k++;
            }
        }
        foreach (GameObject ball in balls)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            rb.drag = drag;
            rb.angularDrag = angularDrag;
            rb.mass = mass;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Over(int win)
    {
        winner = win;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
