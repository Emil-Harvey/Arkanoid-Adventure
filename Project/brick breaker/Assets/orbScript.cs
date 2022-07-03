using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType { ACID, FIRE, WIDE, SHORT}

public class orbScript : MonoBehaviour
{
    public float speed = 1;
    PowerupType powerupType;

    public AudioClip destroySfx;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("collided with: " + col.gameObject.name);
        /// FIX COLLISION

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
        if (Time.frameCount % 3 == 0) { powerupType = PowerupType.ACID;
        }
        else { powerupType = PowerupType.FIRE; }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.0f, -1 * speed * Time.deltaTime, 0.0f);
    }
}
