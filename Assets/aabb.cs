using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aabb : MonoBehaviour
{

    public GameObject object1;
    public GameObject object2;

    private Vector3 object1Min;
    private Vector3 object1Max; 
    private Vector3 object2Min;
    private Vector3 object2Max;

    private bool isColliding;

    private void Start()
    {
        // Calculate the initial bounding box for each object
        CalculateBoundingBox(object1, out object1Min, out object1Max);
        CalculateBoundingBox(object2, out object2Min, out object2Max);
    }

    private void Update()
    {
        // Update the bounding boxes as the objects might move
        CalculateBoundingBox(object1, out object1Min, out object1Max);
        CalculateBoundingBox(object2, out object2Min, out object2Max);

        // Perform AABB collision detection
        if (object1Max.x >= object2Min.x && object1Min.x <= object2Max.x &&
            object1Max.y >= object2Min.y && object1Min.y <= object2Max.y &&
            object1Max.z >= object2Min.z && object1Min.z <= object2Max.z)
        {
            // Objects are colliding
            if (!isColliding)
            {
                isColliding = true;
                Debug.Log("Collision Detected!");
            }
        }
        else
        {
            // Objects are not colliding
            if (isColliding)
            {
                isColliding = false;
            }
        }
    }

    private void CalculateBoundingBox(GameObject obj, out Vector3 min, out Vector3 max)
    {
        // Calculate the minimum and maximum points of the object's bounding box
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
