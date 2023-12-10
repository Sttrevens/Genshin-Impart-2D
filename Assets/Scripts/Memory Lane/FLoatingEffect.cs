using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float floatSpeed = 2f; // Speed of the float
    public float floatHeight = 10f; // Height of the float
    public float enlargementFactor = 1.1f; // Factor by which the image enlarges on hover

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Vector3 originalScale;
    private bool isHovering = false;
    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;
        startY = originalPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHovering)
        {
            // Float up and down over time
            float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            rectTransform.anchoredPosition = new Vector2(originalPosition.x, newY);
        }
    }

    // When cursor enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        rectTransform.anchoredPosition = originalPosition; // Reset to the original position
        rectTransform.localScale = originalScale * enlargementFactor; // Scale up the image
    }

    // When cursor exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        rectTransform.localScale = originalScale; // Reset to the original scale
    }
}
