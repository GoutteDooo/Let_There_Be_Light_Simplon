using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Easter Egg Activated");
        if (collision.gameObject.GetComponent<BulletController>() != null)
        {
            RoomManager.Instance.LoadRoom(14); // Singleton call
        }
    }
}
