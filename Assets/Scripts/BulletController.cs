using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector3 _mousePos;
    private Camera _mainCam;
    private Rigidbody2D _rb;
    public float force;
    Vector2 lastVelocity;
    public bool recentlyTeleported = false;
    private float _timer;
    public float livingTime;
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
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // On récupère le composant Rigidbody2D attaché à la balle
        _rb = GetComponent<Rigidbody2D>(); // Access player's Rigidbody
        _mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = _mousePos - transform.position;
        // On applique une force initiale vers le haut et sur le côté
        _rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;

        // TODO : Récupérer le temps de feu à partir du niveau actuel
        // Actuellement, on va set à 15s
        livingTime = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = _rb.linearVelocity;
        _timer += Time.deltaTime;
        // Détruire la bullet après un certain temps
        if (_timer > livingTime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si balle touche joueur, partie perdue
        if (collision.gameObject.CompareTag("Player"))
        {
            GameStateManager.Instance.SetState(GameState.Lost);
        }

        //Debug.Log("La balle a touché : " + collision.gameObject.name);

        // Si elle touche une Target ou le joueur, on la détruit
        if (collision.gameObject.layer == 6 && !collision.gameObject.GetComponent<TargetScript>().isTargetActive || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
