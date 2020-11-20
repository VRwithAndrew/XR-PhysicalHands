using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PhysicsPoser))]
[RequireComponent(typeof(XRDirectInteractor))]
public class ControllerHider : MonoBehaviour
{
    public GameObject controllerObject = null;

    private PhysicsPoser physicsPoser = null;
    private XRDirectInteractor interactor = null;

    private void Awake()
    {
        physicsPoser = GetComponent<PhysicsPoser>();
        interactor = GetComponent<XRDirectInteractor>();
    }

    private void OnEnable()
    {
        interactor.onSelectEntered.AddListener(Hide);
        interactor.onSelectExited.AddListener(Show);
    }

    private void OnDisable()
    {
        interactor.onSelectEntered.RemoveListener(Hide);
        interactor.onSelectExited.RemoveListener(Show);
    }

    private void Hide(XRBaseInteractable interactable)
    {
        controllerObject.SetActive(false);
    }

    private void Show(XRBaseInteractable interactable)
    {
        StartCoroutine(WaitForRange());
    }

    private IEnumerator WaitForRange()
    {
        yield return new WaitWhile(physicsPoser.WithinPhysicsRange);
        controllerObject.SetActive(true);
    }
}
