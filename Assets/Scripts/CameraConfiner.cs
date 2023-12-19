using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void GoToNextScene()
    {
        // Assuming you want to go to the next scene in the build settings
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}