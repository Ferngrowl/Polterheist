using UnityEngine;

public enum StationaryDirection { Down, Right, Left, Up }

public class Turret : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float fireInterval = 2f;
    public float arrowSpeed = 5f;
    public float detectionRange = 10f;
    public StationaryDirection stationaryDirection;

    public Sprite[] directionSprites; // Down, Right, Left, Up
    private float fireTimer;
    private SpriteRenderer spriteRenderer;

    private LevelManager levelManager;  // Reference to LevelManager

    public AudioClip arrowSound; // Sound to play when possessing an NPCa
    public AudioSource audioSource;   // AudioSource component to play the sound

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelManager = FindObjectOfType<LevelManager>();  // Find the LevelManager in the scene
        UpdateTurretSprite(); // Update sprite based on the initial direction
    }

    void Update()
    {
        // Only process if lockdown is active
        if (levelManager.isLockdownActive)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireInterval)
            {
                FireArrow(GetDirectionVector(stationaryDirection));
                fireTimer = 0f;
            }
        }
    }

    void FireArrow(Vector2 direction)
    {
        if (arrowPrefab && firePoint)
        {
            var arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            arrow.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed;
            arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            // Play door open sound
            if (audioSource && arrowSound)
            {
                audioSource.PlayOneShot(arrowSound);
            }
        }
    }

    Vector2 GetDirectionVector(StationaryDirection dir)
    {
        return dir switch
        {
            StationaryDirection.Up => Vector2.up,
            StationaryDirection.Down => Vector2.down,
            StationaryDirection.Left => Vector2.left,
            StationaryDirection.Right => Vector2.right,
            _ => Vector2.right
        };
    }

    void UpdateTurretSprite()
    {
        if (directionSprites != null && directionSprites.Length == 4)
        {
            spriteRenderer.sprite = directionSprites[(int)stationaryDirection];

            // Adjust the firePoint position based on the stationary direction
            switch (stationaryDirection)
            {
                case StationaryDirection.Down:
                    firePoint.localPosition = new Vector2(0, -1f); // Move firePoint down
                    break;
                case StationaryDirection.Up:
                    firePoint.localPosition = new Vector2(0, 1f);  // Move firePoint up
                    break;
                case StationaryDirection.Left:
                    firePoint.localPosition = new Vector2(-1f, 0); // Move firePoint left
                    break;
                case StationaryDirection.Right:
                    firePoint.localPosition = new Vector2(1f, 0);  // Move firePoint right
                    break;
            }
        }
    }
}