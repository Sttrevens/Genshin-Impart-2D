using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public struct TaggedUI
    {
        public string tag; // Tag to check for proximity
        public GameObject uiElement; // UI element to show/hide
        public float activationDistance; // Distance within which the UI element should be active
    }

    public List<TaggedUI> taggedUIElements = new List<TaggedUI>();
    public Transform playerTransform; // Player's transform

    private void Update()
    {
        foreach (var taggedUI in taggedUIElements)
        {
            // Find all objects with the specified tag
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(taggedUI.tag);
            bool isPlayerClose = false;

            foreach (var obj in taggedObjects)
            {
                // Check the distance between the player and the tagged object
                if (Vector3.Distance(playerTransform.position, obj.transform.position) <= taggedUI.activationDistance)
                {
                    isPlayerClose = true;
                    break; // Player is close to at least one object, no need to check further
                }
            }

            // Show or hide the UI element based on player proximity
            taggedUI.uiElement.SetActive(isPlayerClose);
        }
    }

    public void AddTaggedUIElement(string tag, GameObject uiElement, float activationDistance)
    {
        TaggedUI newTaggedUI = new TaggedUI
        {
            tag = tag,
            uiElement = uiElement,
            activationDistance = activationDistance
        };

        taggedUIElements.Add(newTaggedUI);
    }

    public void RemoveTaggedUIElement(GameObject uiElement)
    {
        // Find the element in the list
        TaggedUI elementToRemove = taggedUIElements.Find(tui => tui.uiElement == uiElement);

        // If found, remove it
        if (elementToRemove.uiElement != null)
        {
            taggedUIElements.Remove(elementToRemove);
        }
    }
}