using UnityEngine;

public class PlayerGhostController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

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
    }

    void FixedUpdate()
    {
        // Move player
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // Flip sprite immediately on input detection
        if (moveInput.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (moveInput.x < -0.1f)
            spriteRenderer.flipX = true;
        
        // Update animation state in FixedUpdate to match physics
        animator.SetBool("isMoving", moveInput.sqrMagnitude > 0);
    }
}
