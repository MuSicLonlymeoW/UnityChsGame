using UnityEngine;

public class PedalTrigger : MonoBehaviour
{
    [Header("Path Settings")]
    public GameObject pathToActivate; // The parent object containing child objects
    public bool togglePath = false;  // If true, pressing the pedal again will toggle the state

    [Header("Trigger Settings")]
    public string triggerTag = "Stone"; // Tag for the objects that can trigger the pedal
    public bool isTriggered = false;   // To keep track of the pedal's state
    
    [Header("Audio Settings")]
    public AudioClip collisionSound; // The sound effect to play on collision
    private AudioSource audioSource;
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

                    // Toggle the isTrigger property of the BoxCollider
                    ToggleBoxColliders(isTriggered);
                }
                PlaySoundEffect();

                // Optional: Add visual/audio feedback
                Debug.Log("Pedal activated by: " + collision.gameObject.name);
            }
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

    private void ToggleMeshRenderers(bool state)
    {
        // Find all child MeshRenderer components and toggle them
        MeshRenderer[] childRenderers = pathToActivate.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in childRenderers)
        {
            renderer.enabled = state; // Enable or disable the MeshRenderer
        }
    }

    private void ToggleBoxColliders(bool state)
    {
        // Find all child BoxCollider components and toggle their isTrigger property
        BoxCollider[] childColliders = pathToActivate.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider collider in childColliders)
        {
            collider.isTrigger = !state; // Set isTrigger to the opposite of the current state
        }
    }
}