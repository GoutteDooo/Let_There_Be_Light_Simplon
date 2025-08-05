using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] rooms; // tableau contenant toutes les rooms
    private int currentRoomIndex = 0; // index permettant de savoir où l'on se situe dans le jeu

    void Start()
    {
        // Désactiver toutes les rooms sauf la première (au cas où ça n'a pas déjà été fait)
        for (int i = 0; i < rooms.Length; i++)
            rooms[i].SetActive(i == currentRoomIndex);
    }

    public void LoadNextRoom()
    {
        // Désactiver l’actuelle
        rooms[currentRoomIndex].SetActive(false);

        // Incrémenter l’index
        currentRoomIndex++;

        if (currentRoomIndex < rooms.Length)
        {
            // Activer la suivante
            rooms[currentRoomIndex].SetActive(true);
        }
        else
        {
            Debug.Log("Fin du jeu ou pas d'autres rooms !");
            // Tu peux afficher un menu de fin ou relancer le jeu
        }
    }
}
