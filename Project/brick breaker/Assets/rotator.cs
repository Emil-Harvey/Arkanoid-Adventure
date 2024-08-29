using UnityEngine;

public class rotator : MonoBehaviour
{
    public float rotationRate;
    public Vector3 rotationAxis;
    public float currentAngle = 0;
    public bool useDeltaTime = true;
    public bool randomiseStartAngle;
    //public bool active;

    // Start is called before the first frame update
    void Start()
    {
        if (randomiseStartAngle)
            currentAngle = Random.Range(0, 360);
        transform.eulerAngles = rotationAxis * currentAngle;
    }

    // Update is called once per frame
    void Update()
    {
        currentAngle += rotationRate * (useDeltaTime ? Time.deltaTime : 1f);
        transform.eulerAngles = rotationAxis * currentAngle;
    }
}
