using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IDragHandler
{
    private RectTransform _draggableTransform;

    private void Awake()
    {
        _draggableTransform = GetComponent<RectTransform>();

    }

    // Start is called before the first frame update
    public void OnDrag(PointerEventData eventData)
    {
        _draggableTransform.anchoredPosition += eventData.delta / LevelCreatorUI.instance.Canvas.scaleFactor;
    }
}
