using UnityEngine;

/**
 * Rattaché au GameObject GameManager
 * S'occupe uniquement de secouer la caméra
 */
public class ScreenShake : MonoBehaviour
{
    [Tooltip("Durée du secouement en secondes")]
    public float shakeTime = 0.5f;

    [Tooltip("Intensité du secouement")]
    public float shakeAmount = 0.1f;

    private Transform camTransform;
    private Vector3 originalPos;
    private float currentShakeTime;

    void Start()
    {
        // On suppose que la caméra principale est celle à secouer
        camTransform = Camera.main.transform;
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (currentShakeTime > 0)
        {
            // Position aléatoire autour de la position d'origine
            camTransform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * shakeAmount;

            currentShakeTime -= Time.deltaTime;

            // Fin du shake → on remet la caméra en place
            if (currentShakeTime <= 0)
            {
                camTransform.localPosition = originalPos;
            }
        }
    }

    /// <summary>
    /// Lance l'effet de shake
    /// </summary>
    /// <param name="duration">Durée du shake</param>
    /// <param name="intensity">Intensité du shake</param>
    public void Shake(float duration, float intensity)
    {
        shakeTime = duration;
        shakeAmount = intensity;
        currentShakeTime = duration;    
    }
}
