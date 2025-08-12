using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Easter Egg Activated");
        if (collision.gameObject.GetComponent<BulletController>() != null)
        {
            RoomManager.Instance.currentRoomIndex = RoomManager.easterEggRoomIndex;
            RoomManager.Instance.LoadRoom(RoomManager.Instance.currentRoomIndex); // Singleton call
        }
    }
}