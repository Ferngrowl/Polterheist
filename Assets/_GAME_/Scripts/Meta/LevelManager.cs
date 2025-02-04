using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float lockdownDuration = 30f;  // Set this in the inspector for how long the lockdown lasts
    public TMP_Text lockdownTimerText;    // TextMeshProUGUI component to display the countdown
    public bool isLockdownActive = false;
    public SceneData sceneData; // Reference to the ScriptableObject
    private float currentLockdownTime;

    [Header("Audio Settings")]
    public AudioSource musicSource;  // Reference to the AudioSource component
    public AudioClip mainTheme;      // Main theme music
    public AudioClip lockdownMusic;  // Lockdown music

    void Start()
    {
        currentLockdownTime = lockdownDuration;
        lockdownTimerText.gameObject.SetActive(false);  // Initially hide timer

         // Start playing the main theme
        if (musicSource != null && mainTheme != null)
        {
            musicSource.clip = mainTheme;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    void Update()
    {
        if (isLockdownActive)
        {
            currentLockdownTime = Mathf.Max(0, currentLockdownTime - Time.deltaTime);
            UpdateLockdownTimer();

            if (currentLockdownTime <= 0f)
            {
                GameOver();
            }
        }
    }

    public void StartLockdown()
    {
        isLockdownActive = true;
        lockdownTimerText.gameObject.SetActive(true);  // Show timer when lockdown starts

        // Switch to lockdown music
        if (musicSource != null && lockdownMusic != null)
        {
            musicSource.clip = lockdownMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private void UpdateLockdownTimer()
    {
        lockdownTimerText.text = $"Lockdown: {Mathf.Ceil(currentLockdownTime)}s";
    }

    public void GameOver()
    {
        sceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("GameOver");
    }

    public void Victory()
    {
        sceneData.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Victory");
    }
}