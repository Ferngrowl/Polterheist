using System.Collections;
using UnityEngine;

public enum SpikeState
{
    Off,
    Primed,
    On
}

public class Spike : MonoBehaviour
{
    public float cycleInterval = 2f;        // Interval between state changes
    public float initialOffset = 0f;         // Initial offset to stagger the cycles
    public Sprite offSprite;                // Sprite when the spike is "Off"
    public Sprite primedSprite;             // Sprite when the spike is "Primed"
    public Sprite onSprite;                 // Sprite when the spike is "On"

    private SpikeState currentState = SpikeState.Off;
    private SpriteRenderer spriteRenderer;  // To update the spike sprite
    private LevelManager levelManager;  // Reference to LevelManager

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelManager = FindObjectOfType<LevelManager>();  // Find the LevelManager in the scene
        StartCoroutine(CycleSpikeState());
    }

    private IEnumerator CycleSpikeState()
    {
        yield return new WaitForSeconds(initialOffset); // Apply initial offset

        while (true)
        {
            if (levelManager.isLockdownActive)
            {
                currentState = (SpikeState)(((int)currentState + 1) % 3);
                UpdateSpikeAppearance();
            }
            yield return new WaitForSeconds(cycleInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pet") && currentState == SpikeState.On && levelManager != null)
        {
            levelManager.GameOver();
        }
    }

    private void UpdateSpikeAppearance()
    {
        spriteRenderer.sprite = currentState switch
        {
            SpikeState.Off => offSprite,
            SpikeState.Primed => primedSprite,
            SpikeState.On => onSprite,
            _ => offSprite
        };
    }
}