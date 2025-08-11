using UnityEngine;
using UnityEngine.Timeline;

/**
 * Instancie un marker sur le premier rebond de la bullet d'un obstacle
 */
public class BulletMarker : MonoBehaviour
{
    public GameObject redMarker;
    public float scale = 0.25f;
    private static GameObject lastMarker;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si c'est un obstacle
        if (!collision.gameObject.CompareTag("Obstacle") && collision.gameObject.layer == LayerMask.NameToLayer("Target")) return;

        // On détruit le possible dernier marker
        if (lastMarker) Destroy(lastMarker);

        // Alors, on le marque d'un point rouge
        Vector2 hitPoint = collision.GetContact(0).point;
        // Trouver la parent
        RoomData room = Object.FindFirstObjectByType<RoomData>();
        // Instancie le marker comme enfant de l’obstacle
        GameObject marker = Instantiate(redMarker, hitPoint, Quaternion.identity, room.transform);

        marker.transform.localScale = Vector3.one * scale;

        //On le mémorise comme lastMarker
        lastMarker = marker;
        // Et on retire le script de la bullet
        Destroy(this);
    }
}
