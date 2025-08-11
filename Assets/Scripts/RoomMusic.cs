// RoomMusic.cs
using UnityEngine;

[DisallowMultipleComponent]
public class RoomMusic : MonoBehaviour
{
    public AudioClip clip;
    public bool loop = true;
    public float fade = 1.5f;
    [Tooltip("Lance la musique dès l'apparition de la room")]
    public bool playOnSpawn = true;
    [Tooltip("Replay la musique à la seconde donnée")]
    public float replayOnLoop = 0f;
}
