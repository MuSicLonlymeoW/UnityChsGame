using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float pushStrength = 5f; // Adjust the strength of the push

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the object hit has a Rigidbody
        Rigidbody rb = hit.collider.attachedRigidbody;

        // Only push objects with Rigidbody and not kinematic
        if (rb != null && !rb.isKinematic)
        {
            // Calculate the push direction
            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // Apply force to the block
            rb.AddForce(pushDirection * pushStrength, ForceMode.Impulse);
        }
    }
}