using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyInfoUIController : MonoBehaviour, IDragHandler
{

    private RectTransform _draggableTransform;


    [SerializeField] private GameObject _title;
    [SerializeField] private GameObject _position;
    [SerializeField] private GameObject _direction;
    [SerializeField] private GameObject _speed;
    [SerializeField] private GameObject _delay;
    [SerializeField] private GameObject _shouldMove;

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
