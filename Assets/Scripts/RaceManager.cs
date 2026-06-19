using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    [Header("Race Settings")]
    public int totalCheckpoints = 5;

    [Header("UI")]
    public Text statusText;
    public Text timerText;

    private int nextCheckpoint = 1;
    private bool raceWon = false;
    private float raceTimer = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateStatusText();
        UpdateTimerText();
    }

    private void Update()
    {
        CheckRestartInput();

        if (raceWon)
            return;

        raceTimer += Time.deltaTime;
        UpdateTimerText();
    }

    private void CheckRestartInput()
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null && keyboard.rKey.wasPressedThisFrame)
        {
            RestartRace();
        }
#else
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartRace();
        }
#endif
    }

    public bool TryCollectCheckpoint(int checkpointNumber)
    {
        if (raceWon)
            return false;

        if (checkpointNumber == nextCheckpoint)
        {
            Debug.Log("Checkpoint " + checkpointNumber + " collected!");

            nextCheckpoint++;
            UpdateStatusText();

            if (nextCheckpoint > totalCheckpoints)
            {
                statusText.text = "All checkpoints collected! Go to the finish line!";
                Debug.Log("All checkpoints collected! Go to the finish line!");
            }

            return true;
        }

        statusText.text = "Wrong checkpoint! Get checkpoint " + nextCheckpoint + " first.";
        Debug.Log("Wrong checkpoint! You need checkpoint " + nextCheckpoint + " first.");
        return false;
    }

    public bool HasAllCheckpoints()
    {
        return nextCheckpoint > totalCheckpoints;
    }

    public void WinRace()
    {
        if (raceWon)
            return;

        raceWon = true;

        statusText.text = "You win! Final Time: " + raceTimer.ToString("F1") + " seconds. Press R to restart.";

        TopDownCarController car = FindFirstObjectByType<TopDownCarController>();

        if (car != null)
        {
            car.StopDriving();
        }

        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.PlayWinSound();
        }

        Debug.Log("You win! Race complete.");
    }

    public void RestartRace()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateStatusText()
    {
        if (statusText == null)
            return;

        statusText.text = "Checkpoint " + nextCheckpoint + " / " + totalCheckpoints + "    Press R to restart";
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        timerText.text = "Time: " + raceTimer.ToString("F1");
    }
}