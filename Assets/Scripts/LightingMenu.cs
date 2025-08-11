using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LightingMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("Effet Néon")]
    [Tooltip("Alpha minimum du scintillement")]
    public float minAlpha = 0.8f;
    [Tooltip("Alpha maximum du scintillement")]
    public float maxAlpha = 1f;
    [Tooltip("Vitesse principale du scintillement")]
    public float pulseSpeed = 2f;
    [Tooltip("Intensité des variations aléatoires")]
    public float flickerIntensity = 0.05f;
    [Tooltip("Vitesse du bruit aléatoire")]
    public float flickerSpeed = 15f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        // Effet pulsation (sinus)
        float pulse = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2);

        // Ajout d'un petit bruit aléatoire rapide
        float flicker = (Mathf.PerlinNoise(Time.time * flickerSpeed, 0) - 0.5f) * flickerIntensity * 2;

        // Application au CanvasGroup
        canvasGroup.alpha = Mathf.Clamp01(pulse + flicker);
    }
}
