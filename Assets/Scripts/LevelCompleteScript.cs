using UnityEngine;

public class LevelCompleteScript : MonoBehaviour
{
    public GameObject menu;
    public RoomManager room;

    void Start()
    {
        room = GameObject.FindFirstObjectByType<RoomManager>();
    }

    public void NextLevel()
    {
        menu.SetActive(false);
        room.LoadNextRoom();
    }
    public void RestartLevel()
    {
        menu.SetActive(false);
        room.RestartLevel();
    }
}
