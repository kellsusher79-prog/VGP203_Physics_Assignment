using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCheckpointSound()
    {
        PlayTone(880f, 0.12f, 0.25f);
    }

    public void PlayCrashSound()
    {
        PlayTone(120f, 0.15f, 0.35f);
    }

    public void PlayWinSound()
    {
        PlayTone(660f, 0.15f, 0.3f);
        Invoke(nameof(PlaySecondWinTone), 0.16f);
        Invoke(nameof(PlayThirdWinTone), 0.32f);
    }

    private void PlaySecondWinTone()
    {
        PlayTone(880f, 0.15f, 0.3f);
    }

    private void PlayThirdWinTone()
    {
        PlayTone(1100f, 0.25f, 0.3f);
    }

    private void PlayTone(float frequency, float duration, float volume)
    {
        int sampleRate = 44100;
        int sampleLength = Mathf.CeilToInt(sampleRate * duration);

        AudioClip clip = AudioClip.Create("Tone", sampleLength, 1, sampleRate, false);

        float[] samples = new float[sampleLength];

        for (int i = 0; i < sampleLength; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate) * volume;
        }

        clip.SetData(samples, 0);
        audioSource.PlayOneShot(clip);
    }
}