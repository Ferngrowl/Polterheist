using UnityEngine;
using UnityEngine.Events;

public class LeverFlip : MonoBehaviour
{
    [Header("Switch Settings")]
    public bool isOn = false; // Current state of the switch
    public UnityEvent onSwitchOn; // Event triggered when the switch is turned on
    public UnityEvent onSwitchOff; // Event triggered when the switch is turned off

    [Header("Sprites")]
    public Sprite offSprite; // Sprite for the "off" position
    public Sprite onSprite;  // Sprite for the "on" position

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    [Header("Audio")]
    public AudioClip leverFlipSound; // Sound to play when flipping the lever
    public AudioSource audioSource;  // AudioSource component to play the sound


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisualState();
    }

    public void ToggleSwitch()
    {
        isOn = !isOn;
        (isOn ? onSwitchOn : onSwitchOff).Invoke();
        UpdateVisualState();

        // Play lever flip sound
        if (audioSource && leverFlipSound)
        {
            audioSource.PlayOneShot(leverFlipSound);
        }

        // Notify the door to add or remove an open source
        Door door = GetComponentInParent<Door>();
        if (door != null)
        {
            if (isOn)
            {
                door.AddOpenSource();
            }
            else
            {
                door.RemoveOpenSource();
            }
        }
    }

    private void UpdateVisualState()
    {
        spriteRenderer.sprite = isOn ? onSprite : offSprite;
    }
}