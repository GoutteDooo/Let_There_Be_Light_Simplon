using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Créer un pattern singleton de GameStateManager
    public static GameStateManager Instance { get; private set; } // en privé pour ne la modifier uniquement par la classe même (les autres peuvent lire)
    public GameState CurrentState { get; private set; } //enum permettant de connaître létat du jeu
    /* ! Celui-ci est crucial ! */
    /**
     * OnGameStateChanged est un événement C#.
     * Il permet à d'autres scripts de s'abonner pour être informés lorsqu'un changement d'état se produit. (principe de l'Observer Pattern)
     * Le Script émet un signal, les autres réagissent sans devoir le scruter en permanence <3 (pas besoin de vérifier avec Update())
     */
    public event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        // Ne garde uniquement qu'une seule instance de GameStateManager (au cas où)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Publique pour changement d'état
    public void SetState(GameState newState)
    {
        // Evite de réémettre l'événément inutilement si l'état n'a pas changé
        if (newState != CurrentState)
        {
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState); // Notifie les abonnés
            Debug.Log($"GameState changed to: {newState}");
        }
    }
}
