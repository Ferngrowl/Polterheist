using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public float wanderSpeed = 0.2f;
    private Vector2 wanderDirection;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(ChangeDirection), 0f, 2f); // Change direction every 2 seconds
    }

    void FixedUpdate()
    {
        if (enabled)
        {
            rb.velocity = wanderDirection * wanderSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void ChangeDirection()
    {
        if (enabled)
        {
            wanderDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}