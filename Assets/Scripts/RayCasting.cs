using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting : MonoBehaviour
{
    [SerializeField] protected OVRInput.Controller controller;

    // Variables for Ray Casting
    [SerializeField] protected float rayLength = 10f;
    [SerializeField] protected LineRenderer lineRenderer;
    private RaycastHit hit;

    // Tag for detecting the floor
    [SerializeField] private string floorTag = "Floor";

    // Variables for Object Spawning
    [SerializeField] protected GameObject chair;
    [SerializeField] protected GameObject desk;
    public Quaternion rotation = Quaternion.Euler(-90f, -180f, 90f);
    public Vector3 yOffset = new Vector3(0, 0.152f, 0);

    // Variables for Teleportation
    private bool isTeleporting;

    // Update is called once per frame
    void Update()
    {
        CastRayFromController();
        SpawnChair();
        SpawnDesk();
        TeleportOnFloor();
    }

    private void CastRayFromController()
    {
        Vector3 rayOrigin = this.transform.position;
        Vector3 rayDirection = this.transform.forward;

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
}
