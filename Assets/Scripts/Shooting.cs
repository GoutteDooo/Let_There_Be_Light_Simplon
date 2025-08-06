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
    private int bulletLefts { get; set; } // Stock restant de bullet
    private bool _hasShot; // Pour définir un timing entre les tirs
    private float _timer;
    private readonly float _timeBetweenFiring = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _hasShot = false;
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        bulletLefts = GameObject.FindFirstObjectByType<RoomData>().bulletStock;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.CurrentState != GameState.Playing)
            return;
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

        if (Input.GetMouseButton(0) && CanFire())
        {
            RoomManager manager = Object.FindFirstObjectByType<RoomManager>();
            if (manager != null && manager.currentRoomInstance != null)
            {
                Instantiate(bullet, bulletTransform.position, Quaternion.identity, manager.currentRoomInstance.transform);
                bulletLefts -= 1;
                FindFirstObjectByType<BulletsCountdownLogic>().DisplayBullets();
                _hasShot = true; // Lance le timer plus haut
                Debug.Log("balles restantes : " + bulletLefts);
            }
        }
    }

    /**
     * Renvoie true si le joueur peut encore tirer des balles à partir de _bulletStock;
     */
    private bool CanFire()
    {
        return bulletLefts > 0 && !_hasShot;
    }

    /**
     * Retourne true si bullets restantes
     */
    public bool HasBulletLefts()
    {
        return bulletLefts > 0;
    }
    public int GetBulletLefts()
    {
        return bulletLefts;
    }
}
