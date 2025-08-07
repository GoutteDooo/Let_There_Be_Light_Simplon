using UnityEngine;

public class PortalLogic : MonoBehaviour
{
    public GameObject linkedPortal;

    [Tooltip("Si activé, la bullet ressortira en position inversée (symétrie verticale) par rapport à son entrée.")]
    public bool invertExitOffset = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && linkedPortal != null)
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (bullet != null && rb != null && !bullet.recentlyTeleported)
            {
                // --- 1. Rotation de la vélocité
                float inAngle = transform.eulerAngles.z;
                float outAngle = linkedPortal.transform.eulerAngles.z;
                float deltaAngle = outAngle - inAngle;
                float angleRad = deltaAngle * Mathf.Deg2Rad;

                Vector2 currentVelocity = rb.linearVelocity;
                Vector2 newVelocity = new Vector2(
                    currentVelocity.x * Mathf.Cos(angleRad) - currentVelocity.y * Mathf.Sin(angleRad),
                    currentVelocity.x * Mathf.Sin(angleRad) + currentVelocity.y * Mathf.Cos(angleRad)
                );
                rb.linearVelocity = newVelocity;

                // --- 2. Position relative dans le portail d’entrée
                Vector2 localOffset = transform.InverseTransformPoint(collision.transform.position);

                // --- 3. Si besoin, inverser la position locale verticalement
                if (invertExitOffset)
                {
                    localOffset.x = -localOffset.x;
                }

                // --- 4. Convertir cette position locale dans le portail de sortie
                Vector2 worldExitPosition = linkedPortal.transform.TransformPoint(localOffset);

                // --- 5. Placement final légèrement en avant du portail
                Vector2 exitDirection = linkedPortal.transform.right; // ou .up selon le prefab
                collision.transform.position = worldExitPosition + exitDirection * 0.3f;

                // --- 6. Empêche téléportation immédiate
                bullet.recentlyTeleported = true;
                linkedPortal.GetComponent<MonoBehaviour>().StartCoroutine(ResetTeleportFlag(bullet));
            }

        }
    }

    private System.Collections.IEnumerator ResetTeleportFlag(BulletController bullet)
    {
        yield return new WaitForSeconds(0.1f);
        bullet.recentlyTeleported = false;
    }
}
