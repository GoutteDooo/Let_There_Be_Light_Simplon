using UnityEngine;

public class LevelCompleteScript : MonoBehaviour
{
    public GameObject menu;
    public RoomManager room;

    void Start()
    {
        room = GameObject.FindFirstObjectByType<RoomManager>();
    }
    void OnEnable()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Won)
            menu.SetActive(true);
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
