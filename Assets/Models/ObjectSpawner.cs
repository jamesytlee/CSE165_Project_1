using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint; // The position and orientation where the spawned objects will originate from

    public GameObject object1Prefab; // The first prefab for the object that can be spawned
    public GameObject object2Prefab; // The second prefab for the object that can be spawned

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update method called");
        // Check if the user has pressed the A button
        if(Input.GetKeyDown(KeyCode.A)) {
            if(Input.GetKeyDown(KeyCode.A)) {
                Debug.Log("A key pressed");
                // Spawn object1Prefab
                SpawnObject(object1Prefab);
            }

        }
    }

    void SpawnObject(GameObject prefab) 
    {
        // Instantiate the specified prefab at the spawn point
        GameObject spawnedObject = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("Spawning object: " + prefab.name);
        // Apply physics properties to the spawned object
        Rigidbody spawnedObjectRigidbody = spawnedObject.GetComponent<Rigidbody>();
        spawnedObjectRigidbody.useGravity = false;
        spawnedObjectRigidbody.isKinematic = false;
    }
}
