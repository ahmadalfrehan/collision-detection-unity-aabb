using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class advancedadvancedAABB : MonoBehaviour
{
    private List<GameObject> objects;
    private List<Vector3> objectMins;
    private List<Vector3> objectMaxs;
    private List<bool> isCollidingList;
    public GameObject mainObject;
    public float detectionRadius = 10f;
    public LayerMask objectLayer;

    private List<GameObject> nearestObjects;


    private void Start()
    {
        objects = new List<GameObject>();
        objectMins = new List<Vector3>();
        objectMaxs = new List<Vector3>();
        isCollidingList = new List<bool>();
        nearestObjects = new List<GameObject>();
        FindNearestObjects();

        // Find all objects in the scene
        /// GameObject[] sceneObjects = FindObjectsOfType<GameObject>();

        // Filter out non-active objects
        foreach (GameObject obj in nearestObjects)//sceneObjects)
        {
            if (obj.activeInHierarchy)
                objects.Add(obj);
        }

        // Initialize lists and calculate initial bounding boxes for each object
        foreach (GameObject obj in objects)
        {
            CalculateBoundingBox(obj, out Vector3 min, out Vector3 max);
            objectMins.Add(min);
            objectMaxs.Add(max);
            isCollidingList.Add(false);
        }
    }

    private void Update()
    {
        // Update bounding boxes for each object
        FindNearestObjects();
        for (int i = 0; i < objects.Count; i++)
        {
            CalculateBoundingBox(objects[i], out Vector3 min, out Vector3 max);
            objectMins[i] = min;
            objectMaxs[i] = max;
        }

        // Perform collision detection for each pair of objects
        for (int i = 0; i < objects.Count - 1; i++)
        {
            for (int j = i + 1; j < objects.Count; j++)
            {
                if (objectMaxs[i].x >= objectMins[j].x && objectMins[i].x <= objectMaxs[j].x &&
                    objectMaxs[i].y >= objectMins[j].y && objectMins[i].y <= objectMaxs[j].y &&
                    objectMaxs[i].z >= objectMins[j].z && objectMins[i].z <= objectMaxs[j].z)
                {
                    // Objects i and j are colliding
                    if (!isCollidingList[i] || !isCollidingList[j])
                    {
                        isCollidingList[i] = true;
                        isCollidingList[j] = true;
                        Debug.Log("Collision Detected between " + objects[i].name + " and " + objects[j].name);
                    }
                }
                else
                {
                    // Objects i and j are not colliding
                    if (isCollidingList[i])
                        isCollidingList[i] = false;

                    if (isCollidingList[j])
                        isCollidingList[j] = false;
                }
            }
        }
    }

    private void CalculateBoundingBox(GameObject obj, out Vector3 min, out Vector3 max)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer != null)
        {
            min = renderer.bounds.min;
            max = renderer.bounds.max;
        }
        else
        {
            min = obj.transform.position;
            max = obj.transform.position;
        }
    }
    private void FindNearestObjects()
    {
        nearestObjects.Clear();

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj != mainObject && obj.layer == objectLayer)
            {
                float distance = Vector3.Distance(mainObject.transform.position, obj.transform.position);
                if (distance <= detectionRadius)
                {
                    nearestObjects.Add(obj);
                }
            }
        }
    }
}

