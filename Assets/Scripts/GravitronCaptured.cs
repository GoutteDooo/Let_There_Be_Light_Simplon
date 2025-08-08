using System.Collections;
using UnityEngine;

public class GravitronCaptured : MonoBehaviour
{
    public bool IsCaptured { get; private set; }

    public void BeginCapture(Transform center, Rigidbody2D rb, float angularSpeed, float shrinkSpeed, float destroyDelay)
    {
        if (IsCaptured) return;
        IsCaptured = true;
        StartCoroutine(CaptureRoutine(center, rb, angularSpeed, shrinkSpeed, destroyDelay));
    }

    IEnumerator CaptureRoutine(Transform center, Rigidbody2D rb, float angularSpeed, float shrinkSpeed, float destroyDelay)
    {
        // On "retire" la bullet de la physique
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true; // on prend le contr�le du transform

        // Option fort : couper le collider pour ne plus g�n�rer d'�v�nements
        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Vector2 c = center.position;
        Vector2 p = transform.position;
        Vector2 offset = p - c;
        float radius = offset.magnitude;
        float angle = Mathf.Atan2(offset.y, offset.x);

        while (radius > 0.02f)
        {
            // Avance l�angle (orbite) et rapproche le rayon (spirale vers le centre)
            angle += angularSpeed * Mathf.Deg2Rad * Time.deltaTime;
            radius = Mathf.MoveTowards(radius, 0f, shrinkSpeed * Time.deltaTime);

            Vector2 pos = (Vector2)c + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            transform.position = pos;

            yield return null;
        }

        if (destroyDelay > 0f) yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
