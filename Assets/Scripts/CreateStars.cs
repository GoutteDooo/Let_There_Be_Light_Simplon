using UnityEngine;

public class CreateStars : MonoBehaviour
{
    [Tooltip("Prefab représentant une étoile (petit sprite blanc)")]
    public GameObject starPrefab;

    [Tooltip("Nombre d'étoiles à générer")]
    public int starCount = 50;

    [Tooltip("Taille minimale et maximale des étoiles")]
    public Vector2 starSizeRange = new Vector2(0.001f, 0.004f);

    void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (!sr || starPrefab == null) return;

        // bornes en LOCAL (avant scale)
        var localBounds = sr.sprite.bounds;            // center + extents (local)
        var center = localBounds.center;               // souvent (0,0) si pivot au centre
        var ext = localBounds.extents;

        for (int i = 0; i < starCount; i++)
        {
            var rx = Random.Range(-ext.x, ext.x);
            var ry = Random.Range(-ext.y, ext.y);
            var localPos = center + new Vector3(rx, ry, 0f);

            var star = Instantiate(starPrefab, transform);
            star.transform.localPosition = localPos;

            float s = Random.Range(starSizeRange.x, starSizeRange.y);
            star.transform.localScale = new Vector3(s, s, 1f);
        }
    }

}
