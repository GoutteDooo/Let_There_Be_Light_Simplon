using UnityEngine;

public class MouseLineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer; // Ligne dynamique
    public LayerMask obstacleMask;
    public GameObject linePrefab; // Prefab contenant juste un LineRenderer configuré

    private static GameObject lastLine;

    void Update()
    {
        // Position souris en monde
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 origin = transform.position;
        Vector2 direction = ((Vector2)mouseWorldPos - origin).normalized;

        // Raycast vers obstacle
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, obstacleMask);

        // Met à jour la preview
        Vector3 endPos = hit.collider != null ? (Vector3)hit.point : origin + direction * 20f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, endPos);

        // Clic = figer la ligne
        if (Input.GetMouseButtonDown(0))
        {
            if (lastLine != null) Destroy(lastLine);

            // Récupérer la room
            RoomData room = Object.FindFirstObjectByType<RoomData>();
            if (room == null) return;
            // Instancier une nouvelle ligne et copier les positions
            GameObject newLine = Instantiate(linePrefab, room.transform);
            LineRenderer lr = newLine.GetComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, origin);
            lr.SetPosition(1, endPos);

            lastLine = newLine;
        }
    }
}
