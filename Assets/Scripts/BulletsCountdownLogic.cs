using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BulletsCountdownLogic : MonoBehaviour
{
    public Text bulletsInGun;
    private Shooting shooting;

    void Start()
    {
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
        if (newState != GameState.Playing)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    void Update()
    {
        DisplayBullets();
    }

    public void DisplayBullets()
    {
        if (shooting != null)
        {
            bulletsInGun.text = shooting.GetBulletLefts().ToString();
        }
    }
    public void RefreshShootingReference()
    {
        shooting = GameObject.FindFirstObjectByType<Shooting>();
    }
}

