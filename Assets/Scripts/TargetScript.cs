using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public bool isTargetActive = false;
    public SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
