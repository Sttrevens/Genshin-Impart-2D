using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectType", menuName = "MapGeneration/ObjectType")]
public class ObjectType : ScriptableObject
{
    public GameObject[] objectPrefabs; // Different variants of the object
    public float spawnProbability; // Probability of this object being placed
    // Additional properties
}