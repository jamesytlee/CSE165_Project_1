using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Rigidbody interactableObject;

    private void Awake()
    {
        interactableObject = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform interactionPoint)
    {
        interactableObject.isKinematic = true;
        transform.SetParent(interactionPoint);
    }

    public void Drop()
    {
        transform.SetParent(null);
        interactableObject.isKinematic = false;
    }

    public void Rotate(float rotationAngle)
    {
        transform.Rotate(Vector3.forward, rotationAngle);
    }
}
