using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Material highlightMaterial;
    private Material originalMaterial;
    private GameObject currentSelection;
    public string selectableTag = "Selectable";

    // Start is called before the first frame update
    void Start()
    {
        // Cast a ray from the camera's position forward
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                // Check if the object that was hit has the specified tag
                if (hit.collider.CompareTag(selectableTag))
                {
                    // If the user clicked on a selectable object, highlight it
                    currentSelection = hit.collider.gameObject;
                    originalMaterial = currentSelection.GetComponent<Renderer>().material;
                }
                else
                {
                    // If the user clicked on an empty space or a non-selectable object, deselect the current selection
                    currentSelection = null;
                }
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSelection != null)
        {
            // Change the material of the selected object to the highlight material
            currentSelection.GetComponent<Renderer>().material = highlightMaterial;
        }
        else
        {
            // Reset the material of the previously selected object
            if (originalMaterial != null)
            {
                originalMaterial = null;
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag(selectableTag))
                {
                    obj.GetComponent<Renderer>().material = originalMaterial;
                }
            }
        }
    }
}
