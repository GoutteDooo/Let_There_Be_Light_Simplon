using UnityEngine;

public class LevelFailScript : MonoBehaviour
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
        if (newState == GameState.Lost)
            menu.SetActive(true);
    }
    public void RestartLevel()
    {
        menu.SetActive(false);
        room.RestartLevel();
    }
}
