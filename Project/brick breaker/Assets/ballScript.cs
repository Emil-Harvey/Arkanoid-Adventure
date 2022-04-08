using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballScript : MonoBehaviour
{
    public int xDir;
    public int yDir;
    public static int lives = 3;
    public GameObject paddle;
    public BoxCollider2D xCollide;
    public BoxCollider2D yCollide;
    public CircleCollider2D xyCollide;// for corners
    private bool isFree;
    private Vector3 paddleXYZ;

    int speed = 3;

    public static int score = 0;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Debug.Log("collided with: "+ collision.gameObject.name + " (" + xDir + ", " + yDir+")");
       
        if (xyCollide.IsTouching(collision.collider)) {//   bounce
           yDir = -yDir;
           xDir = -xDir;
        }
        else if (xCollide.IsTouching(collision.collider)) { xDir = -xDir; }
        else if (yCollide.IsTouching(collision.collider)) { yDir = -yDir; }

        gameObject.transform.Translate(xDir * 0.12f, yDir * 0.12f, 0.0f);//exit collision
        Debug.Log("new dir: (" + xDir + ", " + yDir + ")");
    }

    private void Reset()
    {
        xDir = 0;
        yDir = 0;
        isFree = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("score " + score);
        if (isFree)
        {
            gameObject.transform.Translate(xDir * speed * Time.deltaTime , yDir * speed * Time.deltaTime, 0.0f);
        }
        else
        {

            paddleXYZ = new Vector3(GameObject.Find("paddle").transform.position.x, -5.0f, 0.0f);// position ball should be
            //Debug.Log(GameObject.Find("paddle").transform.position.x);
            //Debug.Log(paddleXYZ);

            gameObject.transform.position = paddleXYZ;// follow paddle

            //wait for input
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                isFree = true;
                yDir = 1;
                xDir = Random.Range(0, 2) * 2 - 1; //1 or -1
            }
        }


        if (gameObject.transform.position.y < -7 || Input.GetMouseButtonDown(1))//  user can right click to reset if stuck, sacrifice a life
        {//below paddle
            //  out of bounds
            lives -= 1;
            Reset();
            //Debug.Log("lives " + lives);


        }
    }

}

;
