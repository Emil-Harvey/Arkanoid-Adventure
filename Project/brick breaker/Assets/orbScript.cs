using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbScript : MonoBehaviour
{
    public float speed = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided with: " + collision.gameObject.name);
        /// FIX COLLISION

        if (collision.gameObject.name != "paddle")
        {
            return;
        }
        else
        {
            // Activate powerup
            //...
            Destroy(this);
        }
    }

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.0f, -1 * speed * Time.deltaTime, 0.0f);
    }
}
