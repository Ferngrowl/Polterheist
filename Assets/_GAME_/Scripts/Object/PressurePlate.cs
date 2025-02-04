using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour
{
    [Header("Pressure Plate Settings")]
    public UnityEvent onPlatePressed; // Event triggered when the plate is pressed
    public UnityEvent onPlateReleased; // Event triggered when the plate is released

    [Header("Sprites")]
    public Sprite unpressedSprite; // Sprite for the "unpressed" state
    public Sprite pressedSprite;   // Sprite for the "pressed" state

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private HashSet<Collider2D> objectsOnPlate = new HashSet<Collider2D>(); // Track objects on the plate

    [Header("Audio")]
    public AudioClip platePressSound;  // Sound to play when the plate is pressed
    public AudioClip plateReleaseSound; // Sound to play when the plate is released
    public AudioSource audioSource;    // AudioSource component to play the sound

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisualState();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidObject(collision))
        {
            objectsOnPlate.Add(collision);
            if (objectsOnPlate.Count == 1)
            {
                onPlatePressed.Invoke();
                UpdateVisualState();

                // Play plate press sound
                if (audioSource && platePressSound)
                {
                    audioSource.PlayOneShot(platePressSound);
                }

                // Notify the door to add an open source
                Door door = GetComponentInParent<Door>();
                if (door != null)
                {
                    door.AddOpenSource();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsValidObject(collision))
        {
            objectsOnPlate.Remove(collision);
            if (objectsOnPlate.Count == 0)
            {
                onPlateReleased.Invoke();
                UpdateVisualState();

                // Play plate release sound
                if (audioSource && plateReleaseSound)
                {
                    audioSource.PlayOneShot(plateReleaseSound);
                }

                // Notify the door to remove an open source
                Door door = GetComponentInParent<Door>();
                if (door != null)
                {
                    door.RemoveOpenSource();
                }
            }
        }
    }

    private bool IsValidObject(Collider2D collision)
    {
        // BoxCollider2D of player, NPC, or barrel triggers the pressure plate
        if (collision is BoxCollider2D && (collision.CompareTag("Player") || collision.CompareTag("NPC") || collision.CompareTag("Barrel")))
        {
            return true;
        }
        return false;
    }

    private void UpdateVisualState()
    {
        spriteRenderer.sprite = objectsOnPlate.Count > 0 ? pressedSprite : unpressedSprite;
    }
}