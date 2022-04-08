using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paddleScript : MonoBehaviour
{
    public float speed;
    private float areaWidth = 13.485f;
    // Start is called before the first frame update
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