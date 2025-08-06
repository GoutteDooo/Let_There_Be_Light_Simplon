using UnityEngine;

public class LevelCompleteScript : MonoBehaviour
{
    public GameObject menu;
    public RoomManager room;

    void Start()
    {
        room = GameObject.FindFirstObjectByType<RoomManager>();
    }

    /**
     * Bouton Next
     */
    public void NextLevel()
    {
        menu.SetActive(false);
        room.LoadNextRoom();
    }
    /**
     * Bouton Restart
     */
    public void RestartLevel()
    {
        menu.SetActive(false);
        room.RestartLevel();
    }
}
