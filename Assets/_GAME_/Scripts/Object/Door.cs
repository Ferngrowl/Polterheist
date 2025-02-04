using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public bool isOpen = false;
    public Sprite closedSprite;
    public Sprite openSprite;
    public float openDuration = 2f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private int openSources = 0; // Counter for active sources keeping the door open

    [Header("Audio")]
    public AudioClip doorOpenSound;  // Sound to play when the door opens
    public AudioSource audioSource;  // AudioSource component to play the sound

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        UpdateVisualState();
    }

    // Called when a source activates (e.g., pressure plate or lever)
    public void AddOpenSource()
    {
        openSources++;
        if (!isOpen)
        {
            isOpen = true;
            UpdateVisualState();

            // Play door open sound
            if (audioSource && doorOpenSound)
            {
                audioSource.PlayOneShot(doorOpenSound);
            }
        }
    }

    // Called when a source deactivates (e.g., pressure plate or lever)
    public void RemoveOpenSource()
    {
        openSources--;
        if (openSources <= 0)
        {
            openSources = 0; // Ensure it doesn't go negative
            isOpen = false;
            UpdateVisualState();

            // Play door open sound
            if (audioSource && doorOpenSound)
            {
                audioSource.PlayOneShot(doorOpenSound);
            }
        }
    }

    // Timed door functionality
    public void TimedDoor()
    {
        if (!isOpen)
        {
            AddOpenSource();
            StartCoroutine(CloseAfterDelay());
        }
    }

    private IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(openDuration);
        RemoveOpenSource();
    }

    private void UpdateVisualState()
    {
        spriteRenderer.sprite = isOpen ? openSprite : closedSprite;
        boxCollider.enabled = !isOpen;
    }
}