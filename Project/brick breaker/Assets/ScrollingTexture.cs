using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    public bool scrolling = false;
    //public float scrollX = 1;
    //public float scrollY = 1;
    public Vector2 scrollVector = new Vector2(0,0);
    //SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scrolling)
        {
            transform.position = new Vector3(Mathf.Repeat(Time.time * scrollVector.x, 100f), Mathf.Repeat(Time.time * scrollVector.y, 100f), 10);
            
            //sr.material.mainTextureOffset += new Vector2(scrollx * Time.deltaTime, scrolly * Time.deltaTime);
        }
    }
}
