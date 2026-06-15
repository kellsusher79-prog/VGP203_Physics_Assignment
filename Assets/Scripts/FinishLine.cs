using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        TopDownCarController car = other.GetComponent<TopDownCarController>();

        if (car != null)
        {
            if (RaceManager.Instance.HasAllCheckpoints())
            {
                RaceManager.Instance.WinRace();
            }
            else
            {
                Debug.Log("Collect all checkpoints first!");
            }
        }
    }
}