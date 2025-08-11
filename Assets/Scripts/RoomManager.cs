using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public GameObject[] roomPrefabs;
    public GameObject currentRoomInstance;
    public int currentRoomIndex = 0;
    private TargetScript[] currentTargets;
    public GameObject levelCompleteMenu;
    private float failCheckTimer = 0f;
    private bool roomJustLoaded = true;
    public GameObject bDisplay;

    private void Awake()
    {
        // Simple singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        currentRoomIndex = 0;//DEV
        LoadRoom(currentRoomIndex);//DEV
        //LoadRoom(0); // Début du jeu
    }

    void Update()
    {
        if (GameStateManager.Instance.CurrentState != GameState.Playing)
            return;
        /* -- FOR DEVELOPMENT -- */
        // Permet de passer au niveau suivant

        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextRoom();
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentRoomIndex--;
            RestartLevel();
        }

        // Léger timing pour interrompre l'update lors de la transition de room
        if (roomJustLoaded)
        {
            failCheckTimer += Time.deltaTime;
            if (failCheckTimer >= 0.5f)
            {
                roomJustLoaded = false;
            }
            else
            {
                return; // Mettre en pause la fonction Update entière
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
                GameStateManager.Instance.SetState(GameState.Won);
            }
        }
        // Managing Restart level
        if (NoBulletLefts())
        {
            //LevelFailScript failMenu = GameObject.FindFirstObjectByType<LevelFailScript>();
            //failMenu.menu.SetActive(true);

            /* On change l'état à Lost et failMenu s'affichera de lui-même */
            bDisplay.SetActive(false);
            GameStateManager.Instance.SetState(GameState.Lost);
        }
    }

    public void LoadRoom(int index)
    {
        // Détruire la room actuelle
        if (currentRoomInstance != null)
            Destroy(currentRoomInstance);

        // Créer la nouvelle room (ou la même, ça dépend d'où est appelée la méthode)
        currentRoomInstance = Instantiate(roomPrefabs[index]);

        failCheckTimer = 0f;
        roomJustLoaded = true;

        var roomMusic = currentRoomInstance.GetComponentInChildren<RoomMusic>(true);
        if (roomMusic && roomMusic.playOnSpawn && roomMusic.clip)
        {
            // Si la musique change
            if (MusicManager.Instance.CurrentClip != roomMusic.clip)
                MusicManager.Instance.Play(roomMusic.clip, roomMusic.loop, roomMusic.fade, roomMusic.replayOnLoop);
        }

        // Affichage UI des bullets
        BulletsCountdownLogic ui = GameObject.FindFirstObjectByType<BulletsCountdownLogic>();
        if (ui != null)
        {
            ui.RefreshShootingReference();
        }

        bDisplay.SetActive(true);

        // Récupère les targets dans cette room
        currentTargets = currentRoomInstance.GetComponentsInChildren<TargetScript>();

        // Stop les sfx en cours (s'il y'en a)
        SFXManager.Instance.StopAllLoopSFX();

        // Et on joue
        if (roomJustLoaded) // On attend que la transition se fasse avant
            GameStateManager.Instance.SetState(GameState.Playing);
    }

    public void LoadNextRoom()
    {
        if (currentRoomIndex == 14)
        {
            currentRoomIndex = 0;
        }
        currentRoomIndex++;
        if (currentRoomIndex < roomPrefabs.Length -1)
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
        //Debug.Log($"Room {currentRoomIndex} red�marr�e.");
    }

    /**
     * Retourne true s'il n'y a plus de stock de bullets ni d'instance de Bullet dans la room
     */
    private bool NoBulletLefts()
    {
        // R�cup�rer l'instance de Shooting de la room
        Shooting shooter = Object.FindFirstObjectByType<Shooting>();
        if (shooter == null)
            return false;

        // V�rifier que le nombre de Bullets dans la room est de 0
        bool isBulletsInRoom = Object.FindFirstObjectByType<BulletController>();

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
