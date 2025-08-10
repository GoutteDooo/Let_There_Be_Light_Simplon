using UnityEngine;

public class PlayermovementScript : MonoBehaviour
{
    public Transform arm;       // The arm whose rotation we check
    public Transform modelRoot; // The main model to flip
    [SerializeField] Animator _animator;

    private bool flipped = false;

    void Update()
    {
        // Get arm's rotation around its axis.
        float armAngle = arm.localEulerAngles.z;

        // Flip state logic with hysteresis (no jitter near thresholds)
        if (!flipped && (armAngle > 90f && armAngle < 270f))
        {
            flipped = true;
        }
        else if (flipped && (armAngle < 90f || armAngle > 270f))
        {
            flipped = false;
        }

        // Snap instantly to the correct facing
        modelRoot.localEulerAngles = new Vector3(
            modelRoot.localEulerAngles.x,
            flipped ? 180f : 0f,
            modelRoot.localEulerAngles.z
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _animator.SetBool("isHit", true);
    }
}