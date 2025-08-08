using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PortalLogic2D : MonoBehaviour
{
    [Header("Lien & options")]
    public PortalLogic2D linkedPortal;
    [Tooltip("Sym�trie locale sur l'axe X du portail (miroir horizontal).")]
    public bool invertLocalX = false;
    [Tooltip("Sym�trie locale sur l'axe Y du portail (miroir vertical).")]
    public bool invertLocalY = false;

    [Header("Sortie")]
    [Tooltip("D�calage pour �jecter la balle en sortie (dans l'axe choisi).")]
    public float exitPush = 0.3f;
    public ExitAxis forwardAxis = ExitAxis.Right; // Right ou Up selon ton prefab

    [Header("S�curit�")]
    [Tooltip("Dur�e pendant laquelle on ignore les collisions avec les 2 portails apr�s TP.")]
    public float ignoreTime = 0.1f;

    private Collider2D portalCol;

    public enum ExitAxis { Right, Up }

    private void Awake()
    {
        portalCol = GetComponent<Collider2D>();
        if (linkedPortal == null)
            Debug.LogWarning($"[{name}] linkedPortal n'est pas assign�.");
    }

    // Supporte les deux modes :
    private void OnCollisionEnter2D(Collision2D collision) => TryTeleport(collision.collider, collision);
    private void OnTriggerEnter2D(Collider2D other) => TryTeleport(other, null);

    private void TryTeleport(Collider2D other, Collision2D collision)
    {
        if (!other || !other.CompareTag("Bullet") || linkedPortal == null) return;

        var rb = other.attachedRigidbody;
        var bullet = other.GetComponent<BulletController>();
        if (rb == null || bullet == null || bullet.recentlyTeleported) return;

        // 1) Point de contact (plus pr�cis que le center)
        Vector2 contactWorld = other.transform.position;
        if (collision != null && collision.contactCount > 0)
            contactWorld = collision.GetContact(0).point; // premier point de contact (Unity API)

        // 2) Position & vitesse en local du portail d'entr�e
        Vector2 localPos = transform.InverseTransformPoint(contactWorld);           // position locale
        Vector2 localVel = transform.InverseTransformDirection(rb.linearVelocity);       // direction locale

        // 3) Miroirs �ventuels
        if (invertLocalX) { localPos.x = -localPos.x; localVel.x = -localVel.x; }
        if (invertLocalY) { localPos.y = -localPos.y; localVel.y = -localVel.y; }

        // 4) Reprojection dans l'espace du portail de sortie
        Vector2 exitWorldPos = linkedPortal.transform.TransformPoint(localPos);
        Vector2 exitWorldVel = linkedPortal.transform.TransformDirection(localVel);

        // 5) Appliquer la t�l�portation proprement c�t� physique
        Vector2 forward = (linkedPortal.forwardAxis == ExitAxis.Right)
            ? (Vector2)linkedPortal.transform.right
            : (Vector2)linkedPortal.transform.up;

        rb.position = exitWorldPos + forward * exitPush;  // t�l�portation nette (physique 2D)
        rb.linearVelocity = exitWorldVel;                       // conserve l��nergie/direction adapt�e

        // 6) Emp�cher la re-t�l�portation imm�diate
        bullet.recentlyTeleported = true;
        StartCoroutine(ResetTeleportFlag(bullet, ignoreTime));

        // Ignorer collisions avec les 2 portails pendant un bref instant
        var otherCol = other;                    // collider de la balle
        var exitCol = linkedPortal.portalCol;   // collider du portail de sortie
        StartCoroutine(TemporarilyIgnore(otherCol, portalCol, exitCol, ignoreTime));
    }

    private IEnumerator TemporarilyIgnore(Collider2D bulletCol, Collider2D entryCol, Collider2D exitCol, float time)
    {
        if (bulletCol != null && entryCol != null) Physics2D.IgnoreCollision(bulletCol, entryCol, true);
        if (bulletCol != null && exitCol != null) Physics2D.IgnoreCollision(bulletCol, exitCol, true);

        yield return new WaitForSeconds(time);

        if (bulletCol != null && entryCol != null) Physics2D.IgnoreCollision(bulletCol, entryCol, false);
        if (bulletCol != null && exitCol != null) Physics2D.IgnoreCollision(bulletCol, exitCol, false);
    }

    private IEnumerator ResetTeleportFlag(BulletController bullet, float time)
    {
        yield return new WaitForSeconds(time);
        if (bullet) bullet.recentlyTeleported = false;
    }
}
