using UnityEngine;

public class TurnOnLights : MonoBehaviour
{
    [SerializeField] private GameObject windows;
    [SerializeField] private bool includeInactiveTargets = true;

    private RoomManager roomManager;
    public bool activate = true;

    void Awake()
    {
        // Evite GetComponent chaque frame et récupère la bonne instance
        roomManager = GetComponent<RoomManager>();
        if (roomManager == null)
            roomManager = FindFirstObjectByType<RoomManager>();
    }

    void Update()
    {
        if (roomManager == null) return;

        var currentRoom = roomManager.currentRoomInstance;
        if (currentRoom == null) return;

        // Inclure aussi les enfants désactivés si besoin
        var targets = currentRoom.GetComponentsInChildren<TargetScript>(includeInactiveTargets);
        if (targets.Length == 0)
        {
            // Au choix : ne rien faire, ou forcer l’extinction
            windows.SetActive(activate);
            return;
        }

        // Active la lumière uniquement si TOUTES les cibles sont actives
        bool allActive = activate;
        foreach (var t in targets)
        {
            if (t == null || !t.isTargetActive)
            {
                allActive = !activate;
                break;
            }
        }

        windows.SetActive(allActive);
    }
}
