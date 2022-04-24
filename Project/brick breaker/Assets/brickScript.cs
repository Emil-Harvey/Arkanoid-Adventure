using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

enum brickType { stoneBrick, mossStoneBrick, sandBrick, mossSandBrick, snowBrick, iceBrick, metalBrick, goldBrick }

public class brickScript : MonoBehaviour
{
    public Sprite[] sprites;
    private Sprite fullHealthSprite;
    private Sprite midHealthSprite;
    private Sprite lowHealthSprite;
    public SpriteRenderer sr;
    private int health;
    [SerializeField]
    private brickType currentType;
    //[SerializeField]
    //private SpriteAtlas atlas;
    //public GameObject ball;
    //public 
    AudioSource audio;
    public AudioClip impactSfx;
    public AudioClip destroySfx;

    private void OnCollisionExit2D(Collision2D collision)
    {
        /*if (collision.gameObject == ball)
        {
            health -= 1;
            ballScript.score += 100;
            Debug.Log("score " + ballScript.score);
        }*/
        health -= 1;
        ballScript.score += 100;
        Debug.Log("score " + ballScript.score);
        switch (health)
        {
            case 3:
                sr.sprite = fullHealthSprite;
                audio.PlayOneShot(impactSfx);
                break;

            case 2:
                sr.sprite = midHealthSprite;
                audio.PlayOneShot(impactSfx);
                break;

            case 1:
                sr.sprite = lowHealthSprite;
                audio.PlayOneShot(destroySfx);
                break;

            default:
                //audio.PlayOneShot(destroySfx); it's too late..
                Destroy(gameObject);//no lives, gone
                break;
        }

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
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(health);


    }


}

