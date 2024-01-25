using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform target; // Assign the target in the inspector
    private Vector3 distance;

    void Start()
    {
        distance = target.position - transform.position;
    }

    void Update()
    {
        // Move towards the target
        transform.position = target.position - distance;
    }
}