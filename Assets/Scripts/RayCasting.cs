using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting : MonoBehaviour
{
    [SerializeField] protected OVRInput.Controller controller;

    [SerializeField] protected float rayLength = 10f;
    [SerializeField] protected LineRenderer lineRenderer;
    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        CastRayFromController();
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
}
