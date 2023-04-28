using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting : MonoBehaviour
{
    [SerializeField] protected OVRInput.Controller controller;

    // Variables for Ray Casting
    [SerializeField] protected float rayLength = 10f;
    [SerializeField] protected LineRenderer lineRenderer;
    protected RaycastHit hit;

    // Variables for Tags
    [SerializeField] protected string floorTag = "Floor";
    [SerializeField] protected string interactableObjectTag = "InteractableObject";

    // Variables for Object Spawning
    [SerializeField] protected GameObject chair;
    [SerializeField] protected GameObject desk;
    protected Quaternion rotation = Quaternion.Euler(-90f, -180f, 90f);
    protected Vector3 yOffset = new Vector3(0, 0.25f, 0);

    // Variables for Teleportation
    protected bool isTeleporting;

    // Variables for Object Interaction
    protected InteractableObject interactableObject;
    private bool isHoldingObject;
    private GameObject interactionPoint;
    [SerializeField] protected float rotationAngle = 45f;

    private void State()
    {
        interactionPoint = new GameObject("Interaction Point");
        interactionPoint.transform.SetParent(this.transform);
    }

    void Update()
    {
        CastRayFromController();
        SpawnChair();
        SpawnDesk();
        TeleportOnFloor();
        InteractWithObject();
        RotateObject();
    }

    private void CastRayFromController()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, rayOrigin);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength))
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + rayDirection * rayLength);
        }
    }

    private void SpawnChair()
    {
        // Chair spawned by pressing "A"
        if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
            // Check if the ray hits the floor
            if (hit.collider != null && hit.collider.CompareTag(floorTag))
            {
                // Spawn chair prefab at intersection point
                Instantiate(chair, hit.point + yOffset, rotation);
            }
        }
    }

    private void SpawnDesk()
    {
        // Desk spawned by pressing "B"
        if (OVRInput.GetDown(OVRInput.Button.Two, controller))
        {
            // Check if the ray hits the floor
            if (hit.collider != null && hit.collider.CompareTag(floorTag))
            {
                // Spawn chair prefab at intersection point
                // y-offset = .152
                Instantiate(desk, hit.point + yOffset, rotation);
            }
        }
    }

    private void TeleportOnFloor()
    {
        // Teleport by pressing Thumbstick
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, controller) && !isTeleporting)
        {
            // Check if the ray hits the floor
            if (hit.collider != null && hit.collider.CompareTag(floorTag))
            {
                StartCoroutine(Teleport(hit.point));
            }
        }
    }

    IEnumerator Teleport(Vector3 targetPosition)
    {
        isTeleporting = true;

        // Add a short delay before teleporting to make the teleportation more smooth
        yield return new WaitForSeconds(0.1f);

        // Find the OVRPlayerController and change its position
        GameObject playerController = transform.root.gameObject;
        targetPosition.y = playerController.transform.position.y; // prevents clipping
        playerController.transform.position = targetPosition;

        isTeleporting = false; 
    }

    private void RotateObject()
    {
        // Rotate the object when the secondary hand trigger is clicked
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger, controller))
        {
            if (hit.collider != null && hit.collider.CompareTag(interactableObjectTag))
            {
                interactableObject = hit.collider.GetComponent<InteractableObject>();
                interactableObject.Rotate(rotationAngle);
            }
        }
    }

    private void InteractWithObject()
    {
        // Pick up object by holding down right trigger
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, controller) > 0.5f)
        {
            if (!isHoldingObject && hit.collider != null && hit.collider.CompareTag(interactableObjectTag))
            {
                interactableObject = hit.collider.GetComponent<InteractableObject>();
                interactableObject.PickUp(interactionPoint.transform);
                isHoldingObject = true;
            }

            if (isHoldingObject)
            {
                interactionPoint.transform.position = hit.point;
            }
        }
        else
        {
            if (isHoldingObject)
            {
                interactableObject.Drop();
                isHoldingObject = false;
            }
        }
    }
}
