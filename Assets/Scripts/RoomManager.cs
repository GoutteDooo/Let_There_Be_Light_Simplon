using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] rooms;
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
        // Désactive toutes les rooms sauf celle à l’index
        for (int i = 0; i < rooms.Length; i++)
            rooms[i].SetActive(i == index);

        currentRoomIndex = index;

        // Récupère les targets dans cette room
        currentTargets = rooms[currentRoomIndex].GetComponentsInChildren<TargetScript>();
        Debug.Log($"Room {index} chargée avec {currentTargets.Length} target(s).");
    }

    public void LoadNextRoom()
    {
        // Désactiver l’actuelle
        rooms[currentRoomIndex].SetActive(false);
        currentRoomIndex++;

        if (currentRoomIndex < rooms.Length)
        {
            LoadRoom(currentRoomIndex);
        }
        else
        {
            Debug.Log("Fin du jeu !");
        }
    }
}
