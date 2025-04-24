using System.Collections;
using UnityEngine;

public class PedalTrigger2 : MonoBehaviour
{
    [Header("Path Settings")]
    public GameObject pathToActivate; // The parent object containing child objects
    public bool togglePath = false;  // If true, pressing the pedal again will toggle the state
    public float moveSpeed = 1f;     // Speed at which the path moves upward
    public float moveDistance = 3f;  // Distance the path will move upward

    [Header("Trigger Settings")]
    public string triggerTag = "Stone"; // Tag for the objects that can trigger the pedal
    public bool isTriggered = false;   // To keep track of the pedal's state

    [Header("Audio Settings")]
    public AudioClip collisionSound; // The sound effect to play on collision
    private AudioSource audioSource; // Reference to the AudioSource component

    private Vector3 initialPosition; // To store the initial position of the path
    private bool isMoving = false;   // To prevent multiple movements at the same time

    private void Start()
    {
        // Add an AudioSource component to the GameObject (if not already added)
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Ensure the sound doesn't play on scene start

        // Store the initial position of the path
        if (pathToActivate != null)
        {
            initialPosition = pathToActivate.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding has the correct tag
        if (collision.gameObject.CompareTag(triggerTag))
        {
            if (!isTriggered || togglePath)
            {
                isTriggered = !isTriggered;

                if (pathToActivate != null)
                {
                    // Toggle the MeshRenderer visibility of the children
                    ToggleMeshRenderers(isTriggered);

                    // Toggle the isTrigger property of the MeshCollider
                    ToggleMeshColliders(isTriggered);

                    // Move the path upward if triggered
                    if (isTriggered && !isMoving)
                    {
                        StartCoroutine(MovePathUpward());
                    }
                }

                // Play the collision sound
                PlaySoundEffect();

                Debug.Log("Pedal activated by: " + collision.gameObject.name);
            }
        }
    }

    private void ToggleMeshRenderers(bool state)
    {
        MeshRenderer[] childRenderers = pathToActivate.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in childRenderers)
        {
            renderer.enabled = state;
        }
    }

    private void ToggleMeshColliders(bool state)
    {
        MeshCollider[] childColliders = pathToActivate.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider collider in childColliders)
        {
            collider.convex = true;
            collider.isTrigger = !state;
        }
    }

    private void PlaySoundEffect()
    {
        if (collisionSound != null && audioSource != null)
        {
            audioSource.clip = collisionSound;
            audioSource.Play();
        }
    }

    private IEnumerator MovePathUpward()
    {
        isMoving = true;
        Vector3 targetPosition = initialPosition + Vector3.up * moveDistance;

        while (Vector3.Distance(pathToActivate.transform.position, targetPosition) > 0.01f)
        {
            // Move the path gradually toward the target position
            pathToActivate.transform.position = Vector3.MoveTowards(
                pathToActivate.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null; // Wait for the next frame
        }

        isMoving = false;
    }
}