using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType { grey, gold, ice, fire, green, dark, bubble, beach, football, tennis }

public class ballScript : MonoBehaviour
{
    public BallType type;

    public float xDir;
    public float yDir;
    //public Vector2 Dir;

    public GameObject paddle;
    public BoxCollider2D xCollide;
    public BoxCollider2D yCollide;
    public CircleCollider2D xyCollide;// for corners
    private bool isFree;
    private Vector3 paddleXYZ;

    int speed = 3;

    AudioSource audio;
    public AudioClip impactSfx;
    public AudioClip dieSfx;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFree) return;

        //Debug.Log("collided with: "+ collision.gameObject.name + " (" + xDir + ", " + yDir+")");

        bool paddleCaught = (collision.gameObject.name == "paddle" && (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)));

        if (paddleCaught)
        {
            isFree = false;
            Debug.Log("caught");
            return;
        }

        audio.PlayOneShot(impactSfx);

        if (xyCollide.IsTouching(collision.collider))
        {//   bounce
            yDir = -yDir;
            xDir = -xDir;
        }
        else if (xCollide.IsTouching(collision.collider)) { xDir = -xDir; }
        else if (yCollide.IsTouching(collision.collider)) { yDir = -yDir; }

        gameObject.transform.Translate(xDir * 0.12f, yDir * 0.12f, 0.0f);//exit collision

        //add some randomness to bounce direction for less predictability
        xDir = Random.Range(0.1f, 1.0f) * Mathf.Sign(xDir);
        yDir = yDir / Mathf.Sqrt(xDir * xDir + yDir * yDir);// normalise upward part of vector to align with horizontal part. 
                                                            //Debug.Log("new dir: (" + xDir + ", " + yDir + ")");

        if (type == BallType.beach || type == BallType.football || type == BallType.tennis)
        {
            var sr = GetComponent<SpriteRenderer>(); // spoof rotation
            sr.flipX = Time.frameCount % 2 == 1;
            sr.flipY = Time.frameCount % 4 >= 2;
        }
    }

    private void Reset()
    {
        xDir = 0;
        yDir = 0;
        isFree = false;
    }

    /// Awake is called before Start
    private void Awake()
    {
        //GameObject.Find("paddle").GetComponent<paddleScript>().AssignBallAwake(gameObject);
    }

    /// Start is called before the first frame update
    void Start()
    {
        //paddleScript.lives = 3;
        audio = GetComponent<AudioSource>();
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Visual/Sprites/ball")[(int)type];
        audio.PlayOneShot(impactSfx);
        Reset();
    }

    /// Update is called once per frame
    void Update()
    {
        
        //Debug.Log("score " + score);
        if (isFree)
        {
            gameObject.transform.Translate(xDir * speed * Time.deltaTime , yDir * speed * Time.deltaTime, 0.0f);
            //transform.position -= new Vector3( transform.parent.position.x, 0, 0);

            if (speed < 12)
                speed = (int)(14 / Mathf.Sqrt( GameObject.FindGameObjectsWithTag("perish").Length) )
                    + ((type == BallType.fire) ? 6 : 3);
            // speed can only be between 3 & 12.
        }
        else // ball is being held to the paddle:
        {

            paddleXYZ = new Vector3(GameObject.Find("paddle").transform.position.x, -5.0f, 0.0f);// position ball should be
            //Debug.Log(GameObject.Find("paddle").transform.position.x);
            //Debug.Log(paddleXYZ);

            /// follow paddle
            gameObject.transform.position = paddleXYZ;

            /// wait for input
            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
            {
                isFree = true;
                xDir = Random.Range(-1.0f, 1.0f); //-1 to 1
                yDir = 0.66f;
                yDir = yDir / Mathf.Sqrt(xDir * xDir + yDir * yDir);// normalise upward part of vector to align with horizontal part. Starting yDir as 0.66 of course limits the angle of any possible result but no matter.

            }
        }


        if (gameObject.transform.position.y < -7 || Input.GetMouseButtonDown(1))//  user can right click to reset if stuck, sacrifice a life
        {/// below paddle
            //  out of bounds

            audio.PlayOneShot(dieSfx);

            /// if no more balls
            //paddleScript.lives -= 1;
            //Reset();
            //Debug.Log("lives " + lives);
            /// else: 
            Destroy(gameObject);
        }

        
       
    }
    public void ChangeType(BallType newType)
    {
        type = newType;
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Visual/Sprites/ball")[(int)type];
    }
}
;
