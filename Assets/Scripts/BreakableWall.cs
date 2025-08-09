using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private bool isBroken = false;

    [Tooltip("Tag de l'objet qui peut casser ce mur")]
    public string bulletTag = "Bullet";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si le mur n'est pas déjà cassé
        if (isBroken) return;

        // Vérifie que l'objet qui entre a bien le tag défini
        if (collision.CompareTag(bulletTag))
        {
            isBroken = true;

            // Détruire le mur
            Destroy(gameObject);
        }
    }
}
