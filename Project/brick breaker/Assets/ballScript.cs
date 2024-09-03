using UnityEngine;

public enum BallType { grey, gold, ice, fire, green, dark, bubble, beach, football, tennis }

public class ballScript : MonoBehaviour
{
    public BallType type;

    
    
    

    public GameObject paddle;
    public BoxCollider2D xCollide;
    public BoxCollider2D yCollide;
    public CircleCollider2D xyCollide;// for corners
    private Rigidbody2D rb;
    private Vector2 velocity => rb.velocity;

    private bool isFree = true;
    private Vector3 paddleXYZ;

    int speed = 3;

    AudioSource audio;
    public AudioClip impactSfx;
    public AudioClip fireImpactSfx;
    public AudioClip dieSfx;

    bool isOverlayHidden => (GameObject.Find("LevelTransition")?.GetComponent<levelController>().isOverlayHidden ?? true);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFree) return;

        //Debug.Log("collided with: "+ collision.gameObject.name + " (" + xDir + ", " + yDir+")");

        bool paddleCaught = (collision.gameObject.name == "paddle" && (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space)));

        if (paddleCaught)
        {
            isFree = false;
            Debug.Log("caught");
            return;
        }

        if (type != BallType.fire)
            audio.PlayOneShot(impactSfx);
        else
            audio.PlayOneShot(fireImpactSfx);

        if (type == BallType.beach || type == BallType.football || type == BallType.tennis)
        {
            var sr = GetComponent<SpriteRenderer>(); // spoof rotation
            sr.flipX = Time.frameCount % 2 == 1;
            sr.flipY = Time.frameCount % 4 >= 2;
        }
    }

    private void Reset()
    {
        rb.velocity = Vector2.zero;
        isFree = false;
    }

    /// Awake is called before Start
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Visual/Sprites/ball")[(int)type];
    }

    /// Start is called before the first frame update
    void Start()
    {
        audio.PlayOneShot(impactSfx);
        Reset();
    }

    /// Update is called once per frame
    void Update()
    {
        //Debug.Log("score " + score);
        
        if (isFree)
        {
            //gameObject.transform.Translate(xDir * speed * Time.deltaTime , yDir * speed * Time.deltaTime, 0.0f);

            if (speed < 12)
                speed = (int)(14 / Mathf.Sqrt( GameObject.FindGameObjectsWithTag("perish").Length) )
                    + ((type == BallType.fire) ? 6 : 4);
            // speed can only be between 3 & 12.

            // regulate velocity to ensure angle is mostly diagonal, not too vertical or horizontal
            if ((velocity.magnitude != speed || Mathf.Abs(velocity.normalized.y - 0.5f) > 0.45))
            {
                if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
                    rb.velocity = new Vector2(velocity.x * 0.8f, velocity.y * 1.2f).normalized * speed;
                else
                    rb.velocity = new Vector2(velocity.x * 1.2f, velocity.y * 0.8f).normalized * speed;
                Debug.Log("new velocity: " + velocity);
            }
        }
        else // ball is being held to the paddle:
        {

            paddleXYZ = new Vector3(GameObject.Find("paddle").transform.position.x, -5.0f, 0.0f);// position ball should be
            //Debug.Log(GameObject.Find("paddle").transform.position.x);
            //Debug.Log(paddleXYZ);

            /// follow paddle
            gameObject.transform.position = paddleXYZ;

            /// wait for input
            if ((Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) && isOverlayHidden)
            {
                isFree = true;
                Vector2 dir;
                dir.x = Random.Range(-1.0f, 1.0f); //-1 to 1
                dir.y = 0.66f;
                dir.y /= Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y); // normalise upward part of vector to align with horizontal part. Starting yDir as 0.66 of course limits the angle of any possible result but no matter.
                GetComponent<Rigidbody2D>().velocity = dir * speed;
            }
        }


        if (gameObject.transform.position.y < -7 || Input.GetMouseButtonDown(1))//  user can right click to reset if stuck, sacrifice a life
        {/// below paddle
            //  out of bounds

            GameObject.FindGameObjectWithTag("global_audio").GetComponent<AudioSource>().PlayOneShot(dieSfx); 

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
