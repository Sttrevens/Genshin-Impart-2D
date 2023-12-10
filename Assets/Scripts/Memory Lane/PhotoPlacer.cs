using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoPlacer : MonoBehaviour
{
    public GameObject oldPhotoPrefab; // Assign your old photo prefab in the inspector
    private GameObject currentPhotoInstance;
    private bool isPlacing = false;
    public Collider2D boundaryCollider;

    public bool isHoldingPhoto = false;

    public GameObject successPanel;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isHoldingPhoto)
        {
            if (isPlacing && currentPhotoInstance != null)
            {
                // Make the photo follow the cursor
                Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentPhotoInstance.transform.position = cursorPosition;

                // When the player clicks, place the photo
                if (Input.GetMouseButtonDown(0))
                {
                    // Optionally, you can add additional logic here to snap the photo
                    // to a grid or to specific positions, if necessary.

                    isPlacing = false; // The photo is now placed
                    //CheckPlacement(currentPhotoInstance); // Check if placed correctly
                    currentPhotoInstance = null; // Reset the current photo instance
                    isHoldingPhoto = false;
                }
            }
            else
            {
                    currentPhotoInstance = Instantiate(oldPhotoPrefab);
                    isPlacing = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (boundaryCollider.OverlapPoint(cursorPosition))
            {
                Debug.Log("Cursor is in the right area!");
                successPanel.SetActive(true);
            }
            else
            {
                Debug.Log("Cursor is not in the right area, try again.");
                Destroy(currentPhotoInstance);
            }
        }
    }

    //private void CheckPlacement(GameObject placedPhoto)
    //{
    //    Collider2D boundaryCollider = boundaryObject.GetComponent<Collider2D>();
    //    Collider2D photoCollider = placedPhoto.GetComponent<Collider2D>();

    //    if (boundaryCollider != null && photoCollider != null)
    //    {
    //        // Get the bounds of both colliders
    //        Bounds boundaryBounds = boundaryCollider.bounds;
    //        Bounds photoBounds = photoCollider.bounds;

    //        // Check if the photo bounds are completely inside the boundary bounds
    //        if (boundaryBounds.Contains(photoBounds.min) && boundaryBounds.Contains(photoBounds.max))
    //        {
    //            Debug.Log("Photo placed correctly! You win!");
    //            // Trigger win condition here
    //        }
    //        else
    //        {
    //            Debug.Log("Photo not placed correctly, try again.");
    //            Destroy(placedPhoto); // Remove the incorrectly placed photo
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Either boundary or photo is missing a Collider2D component.");
    //    }
    //}
}
