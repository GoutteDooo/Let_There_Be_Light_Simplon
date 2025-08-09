using UnityEngine;

public class SlowMotionToggle : MonoBehaviour
{
    [Tooltip("Vitesse du temps quand ralenti (0.5 = moitié de la vitesse normale)")]
    public float slowMotionScale = 0.3f;

    [Tooltip("Vitesse du temps normale (par défaut = 1)")]
    public float normalTimeScale = 1f;

    private bool isSlowMotion = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isSlowMotion)
            {
                Time.timeScale = normalTimeScale;
                Time.fixedDeltaTime = 0.02f; // valeur par défaut
                isSlowMotion = false;
            }
            else
            {
                Time.timeScale = slowMotionScale;
                Time.fixedDeltaTime = 0.02f * slowMotionScale; // adapter la physique
                isSlowMotion = true;
            }
        }
    }
}
