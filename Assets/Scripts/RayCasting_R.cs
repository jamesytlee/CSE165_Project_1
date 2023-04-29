using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting_R : MonoBehaviour
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

    void Update()
    {
        CastRayFromController();
        SpawnChair();
        SpawnDesk();
        TeleportOnFloor();
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
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag(floorTag))
            {
                Teleport(hit.point);
            }
        }
    }

    private void Teleport(Vector3 targetPosition)
    {
        isTeleporting = true;
        Vector3 r_targetPosition = targetPosition;

        // Find the OVRPlayerController and change its position
        GameObject playerController = transform.root.gameObject;
        r_targetPosition.y = playerController.transform.position.y; // prevents clipping
        playerController.transform.position = r_targetPosition;

        isTeleporting = false; 
    }
}
