using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting_L : MonoBehaviour
{
    [SerializeField] protected OVRInput.Controller controller;

    // Variables for Ray Casting
    [SerializeField] protected float rayLength = 10f;
    [SerializeField] protected LineRenderer lineRenderer;
    protected RaycastHit hit;

    // Variables for Tags
    [SerializeField] protected string floorTag = "Floor";
    [SerializeField] protected string interactableObjectTag = "InteractableObject";
    
    // Variables for Object Interaction
    [SerializeField] protected float rotationAngle = 45f;
    [SerializeField] protected float scaleFactor = 1.1f;
    [SerializeField] protected float moveSpeed = 1f;

    void Update()
    {
        CastRayFromController();
        RotateObject();
        ScaleObject();
        MoveObject();
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

    private void RotateObject()
    {
        // Rotate the object by pressing "X"
        if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag(interactableObjectTag))
            {
                hit.collider.transform.Rotate(Vector3.forward * rotationAngle);
            }
        }
    }

    private void ScaleObject()
    {
        // Scale up the object by pressing "Y"
        if (OVRInput.GetDown(OVRInput.Button.Two, controller))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag(interactableObjectTag))
            {
                hit.collider.transform.localScale *= scaleFactor;
            }
        }
    }

    private void MoveObject()
    {
        // Move the object towards the player by pressing "Primary Trigger"
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag(interactableObjectTag))
            {
                Vector3 direction = (transform.position - hit.collider.transform.position).normalized;
                hit.collider.transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }
}
