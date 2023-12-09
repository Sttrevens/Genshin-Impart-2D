using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour, IPointerClickHandler
{
    public GameObject dialogueObject;
    private GameObject clickedObject;

    public UIManager uIManager;

    private void Start()
    {
        // Store the reference to this object's GameObject
        clickedObject = this.gameObject;

        // Make sure the dark background is not visible at the start
        dialogueObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        dialogueObject.SetActive(true);
        string nPC = "NPC";
        float distance = 5f;
        uIManager.AddTaggedUIElement(nPC, dialogueObject, distance);
        uIManager.RemoveTaggedUIElement(clickedObject);
        clickedObject.SetActive(false);
    }
}