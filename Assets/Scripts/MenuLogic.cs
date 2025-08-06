using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    public GameObject startScreen;

    void Start()
    {
        startScreen.SetActive(true);
    }

    public void StartGame()
    {
        startScreen.SetActive(false);
        Debug.Log("Game Started");
        SceneManager.LoadScene("Rooms");
    }
}
