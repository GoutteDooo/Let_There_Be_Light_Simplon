using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    void Update()
    {
          QuitGame();            
    }

    public void QuitGame()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop Play Mode in Editor
#else
                        Application.Quit(); // Close game in build
#endif
            Debug.Log("Quit Game");
        }
    }
}
