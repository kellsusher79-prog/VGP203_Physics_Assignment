using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber = 1;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        TopDownCarController car = other.GetComponent<TopDownCarController>();

        if (car != null)
        {
            bool wasCorrectCheckpoint = RaceManager.Instance.TryCollectCheckpoint(checkpointNumber);

            if (wasCorrectCheckpoint)
            {
                collected = true;

                if (GameAudio.Instance != null)
                {
                    GameAudio.Instance.PlayCheckpointSound();
                }

                gameObject.SetActive(false);
            }
        }
    }
}