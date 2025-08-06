using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public GameObject currentRoomInstance;
    private int currentRoomIndex = 0;
    private TargetScript[] currentTargets;
    public GameObject levelCompleteMenu;
    private float failCheckTimer = 0f;
    private bool roomJustLoaded = true;

    void Start()
    {
        currentRoomIndex = 0;//DEV
        LoadRoom(0);//DEV
        //LoadRoom(0); // Début du jeu
    }

    void Update()
    {
        if (roomJustLoaded)
        {
            failCheckTimer += Time.deltaTime;
            if (failCheckTimer >= 0.5f)
            {
                roomJustLoaded = false;
            }
        }

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
                levelCompleteMenu.SetActive(true);
            }
        }
        // Managing Restart level
        if (NoBulletLefts() && !roomJustLoaded)
        {
            LevelFailScript failMenu = GameObject.FindFirstObjectByType<LevelFailScript>();
            failMenu.menu.SetActive(true);
        }
    }

    void LoadRoom(int index)
    {
        // Détruire la room actuelle
        if (currentRoomInstance != null)
            Destroy(currentRoomInstance);

        // Créer la nouvelle room (ou la même, ça dépend d'où est appelée la méthode)
        currentRoomInstance = Instantiate(roomPrefabs[index]);

        failCheckTimer = 0f;
        roomJustLoaded = true;

        // Récupère les targets dans cette room
        currentTargets = currentRoomInstance.GetComponentsInChildren<TargetScript>();
        //Debug.Log($"Room {index} chargée avec {currentTargets.Length} target(s).");

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
            levelCompleteMenu.SetActive(false);
            SceneManager.LoadScene("EndScreen");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("EndScreen"));
            SceneManager.UnloadSceneAsync("Rooms");
        }
    }

    public void RestartLevel()
    {
        LoadRoom(currentRoomIndex);
        Debug.Log($"Room {currentRoomIndex} redémarrée.");
    }

    /**
     * Retourne true s'il n'y a plus de stock de bullets ni d'instance de Bullet dans la room (et qu'il y'a au moins une target inactive)
     */
    private bool NoBulletLefts()
    {
        // Récupérer l'instance de Shooting de la room
        Shooting shooter = Object.FindFirstObjectByType<Shooting>();
        //Debug.Log("RoomManager, Bulletlefts: " + test.HasBulletLefts());
        // Vérifier que le nombre de Bullets dans la room est de 0
        bool isBulletsInRoom = Object.FindFirstObjectByType<BulletController>();
        //Debug.Log(isBulletsInRoom ? "Y'en a" : "Plus de Bullets");
        // Vérifier si au moins une target est inactive
        bool allTargetsActive = true;
        foreach(var target in currentTargets)
        {
            if (!target.isTargetActive)
            {
                allTargetsActive = false;
                break;
            }
        }
        return !shooter.HasBulletLefts() && !isBulletsInRoom && !allTargetsActive;
    }
}
