using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PortalLogic : MonoBehaviour
{
    public GameObject linkedPortal;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Checks if the object that collided is on the 3rd layer and that a portal is linked
        if (collision.gameObject.layer == 3 && linkedPortal != null)
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (bullet != null && rb != null && !bullet.recentlyTeleported)
            {
                // Teleport the bullet
                collision.transform.position = linkedPortal.transform.position;

                // Rotate velocity based on linked portal's Z rotation
                Vector2 currentVelocity = rb.linearVelocity;
                float angle = linkedPortal.transform.eulerAngles.z;
                float angleRad = angle * Mathf.Deg2Rad;

                Vector2 newVelocity = new Vector2(
                    currentVelocity.x * Mathf.Cos(angleRad) - currentVelocity.y * Mathf.Sin(angleRad),
                    currentVelocity.x * Mathf.Sin(angleRad) + currentVelocity.y * Mathf.Cos(angleRad)
                );

                rb.linearVelocity = newVelocity;

                // Set teleport flag
                bullet.recentlyTeleported = true;

                // Reset the flag after a short delay
                linkedPortal.GetComponent<MonoBehaviour>().StartCoroutine(ResetTeleportFlag(bullet));
            }
        }
    }

    private System.Collections.IEnumerator ResetTeleportFlag(BulletController bullet)
    {
        // Set a delay before being able to teleport again, prevents infinite tp
        yield return new WaitForSeconds(0.5f);
        bullet.recentlyTeleported = false;
    }
}