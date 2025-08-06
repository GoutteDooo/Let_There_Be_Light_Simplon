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
    public GameObject bDisplay;

    void Start()
    {
        currentRoomIndex = 0;//DEV
        LoadRoom(currentRoomIndex);//DEV
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
                bDisplay.SetActive(false);
                levelCompleteMenu.SetActive(true);
            }
        }
        // Managing Restart level
        if (NoBulletLefts() && !roomJustLoaded)
        {
            LevelFailScript failMenu = GameObject.FindFirstObjectByType<LevelFailScript>();
            bDisplay.SetActive(false);
            failMenu.menu.SetActive(true);
        }
    }

    void LoadRoom(int index)
    {
        // D�truire la room actuelle
        if (currentRoomInstance != null)
            Destroy(currentRoomInstance);

        // Cr�er la nouvelle room (ou la m�me, �a d�pend d'o� est appel�e la m�thode)
        currentRoomInstance = Instantiate(roomPrefabs[index]);

        failCheckTimer = 0f;
        roomJustLoaded = true;

        BulletsCountdownLogic ui = GameObject.FindFirstObjectByType<BulletsCountdownLogic>();
        if (ui != null)
        {
            ui.RefreshShootingReference();
        }

        bDisplay.SetActive(true);

        // R�cup�re les targets dans cette room
        currentTargets = currentRoomInstance.GetComponentsInChildren<TargetScript>();
        //Debug.Log($"Room {index} charg�e avec {currentTargets.Length} target(s).");

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
            bDisplay.SetActive(false);
            levelCompleteMenu.SetActive(false);
            SceneManager.LoadScene("EndScreen");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("EndScreen"));
            SceneManager.UnloadSceneAsync("Rooms");
        }
    }

    public void RestartLevel()
    {
        LoadRoom(currentRoomIndex);
        Debug.Log($"Room {currentRoomIndex} red�marr�e.");
    }

    /**
     * Retourne true s'il n'y a plus de stock de bullets ni d'instance de Bullet dans la room (et qu'il y'a au moins une target inactive)
     */
    private bool NoBulletLefts()
    {
        // R�cup�rer l'instance de Shooting de la room
        Shooting shooter = Object.FindFirstObjectByType<Shooting>();
        //Debug.Log("RoomManager, Bulletlefts: " + test.HasBulletLefts());
        // V�rifier que le nombre de Bullets dans la room est de 0
        bool isBulletsInRoom = Object.FindFirstObjectByType<BulletController>();
        //Debug.Log(isBulletsInRoom ? "Y'en a" : "Plus de Bullets");
        // V�rifier si au moins une target est inactive
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
