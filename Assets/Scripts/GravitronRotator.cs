using UnityEngine;

public class GravitronRotator : MonoBehaviour
{
    public float rotationSpeed = -90f; // deg/sec

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
