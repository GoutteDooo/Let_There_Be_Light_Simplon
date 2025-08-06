using UnityEngine;

public class LevelFailScript : MonoBehaviour
{
    public GameObject menu;
    public RoomManager room;

    void Start()
    {
        room = GameObject.FindFirstObjectByType<RoomManager>();
    }
    public void RestartLevel()
    {
        menu.SetActive(false);
        room.RestartLevel();
    }
}
