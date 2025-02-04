using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    private GameObject nearbyNPC; // Reference to the NPC in possession range
    private GameObject currentNPC; // NPC currently possessed
    private PlayerGhostController ghostController;
    private bool isPossessing = false;

    public CameraFollow cameraFollow; // Reference to the CameraFollow script
    public LevelManager levelManager;  // Reference to the LevelManager
    public ParticleSystem possessionParticleEffect; // Reference to the particle effect
    public AudioClip possessionSound; // Sound to play when possessing an NPC
    public AudioSource audioSource;   // AudioSource component to play the sound

    void Awake()
    {
        ghostController = GetComponent<PlayerGhostController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC") || collision.CompareTag("Pet"))
        {
            nearbyNPC = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == nearbyNPC)
        {
            nearbyNPC = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearbyNPC != null && !isPossessing)
        {
            PossessNPC();
        }
        else if (Input.GetKeyDown(KeyCode.F) && isPossessing)
        {
            ExitPossession();
        }
    }

   void PossessNPC()
    {
        currentNPC = nearbyNPC;
        ghostController.enabled = false;

        if (currentNPC.TryGetComponent(out NPCPossessionControls possessionControls))
        {
            possessionControls.enabled = true;

            // Play possession sound
            if (audioSource && possessionSound)
            {
                    audioSource.PlayOneShot(possessionSound);
            }

            // Trigger lockdown if the pet is possessed
            if (possessionControls.npcType == NPCType.Pet && levelManager != null && !levelManager.isLockdownActive)
            {
                levelManager.StartLockdown();
            }
        }

        // Play particle effect
        if (possessionParticleEffect)
        {
            var particles = Instantiate(possessionParticleEffect, transform.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);
        }

        ToggleGhostVisibility(false);
        if (cameraFollow != null)
        {
            cameraFollow.SwitchCameraFollowTarget(currentNPC.transform);
        }

        isPossessing = true;
    }

    void ExitPossession()
    {
        if (currentNPC != null && currentNPC.TryGetComponent(out NPCPossessionControls possessionControls))
        {
            // Reset the NPC's animation state
            if (possessionControls.TryGetComponent(out Animator npcAnimator))
            {
                npcAnimator.SetBool("isMoving", false);
            }

            possessionControls.enabled = false; // Disable NPC control
        }

        // Play particle effect
        if (possessionParticleEffect)
        {
            var particles = Instantiate(possessionParticleEffect, currentNPC.transform.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);
        }

        // Reset ghost position and enable movement
        transform.position = currentNPC.transform.position;
        ghostController.enabled = true;

        // Show ghost and switch camera back
        ToggleGhostVisibility(true);
        if (cameraFollow != null)
        {
            cameraFollow.SwitchCameraFollowTarget(transform);
        }

        isPossessing = false;
    }

    private void ToggleGhostVisibility(bool visible)
    {
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.enabled = visible;
        }
    }
}