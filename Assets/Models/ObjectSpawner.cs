using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint; // The position and orientation where the spawned objects will originate from

    public GameObject object1Prefab; // The first prefab for the object that can be spawned
    public GameObject object2Prefab; // The second prefab for the object that can be spawned

    // Update is called once per frame
    void Update()
    {
        // Check if the user has pressed the A button
        if(Input.GetButtonDown("A")) {
            // Spawn object1Prefab
            SpawnObject(object1Prefab);
        }
    }

    void SpawnObject(GameObject prefab) 
    {
        // Instantiate the specified prefab at the spawn point
        GameObject spawnedObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        // Apply physics properties to the spawned object
        Rigidbody spawnedObjectRigidbody = spawnedObject.GetComponent<Rigidbody>();
        spawnedObjectRigidbody.useGravity = true;
        spawnedObjectRigidbody.isKinematic = false;
    }
}
