using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision deadzone");
        if (collision.gameObject.GetComponent<BulletController>() != null)
        {
            SFXManager.Instance.PlaySFX("BulletOut");
            SFXManager.Instance.PlaySFX("BulletOutSparks");
            SFXManager.Instance.StopSFX("Electricite");
        }
        Destroy(collision.gameObject);
    }
}
