using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GravitronController : MonoBehaviour
{
    [Header("Ciblage")]
    public LayerMask bulletMask;
    [Tooltip("Transform du coeur (boule rose) pour en déduire le rayon intérieur")]
    public Transform core;

    [Header("Gravité")]
    public float gravityStrength = 20f;
    [Range(0.5f, 3f)] public float falloff = 2f; // 1/r^falloff
    [Tooltip("Rayon d'exclusion au centre (si core non fourni)")]
    public float innerExclusionRadius = 0.3f;

    float innerRadiusSqr;

    void Awake()
    {
        // S’assure que le collider est un trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // Si on a le coeur, on prend son rayon réel comme exclusion
        if (core != null)
        {
            var cc = core.GetComponent<CircleCollider2D>();
            if (cc != null)
                innerExclusionRadius = Mathf.Max(innerExclusionRadius, cc.radius * Mathf.Max(core.lossyScale.x, core.lossyScale.y));
        }
        innerRadiusSqr = innerExclusionRadius * innerExclusionRadius;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Filtre Layer
        if ((bulletMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        // Si déjà capturée, on n'applique plus de force
        var captured = other.GetComponent<GravitronCaptured>();
        if (captured != null && captured.IsCaptured)
            return;

        var rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector2 center = transform.position;
        Vector2 pos = rb.position;
        Vector2 toCenter = center - pos;

        // Stopper l'attraction dans la "zone coeur"
        float distSqr = toCenter.sqrMagnitude;
        if (distSqr <= innerRadiusSqr)
            return;

        float r = Mathf.Sqrt(distSqr);
        float forceMag = gravityStrength / Mathf.Pow(r, falloff);
        rb.AddForce(toCenter.normalized * forceMag, ForceMode2D.Force);
    }
}
