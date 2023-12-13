using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfiner : MonoBehaviour
{
    public Collider2D confineArea; // Assign the Collider2D that defines the boundary

    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, confineArea.bounds.min.x, confineArea.bounds.max.x),
            Mathf.Clamp(transform.position.y, confineArea.bounds.min.y, confineArea.bounds.max.y),
            transform.position.z);
    }
}