using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class TargetScript : MonoBehaviour
{
    public bool isTargetActive = false;
    public GameObject Inactive;
    public GameObject Active;
    public GameObject targetHitAnimation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Inactive.SetActive(true);
        Active.SetActive(false);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 && !isTargetActive)
        {
            Object.FindAnyObjectByType<ScreenShake>().Shake(0.2f, 0.06f); // Screenshake

            isTargetActive = true;
            Inactive.SetActive(false);
            Active.SetActive(true);
            Debug.Log("Target is active");

            // Particules - Electricité
            GameObject pfx = Instantiate(targetHitAnimation, this.transform);
            pfx.transform.position = this.transform.position;


        }
    }
}
