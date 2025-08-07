using UnityEditor;
using UnityEngine;

/**
 * Instanciée à chaque nouveau chargement de room
 */
public class MouseLineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public LayerMask obstacleMask;  // Masque de collision pour filtrer les obstacles
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
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 origin = transform.position;
        Vector2 direction = ((Vector2)mouseWorldPos - origin).normalized;

        // Lancer un raycast dans la direction de la souris
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, obstacleMask);

        if (hit.collider != null)
        {
            // Mettre à jour les positions du LineRenderer
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // Si aucun mur touché, ligne jusqu'à une certaine distance
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, origin + direction * 20f); // distance arbitraire
        }
    }
}
