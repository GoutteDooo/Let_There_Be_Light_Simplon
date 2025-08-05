using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector3 _mousePos;
    private Camera _mainCam;
    private Rigidbody2D _rb;
    public float force;
    public float speed = 5f;
    Vector2 lastVelocity;
    public bool recentlyTeleported = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // On récupère le composant Rigidbody2D attaché à la balle
        _rb = GetComponent<Rigidbody2D>(); // Access player's Rigidbody
        _mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = _mousePos - transform.position;
        // On applique une force initiale vers le haut et sur le côté
        _rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = _rb.linearVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("La balle a touché : " + collision.gameObject.name);
        var speed = lastVelocity.magnitude;
        var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        _rb.linearVelocity = direction * Mathf.Max(speed, 0f);

        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
