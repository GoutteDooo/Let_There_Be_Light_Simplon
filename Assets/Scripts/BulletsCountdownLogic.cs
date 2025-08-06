using UnityEngine;
using UnityEngine.UI;

public class BulletsCountdownLogic : MonoBehaviour
{
    public Text bulletsInGun;
    private Shooting shooting;

    void Start()
    {
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

