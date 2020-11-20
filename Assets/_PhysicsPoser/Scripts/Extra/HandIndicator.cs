using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandIndicator : MonoBehaviour
{
    [SerializeField] private XRController controller = null;
    [SerializeField] private GameObject hand = null;
    [SerializeField] private float showDistance = 0.25f;

    private MeshRenderer meshRenderer = null;
    private Vector3 targetPosition = Vector3.zero;
    private float currentDistance = 0.0f;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        SetTargetPosition();
        CalculateDistance();
        ToggleMesh();
    }

    private void SetTargetPosition()
    {
        controller.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out targetPosition);
        transform.position = targetPosition;
    }

    private void CalculateDistance()
    {
        currentDistance = Vector3.Distance(hand.transform.position, targetPosition);
    }

    private void ToggleMesh()
    {
        meshRenderer.enabled = showDistance < currentDistance;
    }
}
