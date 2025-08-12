using UnityEngine;
using UnityEngine.InputSystem;
public class QuitGameScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
