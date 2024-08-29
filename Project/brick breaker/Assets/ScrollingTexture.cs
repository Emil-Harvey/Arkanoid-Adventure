using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    public bool scrolling = false;
    public Vector2 scrollVector = new Vector2(0,0);

    // Update is called once per frame
    void Update()
    {
        if (!scrolling) return;

        transform.position = new Vector3(Mathf.Repeat(Time.time * scrollVector.x, 100f), Mathf.Repeat(Time.time * scrollVector.y, 100f), 10);
    }
}
