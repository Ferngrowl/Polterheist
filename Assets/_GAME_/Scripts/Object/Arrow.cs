using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifetime = 5f; // Time before the arrow is destroyed
    private LevelManager levelManager;  // Reference to LevelManager

    void Start()
    {
        Destroy(gameObject, lifetime);
        levelManager = FindObjectOfType<LevelManager>();  // Find the LevelManager in the scene
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pet") && levelManager != null)
        {
            levelManager.GameOver();
        }
        Destroy(gameObject);
    }
}