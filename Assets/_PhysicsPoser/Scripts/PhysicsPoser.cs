using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRController))]
public class PhysicsPoser : MonoBehaviour
{
    // Values

    // References

    // Runtime

    private void Awake()
    {
        // Get the stuff
    }

    private void Start()
    {
        // As soon as we start, move to the hand
    }

    private void Update()
    {
        // Update our target location
    }

    private void UpdateTracking(InputDevice inputDevice)
    {
        // Get the rotation and position from the device
    }

    private void FixedUpdate()
    {
        // Move via transform if we're holding an object, or not within physics range

        // Else move using physics
    }

    public bool IsHoldingObject()
    {
        return false;
    }

    public bool WithinPhysicsRange()
    {
        return false;
    }

    private void MoveUsingPhysics()
    {
        // Prevents overshooting

        // Get target velocity

        // Check if it's valid

        // Figure out the max we can move, then move via velocity
    }

    private Vector3 FindNewVelocity()
    {
        return Vector3.zero;
    }

    private void RotateUsingPhysics()
    {
        // Prevents overshooting

        // Get target velocity

        // Check if it's valid

        // Figure out the max we can rotate, then move via velocity
    }

    private Vector3 FindNewAngularVelocity()
    {
        // Figure out the difference in rotation

        // Do the weird thing to account for have a range of -180 to 180

        // Figure out the difference we can move this frame
        return Vector3.zero;
    }

    private bool IsValidVelocity(float value)
    {
        // Is it an actual number, or is a broken number?
        return false;
    }

    private void MoveUsingTransform()
    {
        // Prevents jitter
    }

    private void RotateUsingTransform()
    {
        // Prevents jitter
    }

    private void OnDrawGizmos()
    {
        // Show the range at which the physics takeover
    }

    private void OnValidate()
    {
        // Just in case
    }
}
