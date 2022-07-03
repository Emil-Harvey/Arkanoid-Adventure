using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paddleScript : MonoBehaviour
{
    public float speed;
    private float areaWidth = 13.485f;// barrier collision
    //private float paddleWidth

    public static int lives = 3;

    public static int score = 0;

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
            default:
                Debug.Log("bugged powerup recieved (?)");
                break;
        }
    }

    void Start()
    {
        gameObject.transform.position = new Vector3(0.0f, -6.0f, 0.0f);// set start pos
        speed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(Input.GetAxis("Mouse X") * speed, 0.0f, 0.0f);// follow mouse   
        if (gameObject.transform.position.x < -areaWidth)
        {
            //hit wall
            gameObject.transform.position = new Vector3(-areaWidth, -6.0f, 0.0f);
        }
        else if (gameObject.transform.position.x > areaWidth)
        {
            //hit other wall
            gameObject.transform.position = new Vector3(areaWidth, -6.0f, 0.0f);
        }


    }
    
}