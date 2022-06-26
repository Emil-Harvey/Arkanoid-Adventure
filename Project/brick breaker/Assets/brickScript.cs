using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

enum brickType { stoneBrick, mossStoneBrick, sandBrick, mossSandBrick, snowBrick, iceBrick, metalBrick, goldBrick }
struct flag { public bool b1, b2; }

public class brickScript : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite[] sprites;
    Sprite fullHealthSprite;
    Sprite midHealthSprite;
    Sprite lowHealthSprite;

    int health;
    [SerializeField] brickType currentType;
    [SerializeField] GameObject damageEffectOverlay;
    float damageEffectTimer = 0.0f;

    public bool isPowerup = false;
    [SerializeField] GameObject powerupOverlay;
    public GameObject powerupType;

    //[SerializeField]
    //private SpriteAtlas atlas;
    //public GameObject ball;
    //public 
    AudioSource audio;
    public AudioClip impactSfx;
    public AudioClip destroySfx;

    flag effects; //= { false, false };

    void takeDamage()
    {
        health -= 1;
        paddleScript.score += 10;

        switch (health)
        {
            case 3:
                sr.sprite = fullHealthSprite;
                break;

            case 2:
                sr.sprite = midHealthSprite;
                break;

            case 1:
                sr.sprite = lowHealthSprite;               
                break;

            default:
                // get global audio and play death sound from it so that I can be destroyed easily
                AudioSource globalAudio = GameObject.FindGameObjectWithTag("global_audio").GetComponent<AudioSource>();
                globalAudio.PlayOneShot(destroySfx);

                paddleScript.score += 90; // 100pts total for destroying
                Debug.Log("score " + paddleScript.score);

                if (isPowerup)
                    Instantiate(powerupType, transform.position, Quaternion.identity);

                Destroy(gameObject);//no lives, gone
                break;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<ballScript>() != null && !effects.b1){
            
            var this_ball = collision.gameObject.GetComponent<ballScript>();
            if (this_ball.type == BallType.green){
                effects.b1 = true;
                effects.b2 = true;// apply acid effect
                var effectOverlay = Instantiate(damageEffectOverlay,
                    transform.position +new Vector3(0,0,-0.1f), Quaternion.identity);// create acid sprite 'on top of' self
                effectOverlay.transform.parent = this.transform;
               // Debug.Log("ACID EFFECT created!!");
            }
            if (this_ball.type == BallType.fire) {
                effects.b1 = true;
                effects.b2 = false;// apply 'fire' effect
                var effectOverlay = Instantiate(damageEffectOverlay, 
                    transform.position +new Vector3(0,0,-0.1f), Quaternion.identity);// create acid sprite 'on top of' self
                effectOverlay.transform.parent = this.transform;
            }
            else {  }
        }
        /*{health -= 1;ballScript.score += 100; Debug.Log("score " + ballScript.score);}*/
        
        audio.PlayOneShot(impactSfx);
        takeDamage();

        //if (health < 1) {  }//gone
    }

    // Start is called before the first frame update
    void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        //brickType = "stone";

        switch (currentType)
        {
            case brickType.stoneBrick:
                health = 3;
                sprites = Resources.LoadAll<Sprite>("Visual/Sprites/stoneBrick");
                break;
            case brickType.mossStoneBrick:
                health = 3;
                sprites = Resources.LoadAll<Sprite>("Visual/Sprites/mossStoneBrick");
                break;
            case brickType.sandBrick:
                health = 2;
                sprites = Resources.LoadAll<Sprite>("Visual/Sprites/SandBrick");
                break;
            case brickType.mossSandBrick:
                health = 2;
                sprites = Resources.LoadAll<Sprite>("Visual/Sprites/mossSandBrick");
                break;
            case brickType.snowBrick:
                health = 2;
                sprites = Resources.LoadAll<Sprite>("Visual/Sprites/snowBrick");
                break;
            case brickType.iceBrick:
                health = 3;
                sprites = Resources.LoadAll<Sprite>("Visual/Sprites/iceBrick");
                break;
            case brickType.metalBrick:
                health = 4;
                sprites = Resources.LoadAll<Sprite>("Visual/Sprites/metalBrick");
                break;
            case brickType.goldBrick:
                health = 4;
                sprites = Resources.LoadAll<Sprite>("Visual/Sprites/goldBrick");
                break;
            default:
                health = 1;
                sprites = Resources.LoadAll<Sprite>("Visual/Sprites/stoneBrick");

                break;
        }


        lowHealthSprite = sprites[sprites.Length - 1];
        midHealthSprite = sprites[sprites.Length - 2];
        fullHealthSprite = sprites[0];//consistent

        sr.sprite = fullHealthSprite;

        audio = GetComponent<AudioSource>();

        effects.b1 = false;// b1 tells if an effect is active or not.
        effects.b2 = false;// b2 indicates if the effect is acid or fire

        if (isPowerup)
        {
            var powerupSpriteOverlay = Instantiate(powerupOverlay,
                    transform.position + new Vector3(0, 0, -0.08f), Quaternion.identity);// create led sprite 'on top of' self
            powerupSpriteOverlay.transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ///half timer = 0;
        //Debug.Log(health);
        if (effects.b1)
        {
            damageEffectTimer += 1;
            if (damageEffectTimer > 250){
                takeDamage();
                damageEffectTimer = 0;
            }//*/
        }

    }


}

