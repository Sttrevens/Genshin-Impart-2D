using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowLamp : MonoBehaviour, IPointerClickHandler
{
    public GameObject broomLampObject;
    public GameObject awesomeDialogue;
    public GameObject bbDialogue;
    private GameObject clickedObject;

    public UIManager uiManager;

    private void Start()
    {
        clickedObject = this.gameObject;

        broomLampObject.SetActive(false);
        awesomeDialogue.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiManager != null)
        {
            uiManager.RemoveTaggedUIElement(bbDialogue);
        }

        bbDialogue.SetActive(false);
        broomLampObject.SetActive(true);
        awesomeDialogue.SetActive(true);

        clickedObject.SetActive(false);
    }
}