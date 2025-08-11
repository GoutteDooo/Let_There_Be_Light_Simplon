using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayermovementScript : MonoBehaviour
{
    public Transform arm;       // The arm whose rotation we check
    public Transform modelRoot; // The main model to flip
    [SerializeField] Animator _animator;
    public GameObject explosion;
    public float explodeOffset = 0.5f;

    private bool flipped = false;
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
        if (newState == GameState.Won)
            _animator.SetTrigger("isWon");
    }

    void Update()
    {
        // Get arm's rotation around its axis.
        float armAngle = arm.localEulerAngles.z;

        // Flip state logic with hysteresis (no jitter near thresholds)
        if (!flipped && (armAngle > 90f && armAngle < 270f))
        {
            flipped = true;
        }
        else if (flipped && (armAngle < 90f || armAngle > 270f))
        {
            flipped = false;
        }

        // Snap instantly to the correct facing
        modelRoot.localEulerAngles = new Vector3(
            modelRoot.localEulerAngles.x,
            flipped ? 180f : 0f,
            modelRoot.localEulerAngles.z
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _animator.SetBool("isHit", true);
        Vector3 position = collision.gameObject.transform.position;
        GameObject fx = Instantiate(explosion, new Vector3(position.x, position.y + explodeOffset, position.z), Quaternion.identity);

        bool impactAGauche = collision.transform.position.x < transform.position.x;

        // Flip si impact à gauche
        if (impactAGauche)
        {
            Vector3 scale = fx.transform.localScale;
            scale.x *= -1; // Inverser horizontalement
            fx.transform.localScale = scale;
        }

    }
}