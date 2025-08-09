using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GravitronCore : MonoBehaviour
{
    public LayerMask bulletMask;

    [Header("Orbite")]
    [Tooltip("Vitesse angulaire en °/s")]
    public float angularSpeed = 360f;
    [Tooltip("Vitesse de rétrécissement du rayon")]
    public float shrinkSpeed = 3f;
    [Tooltip("Délai après la fin de l’orbite avant destruction")]
    public float destroyDelay = 0.1f;

    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((bulletMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        var rb = other.attachedRigidbody;
        if (rb == null) return;

        // Ajoute (ou récupère) le contrôleur de capture sur la bullet
        var captured = other.GetComponent<GravitronCaptured>();
        if (captured == null) captured = other.gameObject.AddComponent<GravitronCaptured>();

        captured.BeginCapture(transform, rb, angularSpeed, shrinkSpeed, destroyDelay);
    }
}
