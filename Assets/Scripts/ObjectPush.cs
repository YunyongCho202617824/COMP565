using UnityEngine;

public class PlayerPushObject : MonoBehaviour
{
    // SerializeField attribute allows private fields to be displayed in the Unity Editor.
    // Declare a private variable to store the magnitude of the force applied.
    [SerializeField]
    private float forceMagnitude;

    // This method is called when the character controller hits a collider while performing a Move action.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Get the rigidbody component attached to the collider that was hit.
        var rigidBody = hit.collider.attachedRigidbody;

        // Check if a rigidbody was found.
        if (rigidBody != null)
        {
            // Calculate the direction from the player to the object being hit.
            var forceDirection = hit.gameObject.transform.position - transform.position;

            // Ensure the force is applied only in the horizontal plane (y = 0).
            forceDirection.y = 0;

            // Normalize the force direction to ensure consistent magnitude regardless of distance.
            forceDirection.Normalize();

            // Apply force to the rigidbody at the position of the player controller.
            // The force is calculated by multiplying the force direction by the force magnitude.
            // ForceMode.Impulse applies an instant force to the object.
            rigidBody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }
    }
}
