using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PortalLogic : MonoBehaviour
{
    public GameObject linkedPortal;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 && linkedPortal != null)
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (bullet != null && rb != null && !bullet.recentlyTeleported)
            {
                // --- 1. Calcul de la rotation relative
                float inAngle = transform.eulerAngles.z;
                float outAngle = linkedPortal.transform.eulerAngles.z;
                float deltaAngle = outAngle - inAngle;
                float angleRad = deltaAngle * Mathf.Deg2Rad;

                // --- 2. Rotation de la vélocité
                Vector2 currentVelocity = rb.linearVelocity;
                Vector2 newVelocity = new Vector2(
                    currentVelocity.x * Mathf.Cos(angleRad) - currentVelocity.y * Mathf.Sin(angleRad),
                    currentVelocity.x * Mathf.Sin(angleRad) + currentVelocity.y * Mathf.Cos(angleRad)
                );
                rb.linearVelocity = newVelocity;

                // --- 3. Position relative d'entrée
                Vector2 localOffset = transform.InverseTransformPoint(collision.transform.position);
                Vector2 worldExitPosition = linkedPortal.transform.TransformPoint(localOffset);

                // --- 4. Placement bullet + légère avancée
                Vector2 exitDirection = linkedPortal.transform.right; // ou .up selon ton prefab
                collision.transform.position = worldExitPosition + exitDirection * 0.3f;

                // --- 5. Anti-téléportation infinie
                bullet.recentlyTeleported = true;
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