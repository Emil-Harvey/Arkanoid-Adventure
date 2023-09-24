using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paddleScript : MonoBehaviour
{
    public float speed;
    private float areaWidth = 14.985f;// barrier collision
    private float paddleWidth = 1.5f;// half paddle's real width

    public static int lives = 3;

    public static int score = 0;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] private List<GameObject> balls = new List<GameObject>();

    //bool started = false;

    public void OnPowerupGet(PowerupType type, float value = 0)
    {
        var ball = FindObjectOfType<ballScript>(); // doesn't seem to work in multiple cases? inefficient if ball not needed
        switch (type)
        {
            case PowerupType.ACID:
                //var ball
                ball.ChangeType(BallType.green);
                break;
            case PowerupType.FIRE:
                //var ball = FindObjectOfType<ballScript>(); unnecessary assignment?
                ball.ChangeType(BallType.fire);
                break;
            case PowerupType.WIDE:
                Debug.Log("wide");
                paddleWidth = 3.0f;
                GetComponent<SpriteRenderer>().size = new Vector2(paddleWidth*2, 0.8f);
                break;
            case PowerupType.SHORT:
                paddleWidth = 0.7f;
                GetComponent<SpriteRenderer>().size = new Vector2(paddleWidth*2, 0.8f);
                break;
            case PowerupType.REVERSE:
                speed = -speed;
                break;
            case PowerupType.SLOW:
                speed *= 0.35f;
                break;
            default:
                Debug.Log("bugged powerup recieved (?)");
                break;
        }
    }

    void Start()
    {
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

        //started = true;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(Input.GetAxis("Mouse X") * speed, 0.0f, 0.0f);// follow mouse   

        if (gameObject.transform.position.x < -(areaWidth - paddleWidth))// hit wall
        {
            //Debug.Log("hit wall: pos="+ -(areaWidth + paddleWidth) + ". pWidth=" + paddleWidth);
            gameObject.transform.position = new Vector3(-(areaWidth - paddleWidth), -6.0f, 0.0f);
        }
        else if (gameObject.transform.position.x > (areaWidth - paddleWidth))// hit other wall
        {
            gameObject.transform.position = new Vector3((areaWidth - paddleWidth), -6.0f, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            var newBall = Instantiate(ballPrefab,
                        transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            balls.Add(newBall); //newBall.transform.parent = this.transform;//
        }

        for (int i = 0; i<balls.Count; i++){ // delete unused balls from array
            if (!balls[i])
                balls.RemoveAt(i);
        }

        if (/*transform.childCount*/balls.Count < 1)
        {
            lives -= 1;
            var newBall = Instantiate(ballPrefab,
                        transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            balls.Add(newBall); //newBall.transform.parent = this.transform;//newBall.transform.SetParent(this.transform, true);
        }
        
    }
}