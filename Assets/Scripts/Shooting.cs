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
    public bool canFire; // to know when player can fire
    private int _bulletLefts; // Stock restant de bullet
    private float _timer;
    public float timeBetweenFiring;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canFire = true;
        _bulletLefts = GameObject.FindFirstObjectByType<RoomData>().bulletStock;
        Debug.Log("Nombre de bullets restantes : " +  _bulletLefts);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!canFire)
        {
            _timer += Time.deltaTime;
            if(_timer > timeBetweenFiring)
            {
                canFire = true;
                _timer = 0;
            }
        } // TODO : Set canFire to true when there is no bullet left in the room

        if (Input.GetMouseButton(0) && canFire)
        {
            RoomManager manager = Object.FindFirstObjectByType<RoomManager>();
            canFire = false;
            if (manager != null && manager.currentRoomInstance != null)
            {
                Instantiate(bullet, bulletTransform.position, Quaternion.identity, manager.currentRoomInstance.transform);
            }
        }
    }
}
