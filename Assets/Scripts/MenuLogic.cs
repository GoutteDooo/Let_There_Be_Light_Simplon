using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    public GameObject menu;

    void Start()
    {
        menu.SetActive(true);
    }

    public void StartGame()
    {
        menu.SetActive(false);
        Debug.Log("Game Started");
        SceneManager.LoadScene("Rooms");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Rooms"));
        SceneManager.UnloadSceneAsync("StartScreen");
    }
}
