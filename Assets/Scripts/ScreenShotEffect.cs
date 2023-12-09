using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScreenshotEffect : MonoBehaviour, IPointerClickHandler
{
    public GameObject darkBackground; // Assign in Inspector
    private GameObject clickedObject; // Reference to the clicked GameObject itself

    public bool isTakeBroomPhoto = false;
    public bool isTakeLampPhoto = false;

    private void Start()
    {
        // Store the reference to this object's GameObject
        clickedObject = this.gameObject;

        // Make sure the dark background is not visible at the start
        darkBackground.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Deactivate this object
        clickedObject.SetActive(false);

        // Activate the dark background
        darkBackground.SetActive(true);

        // Optionally, set the dark background's alpha to a semi-transparent value
        darkBackground.GetComponent<Image>().color = new Color(0, 0, 0, 0.7f);
    }

    public void ReactivateImage()
    {
        // Reactivate this object
        clickedObject.SetActive(true);

        // Deactivate the dark background
        darkBackground.SetActive(false);
    }

    public void TakeBroomPhoto()
    {
        isTakeBroomPhoto = true;

        ReactivateImage();
    }

    public void TakeLampPhoto()
    {
        isTakeLampPhoto = true;
    }
}