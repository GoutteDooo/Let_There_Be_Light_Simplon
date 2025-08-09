using UnityEngine;

public class AsteroideRotator : MonoBehaviour
{
    private float rotationSpeed; // deg/sec

    private void Start()
    {
        rotationSpeed = Random.Range(45, 200); // Rotation entre 10 et 90deg par secondes
    }
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
