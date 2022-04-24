//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class RainbowText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public Color[] colours;
    int c = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        c = (c+1) % 240;//colours.Length;
        text.color = colours[c/30];//new Color(0.5f, 0.0f, 0.0f);
    }
}
