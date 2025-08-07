using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision deadzone");
        if (collision.gameObject.GetComponent<BulletController>() != null)
            Destroy(collision.gameObject);
    }
}
