using UnityEngine;

public class RainbowText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public Color[] colours;
    int c = 0;

    // Update is called once per frame
    void Update()
    {
        c = ++c % 240;//colours.Length;
        text.color = colours[c/30];//new Color(0.5f, 0.0f, 0.0f);
    }
}
