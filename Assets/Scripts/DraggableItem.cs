using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public string targetTag; // The tag of the target game object

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.position;
        // Optional: Increase the block raycasts to allow events to pass through the dragged object
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // Adjust for the canvas scale factor
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore the block raycasts
        canvasGroup.blocksRaycasts = true;
        // Return to original position if not over the target
        //rectTransform.position = originalPosition;
    }

    // This method will be called by the DropZone script when the item is correctly placed
    public void OnItemPlaced()
    {
        // Do something when the item is placed correctly
        Debug.Log("Item placed correctly!");
        // Disable further dragging, if desired
        canvasGroup.blocksRaycasts = false;
    }
}