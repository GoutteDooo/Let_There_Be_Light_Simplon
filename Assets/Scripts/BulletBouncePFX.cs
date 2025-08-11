using UnityEngine;

[DisallowMultipleComponent]
public class BulletBouncePFX : MonoBehaviour
{
    [Header("Réglages Visuels")]
    [Tooltip("Échelle multiplicative appliquée au FX.")]
    public float fxScale = 1f;

    [Tooltip("Décalage le long de la normale pour éviter d'entrer dans la surface.")]
    public float spawnOffset = 0.02f;

    [Tooltip("Décalage d'angle si ton sprite 'regarde' une autre direction par défaut (souvent -90° si ton sprite regarde à droite).")]
    public float angleOffset = 0f;

    [Header("Cycle de vie")]
    [Tooltip("Si vrai, détruit le FX une fois l’animation terminée.")]
    public bool autoDestroyOnAnimEnd = true;

    [Tooltip("Durée de vie forcée (écrase l'auto-détection). <= 0 pour auto.")]
    public float lifeTimeOverride = -1f;

    private Animator _animator;
    private bool _initialized;

    // ---------- API PUBLIQUE : SPAWN DEPUIS UNE COLLISION ----------
    public static BulletBouncePFX Spawn(GameObject prefab, Collision2D collision)
    {
        if (prefab == null || collision == null) return null;

        // 1) Récup contacts
        var contactsBuffer = new ContactPoint2D[8];
        int count = collision.GetContacts(contactsBuffer);
        if (count <= 0) return null;

        Vector2 n = Vector2.zero;
        Vector2 p = Vector2.zero;
        for (int i = 0; i < count; i++)
        {
            n += contactsBuffer[i].normal;
            p += contactsBuffer[i].point;
        }
        n.Normalize();
        p /= Mathf.Max(1, count);

        // 2) Calcul rotation
        float angleOffset = prefab.GetComponent<BulletBouncePFX>()?.angleOffset ?? 0f;
        float angle = Vector2.SignedAngle(Vector2.up, n) + angleOffset;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        // 3) Position avec décalage
        float spawnOffset = prefab.GetComponent<BulletBouncePFX>()?.spawnOffset ?? 0f;
        Vector3 spawnPos = (Vector3)p + (Vector3)(n * spawnOffset);

        // 4) Instanciation
        var go = Object.Instantiate(prefab, spawnPos, rot);
        var pfx = go.GetComponent<BulletBouncePFX>();
        pfx._initialized = true;
        pfx?.SetupLifecycle();

        return pfx;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Ajuste l’échelle
        transform.localScale = Vector3.one * fxScale;

        // Si instancié manuellement
        if (!_initialized)
        {
            SetupLifecycle();
        }
    }

    // ---------- AUTODESTRUCTION ----------
    private void SetupLifecycle()
    {
        if (!autoDestroyOnAnimEnd) return;

        float lifetime = lifeTimeOverride > 0f ? lifeTimeOverride : GetAnimationLengthSafe();
        if (lifetime > 0f)
        {
            Destroy(gameObject, lifetime);
        }
    }

    private float GetAnimationLengthSafe()
    {
        if (_animator != null)
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.length > 0f && state.normalizedTime < 0.1f)
                return state.length;

            var rac = _animator.runtimeAnimatorController;
            if (rac != null && rac.animationClips != null && rac.animationClips.Length > 0)
            {
                float maxLen = 0f;
                foreach (var clip in rac.animationClips)
                    if (clip && clip.length > maxLen) maxLen = clip.length;
                if (maxLen > 0f) return maxLen;
            }
        }
        return 0.5f; // fallback
    }
}
