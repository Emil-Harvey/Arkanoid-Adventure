using System.Collections.Generic;
using UnityEngine;

public class paddleScript : MonoBehaviour
{
    private float speed = 1;
    private const float areaWidth = 14.985f;// barrier collision
    private float paddleWidth = 1.5f;// half paddle's real width

    public static int lives = 3;
    public static int score = 0;

    private static float input = 0;

    public static paddleScript instance;
    [SerializeField] GameObject ballPrefab;
    private List<GameObject> balls = new List<GameObject>();

    Sprite GetPaddle(int i) => Resources.LoadAll<Sprite>("Visual/Sprites/paddle")[i];

    public void OnPowerupGet(PowerupType type, float value = 0)
    {
        var ball = FindObjectOfType<ballScript>(); // doesn't seem to work in multiple cases? inefficient if ball not needed
        Debug.Log($"picked up {type}");
        switch (type)
        {
            case PowerupType.ACID:
                ball.ChangeType(BallType.green);
                break;
            case PowerupType.FIRE:
                ball.ChangeType(BallType.fire);
                break;
            case PowerupType.MULTI:
                 AddExtraBall(BallType.beach);
                 break;
            case PowerupType.WIDE:
                if ( paddleWidth < 3.0f ) paddleWidth = 3.0f;
                else paddleWidth = 1.5f;
                GetComponent<SpriteRenderer>().size = new Vector2(paddleWidth*2, 0.8f);
                break;
            case PowerupType.SHORT:
                if ( paddleWidth > 0.7f ) paddleWidth = 0.7f;
                else paddleWidth = 1.5f;
                GetComponent<SpriteRenderer>().size = new Vector2(paddleWidth*2, 0.8f);
                break;
            case PowerupType.REVERSE:
                speed = -speed;
                if ( speed < 0 ) GetComponent<SpriteRenderer>().sprite = GetPaddle(3);
                else GetComponent<SpriteRenderer>().sprite = GetPaddle(0);
                break;
            case PowerupType.SLOW:
                speed = (speed + 0.65f) % 1.30f; // toggle between 1.00 & 0.35.
                Debug.Log("new paddle speed = "+speed);
                if ( speed > 0.35f ) GetComponent<SpriteRenderer>().sprite = GetPaddle(2);
                else GetComponent<SpriteRenderer>().sprite = GetPaddle(0);
                break;

            default:
                Debug.Log("bugged powerup recieved (?)");
                break;
        }
    }

    void AddExtraBall(BallType type = BallType.grey)
    {
        var newBall = Instantiate(ballPrefab,
                    transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        if (type != BallType.grey)
            newBall.GetComponent<ballScript>().ChangeType(type);
        balls.Add(newBall); 
    }

    void Start()
    {
        instance = this;
        gameObject.transform.position = new Vector3(0.0f, -6.0f, 0.0f);// set start pos
        speed = 1.0f;
        lives = 3;

        if (GameObject.Find("ball"))
            balls.Add(GameObject.Find("ball"));

        if (balls.Count < 1)
        {
            var initialBall = Instantiate(ballPrefab,
                    transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            balls.Add(initialBall); //initialBall.transform.parent = this.transform; //
        }

        if (levelController.isFinalLevel)
            lives = 0;
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetKey(KeyCode.RightArrow) ? Mathf.Clamp(input * 1.04f, 0.1f, 1.2f)
              : Input.GetKey(KeyCode.LeftArrow) ? Mathf.Clamp(input * 1.04f, -1.2f, -0.1f)
              : Input.GetAxis("Mouse X");

        gameObject.transform.position += new Vector3(input * speed, 0.0f, 0.0f);// follow mouse   

        if (gameObject.transform.position.x < -(areaWidth - paddleWidth))// hit wall
        {
            //Debug.Log("hit wall: pos="+ -(areaWidth + paddleWidth) + ". pWidth=" + paddleWidth);
            gameObject.transform.position = new Vector3(-(areaWidth - paddleWidth), -6.0f, 0.0f);
        }
        else if (gameObject.transform.position.x > (areaWidth - paddleWidth))// hit other wall
        {
            gameObject.transform.position = new Vector3((areaWidth - paddleWidth), -6.0f, 0.0f);
        }

        //if (Input.GetKeyDown(KeyCode.B)) ;


        for (int i = 0; i < balls.Count; ++i)
        { // delete unused balls from array
            if (!balls[i])
                balls.RemoveAt(i);
        }
        //Debug.Log(ballCount);
        if (balls.Count < 1)
        {
            Debug.Log("paddle losing life");
            lives -= 1;
            var newBall = Instantiate(ballPrefab,
                        transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            balls.Add(newBall); //newBall.transform.parent = this.transform;//newBall.transform.SetParent(this.transform, true);
        }

    }
}