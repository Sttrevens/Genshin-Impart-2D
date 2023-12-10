using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Get the DraggableItem script from the dragged object
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggedItem != null && eventData.pointerDrag.tag == draggedItem.targetTag)
        {
            // The dragged item has been dropped over this drop zone
            // You can perform additional checks here, like ensuring it's centrally placed if needed
            // Call the OnItemPlaced method to handle the correct placement
            draggedItem.OnItemPlaced();
        }
    }
}
