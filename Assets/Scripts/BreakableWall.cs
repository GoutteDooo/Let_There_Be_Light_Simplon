using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class BreakableWall : MonoBehaviour
{
    [Tooltip("Tag de l'objet qui casse le mur")]
    public string bulletTag = "Bullet";

    [Tooltip("Délai optionnel après le rebond avant destruction")]
    public float destroyDelay = 0.1f;

    [Tooltip("Désactive le collider tout de suite pour éviter un 2e contact")]
    public bool disableColliderImmediately = true;

    private bool isBroken;

    void Reset()
    {
        // On veut un mur "solide", donc pas de trigger
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken) return;
        if (!collision.collider.CompareTag(bulletTag)) return;

        isBroken = true;

        if (disableColliderImmediately)
        {
            var col = GetComponent<Collider2D>();
            if (col) col.enabled = false; // évite les doubles collisions la frame suivante
        }

        // Laisse la physique finir le rebond, puis casse le mur
        StartCoroutine(BreakAfterBounce());
    }

    private IEnumerator BreakAfterBounce()
    {
        // Attend la fin de la prochaine étape de physique
        yield return new WaitForFixedUpdate(); // la résolution des collisions survient autour de FixedUpdate
        if (destroyDelay > 0f)
            yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}
