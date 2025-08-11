using System.Threading;
using UnityEngine;

/**
 * -!- Instanciée en même temps que la room -!-
 * ------------------------------------------
 * S'occupe de la logique de tirs
 * Lorsque le joueur clic, cela déclenche un tir, et instancie une Bullet.
 * La logique de la Bullet se situe dans le script BulletController.cs
 */
public class Shooting : MonoBehaviour
{
    private Camera _mainCam;
    private Vector3 mousePos;
    public GameObject bullet; // bullet that will be instantiated
    public Transform bulletTransform; // gun
    private int _bulletLefts { get; set; } // Stock restant de bullet
    private bool _hasShot; // Pour définir un timing entre les tirs
    private float _timer;
    private readonly float _timeBetweenFiring = 0.3f;
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
        // Si l'état du jeu n'est pas sur "Play", on désactive le drawer
        gameObject.SetActive(newState == GameState.Playing);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _hasShot = false;
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _bulletLefts = GameObject.FindFirstObjectByType<RoomData>().bulletStock;
    }

    // Update is called once per frame
    void Update()
    {
        // Récupère la position du curseur sur l'écran
        mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        // Définit la position du bras
        Vector3 rotation = mousePos - transform.position;
        // Définit la rotation du bras
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        // Set la rotation
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (_hasShot)
        {
            _timer += Time.deltaTime;
            // une fois le timer écoulé, le joueur peut retirer une balle (s'il lui en reste)
            if(_timer > _timeBetweenFiring)
            {
                _timer = 0;
                _hasShot = false;
            }
        }

        /* - TIR DU JOUEUR - */
        if (Input.GetMouseButton(0) && CanFire())
        {
            RoomManager manager = Object.FindFirstObjectByType<RoomManager>();
            if (manager != null && manager.currentRoomInstance != null)
            {
                Instantiate(bullet, bulletTransform.position, Quaternion.identity, manager.currentRoomInstance.transform);
                _bulletLefts -= 1;
                FindFirstObjectByType<BulletsCountdownLogic>().DisplayBullets();
                _hasShot = true; // Lance le timer plus haut
                Object.FindAnyObjectByType<ScreenShake>().Shake(0.2f, 0.1f); // Screenshake
                SFXManager.Instance.PlaySFX("Shoot");
            }
        }
    }

    /**
     * Renvoie true si le joueur peut encore tirer des balles
     */
    private bool CanFire()
    {
        return _bulletLefts > 0 && !_hasShot;
    }
    /**
     * Retourne true si bullets restantes
     */
    public bool HasBulletLefts()
    {
        return _bulletLefts > 0;
    }
    public int GetBulletLefts()
    {
        return _bulletLefts;
    }
}
