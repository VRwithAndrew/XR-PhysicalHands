using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRController))]
public class PhysicsPoser : MonoBehaviour
{
    // Values
    public float physicsRange = 0.1f;
    public LayerMask physicsMask = 0;

    [Range(0, 1)] public float slowDownVelocity = 0.75f;
    [Range(0, 1)] public float slowDownAngularVelocity = 0.75f;

    [Range(0, 100)] public float maxPositionChange = 75.0f;
    [Range(0, 100)] public float maxRotationChange = 75.0f;

    // References
    private RigidBody rigidBody = null;
    private XRController controller = null;
    private XRBaseInteractor interactor = null;
    // Runtime
    private Vector3 targetPosition = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;
    private void Awake()
    {
        // Get the stuff
        rigidBody = GetComponent<Rigidbody>();
        controller = GetComponent<XRController>();
        interactor = GetComponent<XRBaseInteractor>();
    }

    private void Start()
    {
        // As soon as we start, move to the hand
        UpdateTracking(controller.inputDevice);
        MoveUsingTransform();
        RotateUsingTransform();
    }

    private void Update()
    {
        // Update our target location
        UpdateTracking(controller.inputDevice);
    }

    private void UpdateTracking(InputDevice inputDevice)
    {
        // Get the rotation and position from the device
        inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out targetPosition);
        inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out targetRotation);
    }

    private void FixedUpdate()
    {
        // Move via transform if we're holding an object, or not within physics range
        if (IsHoldingObject() || !WithinPhysicsRange()) 
        {
            MoveUsingTransform();
            RotateUsingTransform();
        }
        // Else move using physics
        else 
        {
            MoveUsingPhysics();
            RotateUsingPhysics();
        }
    }

    public bool IsHoldingObject()
    {
        return interactor.selectTarget;
    }

    public bool WithinPhysicsRange()
    {
        return Physics.CheckSphere(transform.position, physicsRange, physicsMask, QueryTriggerInteraction.Ignore);
    }

    private void MoveUsingPhysics()
    {
        // Prevents overshooting
        rigidBody.velocity *= slowDownVelocity;

        // Get target velocity
        Vector3 velocity = FindNewVelocity();

        // Check if it's valid
        if (IsValidVelocity(velocity.x)) 
        {
            // Figure out the max we can move, then move via velocity
            float maxChange = maxPositionChange * Time.deltaTime;
            rigidBody.velocity = Vector3.MoveTowards(rigidBody.velocity, velocity, maxChange);    
        }
    }

    private Vector3 FindNewVelocity()
    {
        Vector3 difference = targetPosition - rigidBody.position;
        return difference / Time.deltaTime;
    }

    private void RotateUsingPhysics()
    {
        // Prevents overshooting
        rigidBody.angularVelocity *= slowDownAngularVelocity;

        // Get target velocity
        Vector3 angularVelocity = FindNewAngularVelocity();

        // Check if it's valid
        if (IsValidVelocity(angularVelocity.x)) 
        {
            // Figure out the max we can rotate, then move via velocity
            float maxChange = maxRotationChange * Time.deltaTime;
            rigidBody.angularVelocity = Vector3.MoveTowards(rigidBody.angularVelocity, angularVelocity, maxChange);
        }
    }

    private Vector3 FindNewAngularVelocity()
    {
        // Figure out the difference in rotation
        Quaternion difference = targetRotation * Quaternion.Inverse(rigidBody.rotation);
        difference.ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);

        // Do the weird thing to account for have a range of -180 to 180
        if (angleInDegrees > 180)
            angleInDegrees -= 360;

        // Figure out the difference we can move this frame
        return (rotationAxis * angleInDegrees * Mathf.Deg2Rad) / Time.deltaTime;
    }

    private bool IsValidVelocity(float value)
    {
        // Is it an actual number, or is a broken number?
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }

    private void MoveUsingTransform()
    {
        // Prevents jitter
        rigidBody.velocity = Vector3.zero;
        transform.localPosition = targetPosition;
    }

    private void RotateUsingTransform()
    {
        // Prevents jitter
        rigidBody.angularVelocity = Vector3.zero;
        transform.localRotation = targetRotation;
    }

    private void OnDrawGizmos()
    {
        // Show the range at which the physics takeover
        Gizmos.DrawWireSphere(transform.position, physicsRange);
    }

    private void OnValidate()
    {
        // Just in case
        if (TryGetComponent(out RigidBody rigidBody))
            rigidBody.useGravity = false;
    }
}
