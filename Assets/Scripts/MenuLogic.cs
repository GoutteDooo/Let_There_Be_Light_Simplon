using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    public GameObject menu;

    void Start()
    {
        GameStateManager.Instance.SetState(GameState.Paused);
    }

    public void StartGame()
    {
        menu.SetActive(false);

        // Remplace la scene StartScreen par la scene Rooms
        SceneManager.LoadScene("Rooms");
    }
}
