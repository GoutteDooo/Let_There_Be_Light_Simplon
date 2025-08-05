using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 0.5f;
    Vector2 lastVelocity;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // On récupère le composant Rigidbody2D attaché à la balle
        rb = GetComponent<Rigidbody2D>(); // Access player's Rigidbody.

        // On applique une force initiale vers le haut et sur le côté
        rb.AddForce(new Vector2(2f, 2f), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.linearVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("La balle a touché : " + collision.gameObject.name);
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        rb.linearVelocity = direction * Mathf.Max(speed, 0f);
    }
}
