using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType { ACID, FIRE, WIDE, SHORT, REVERSE, SLOW}

public class orbScript : MonoBehaviour
{
    public float speed = 1;
    public PowerupType powerupType = (PowerupType)(-1);

    public AudioClip destroySfx;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("collided with: " + col.gameObject.name);
        
        if (col.gameObject.name != "paddle")
        {
            return;
        }
        else
        {
            // Activate powerup
            var paddle = FindObjectOfType<paddleScript>();
            paddle.OnPowerupGet(powerupType);

            // get global audio and play death sound from it so that I can be destroyed at the same time
            AudioSource globalAudio = GameObject.FindGameObjectWithTag("global_audio").GetComponent<AudioSource>();
            globalAudio.PlayOneShot(destroySfx);
            //...
            Destroy(gameObject);
        }
    }

        // Start is called before the first frame update
    void Start()
    {
        if (powerupType == (PowerupType)(-1)) // assign a random powerup if none has been determined
        {
            switch(Time.frameCount % 6)
            {
                case 0:
                    powerupType = PowerupType.ACID;
                    break;
                case 1:
                    powerupType = PowerupType.FIRE;
                    break;
                case 2:
                    powerupType = PowerupType.SHORT;
                    break;
                case 3:
                    powerupType = PowerupType.WIDE;
                    break;
                case 4:
                    powerupType = PowerupType.SLOW;
                    break;
                case 5:
                    powerupType = PowerupType.REVERSE;
                    break;
                default:
                    powerupType = PowerupType.REVERSE;
                    break;
            }
            
        }

        //transform.Translate(0,0,-4);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.0f, -1 * speed * Time.deltaTime, 0.0f);
    }
}
