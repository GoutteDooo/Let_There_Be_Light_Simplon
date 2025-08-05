using UnityEngine;

public class GravitronController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collidingBullet)
    {
        if (collidingBullet.gameObject.layer == 3) // layer 3 est le layer de la bullet
            Destroy(collidingBullet.gameObject);
    }
}
