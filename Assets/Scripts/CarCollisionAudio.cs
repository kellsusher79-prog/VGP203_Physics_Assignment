using UnityEngine;

public class CarCollisionAudio : MonoBehaviour
{
    private float cooldown = 0.25f;
    private float nextCrashTime = 0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time < nextCrashTime)
            return;

        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.PlayCrashSound();
        }

        nextCrashTime = Time.time + cooldown;
    }
}