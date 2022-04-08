using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    public bool scrolling = false;
    public float scrollx = 1;
    public float scrolly = 1;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scrolling)
        {
            transform.position = new Vector2(Mathf.Repeat(Time.time * scrollx, 3.2f), Mathf.Repeat(Time.time * scrolly, 3.2f));
            //Sprite s;
            //sr.material.mainTextureOffset += new Vector2(scrollx * Time.deltaTime, scrolly * Time.deltaTime);
        }
    }
}
