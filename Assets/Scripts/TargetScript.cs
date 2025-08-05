using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public bool isTargetActive = false;
    public SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#D4C477", out color)) // remplace par ton code hex
        {
            sprite.color = color;
        }
        else
        {
            Debug.LogWarning("Code couleur invalide !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isTargetActive = true;
            sprite.color = Color.yellow;
            Debug.Log("Target is active");
        }
    }
}
