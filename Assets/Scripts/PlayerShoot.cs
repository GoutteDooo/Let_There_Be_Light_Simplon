using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject Bullet;
    public float armLength = 0.8f; // Distance entre le centre du joueur et l�apparition de la balle
    public LayerMask bulletLayer;  // Pour �viter les collisions avec le joueur

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic gauche
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            // Direction du bras ? normalis�e
            Vector2 direction = (mouseWorldPos - transform.position).normalized;

            // Position de spawn de la balle (au bout du bras)
            Vector2 spawnPosition = (Vector2)transform.position + direction * armLength;

            Debug.Log("Clic détecté, tentative d'instancier une bullet");
            // Instanciation de la bullet
            GameObject bullet = Instantiate(Bullet, spawnPosition, Quaternion.identity);

            // Emp�che la bullet de toucher imm�diatement le joueur (si n�cessaire)
            Physics2D.IgnoreCollision(
                bullet.GetComponent<Collider2D>(),
                GetComponent<Collider2D>() // capsule du joueur
            );

            // Appliquer la direction � la bullet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            float speed = bullet.GetComponent<BulletController>().speed;

            rb.linearVelocity = direction * speed;
        }
    }
}
