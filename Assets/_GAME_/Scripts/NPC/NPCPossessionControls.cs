using UnityEngine;

public enum NPCType
{
    Strong,
    Agile,
    Pet
}

public class NPCPossessionControls : MonoBehaviour
{
    public float moveSpeed = 5f;
    public NPCType npcType; // NPC Type reference
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private LeverFlip currentLever; // Stores nearby lever
    private GameObject currentBarrel; // Stores nearby barrel
    private bool isPullingBarrel = false; // Tracks if the NPC is pulling a barrel

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get movement input
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Adjust sorting order based on Y position
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);

        // Handle lever interactions
        if (currentLever != null && (npcType == NPCType.Strong || npcType == NPCType.Agile) && Input.GetKeyDown(KeyCode.E))
        {
            currentLever.ToggleSwitch();
        }

        // Handle barrel pulling
        if (npcType == NPCType.Strong && currentBarrel != null)
        {
            if (Input.GetKey(KeyCode.E)) // Hold E to pull the barrel
            {
                isPullingBarrel = true;
            }
            else
            {
                isPullingBarrel = false;
            }
        }
    }

    void FixedUpdate()
    {
        // Move the NPC
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // Pull the barrel if holding E
        if (isPullingBarrel && currentBarrel != null)
        {
            Rigidbody2D barrelRb = currentBarrel.GetComponent<Rigidbody2D>();
            if (barrelRb != null)
            {
                // Move the barrel in the same direction as the NPC
                barrelRb.MovePosition(barrelRb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
            }
        }

        // Update animation
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        // Flip sprite based on movement direction
        if (moveInput.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (moveInput.x < -0.1f)
            spriteRenderer.flipX = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lever") && (npcType == NPCType.Strong || npcType == NPCType.Agile))
        {
            currentLever = other.GetComponent<LeverFlip>();
        }

        if (other.CompareTag("Barrel") && npcType == NPCType.Strong)
        {
            currentBarrel = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Lever"))
        {
            currentLever = null;
        }

        if (other.CompareTag("Barrel") && npcType == NPCType.Strong)
        {
            currentBarrel = null;
            isPullingBarrel = false; // Stop pulling if the barrel is out of range
        }
    }
}