using UnityEngine;

/**
 * Instanciée lorsque le joueur tire
 * Rattaché à la bullet
 */
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
    public GameObject bounceFX;
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

        SFXManager.Instance.PlayLoopSFX("Electricite");

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
            SFXManager.Instance.PlaySFX("BulletOutSparks");
            SFXManager.Instance.PlaySFX("BulletOut");
            SFXManager.Instance.StopAllLoopSFX();
            Destroy(gameObject);
        }
    }

    // Rebond sur n'importe quelle surface
    void OnCollisionEnter2D(Collision2D collision)
    {
        Object.FindAnyObjectByType<ScreenShake>().Shake(0.1f, 0.04f); // Screenshake

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            /* -- PFXs -- */
            /* ---------- */
            if (bounceFX != null)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    BulletBouncePFX.Spawn(bounceFX, collision);
                }
            }

            /* -- SFX -- */
            /* --------- */
            if (collision.gameObject.layer == LayerMask.NameToLayer("Breakable"))
                SFXManager.Instance.PlaySFX("BulletBounceBreakable");
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                if (!collision.gameObject.GetComponent<TargetScript>().isTargetActive)
                {
                    SFXManager.Instance.PlaySFX("BulletTarget");
                    SFXManager.Instance.PlaySFX("TargetSparks");
                }
                else
                    SFXManager.Instance.PlaySFX("BulletBounce");
            }
            else
                SFXManager.Instance.PlaySFX("BulletBounce");
        }


        // Si balle touche joueur, partie perdue
        if (collision.gameObject.CompareTag("Player"))
        {
            GameStateManager.Instance.SetState(GameState.Lost);
            SFXManager.Instance.PlaySFX("BulletOutSparks");
            SFXManager.Instance.PlaySFX("BulletOut");
            SFXManager.Instance.PlaySFX("Death");
        }

        //Debug.Log("La balle a touché : " + collision.gameObject.name);

        // Si elle touche une Target ou le joueur, on la détruit
        if (collision.gameObject.layer == 6 && !collision.gameObject.GetComponent<TargetScript>().isTargetActive || collision.gameObject.CompareTag("Player"))
        {
            SFXManager.Instance.StopAllLoopSFX();
            Destroy(gameObject);
        }
    }
}
