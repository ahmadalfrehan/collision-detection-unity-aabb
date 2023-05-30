using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class advancedAABB : MonoBehaviour
{

    public List<GameObject> objects;
    private List<Vector3> objectMins;
    private List<Vector3> objectMaxs;
    private List<bool> isCollidingList;

    private void Start()
    {
        // Initialize lists and calculate initial bounding boxes for each object
        objectMins = new List<Vector3>();
        objectMaxs = new List<Vector3>();
        isCollidingList = new List<bool>();

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
}


