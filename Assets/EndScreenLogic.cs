using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScreenLogic : MonoBehaviour
{
    public void GoHome()
    {
        SceneManager.LoadScene("StartScreen");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartScreen"));
        SceneManager.UnloadSceneAsync("EndScreen");
    }
}
