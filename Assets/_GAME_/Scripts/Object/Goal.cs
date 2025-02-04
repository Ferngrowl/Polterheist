using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private LevelManager levelManager;  // Reference to LevelManager
    
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();  // Find the LevelManager in the scene
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pet"))
        {
            // If the pet collides trigger victory
            levelManager.Victory();
        }
    }
}
