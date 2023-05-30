using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stay : MonoBehaviour
{
    public float detectionRadius = 10f;
    public LayerMask objectLayer;
    public GameObject planeObject;

    private List<GameObject> nearestObjects;

    private void Start()
    {
        nearestObjects = new List<GameObject>();
    }

    private void Update()
    {
        FindNearestObjects();
        CheckCollisions();
    }

    private void FindNearestObjects()
    {
        nearestObjects.Clear();

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj != gameObject && obj.layer == objectLayer)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance <= detectionRadius)
                {
                    nearestObjects.Add(obj);
                }
            }
        }
    }

    private void CheckCollisions()
    {
        for (int i = 0; i < nearestObjects.Count; i++)
        {
            Renderer renderer1 = gameObject.GetComponent<Renderer>();
            Renderer renderer2 = nearestObjects[i].GetComponent<Renderer>();

            if (renderer1 != null && renderer2 != null)
            {
                Vector3 min1 = renderer1.bounds.min;
                Vector3 max1 = renderer1.bounds.max;
                Vector3 min2 = renderer2.bounds.min;
                Vector3 max2 = renderer2.bounds.max;

                // Perform AABB collision check
                if (max1.x >= min2.x && min1.x <= max2.x &&
                    max1.y >= min2.y && min1.y <= max2.y &&
                    max1.z >= min2.z && min1.z <= max2.z)
                {
                    // Collision detected between the objects
                    Debug.Log("Collision Detected between " + gameObject.name + " and " + nearestObjects[i].name);

                    // Check if the collision is with the plane
                    if (nearestObjects[i] == planeObject)
                    {
                        // Calculate the new position to stay on the plane
                        float yOffset = max2.y - min1.y;
                        Vector3 newPosition = transform.position + new Vector3(0f, yOffset, 0f);
                        transform.position = newPosition;
                    }
                }
            }
        }
    }
}
