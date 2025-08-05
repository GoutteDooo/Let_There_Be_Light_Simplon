using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public GameObject currentRoomInstance;
    private int currentRoomIndex = 0;
    private TargetScript[] currentTargets;

    void Start()
    {
        LoadRoom(0); // Début du jeu
    }

    void Update()
    {
        if (currentTargets != null && currentTargets.Length > 0)
        {
            bool allTargetsActive = true;

            foreach (var target in currentTargets)
            {
                if (!target.isTargetActive)
                {
                    allTargetsActive = false;
                    break;
                }
            }

            if (allTargetsActive)
            {
                LoadNextRoom();
            }
        }
    }

    void LoadRoom(int index)
    {
        // Détruire la room actuelle
        if (currentRoomInstance != null)
            Destroy(currentRoomInstance);

        // Créer la nouvelle room (ou la même, ça dépend d'où est appelée la méthode)
        currentRoomInstance = Instantiate(roomPrefabs[index]);

        // Récupère les targets dans cette room
        currentTargets = currentRoomInstance.GetComponentsInChildren<TargetScript>();
        Debug.Log($"Room {index} chargée avec {currentTargets.Length} target(s).");
    }

    public void LoadNextRoom()
    {
        currentRoomIndex++;

        if (currentRoomIndex < roomPrefabs.Length)
        {
            LoadRoom(currentRoomIndex);
        }
        else
        {
            Debug.Log("Fin du jeu !");
        }
    }

    public void RestartLevel()
    {
        LoadRoom(currentRoomIndex);
        Debug.Log($"Room {currentRoomIndex} redémarrée.");
    }
}
