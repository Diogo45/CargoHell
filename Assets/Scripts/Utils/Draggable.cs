using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Draggable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _held;
    private bool _selected;

    private Camera _camera;

    private void Start()
    {
        InputManager.instance.clickAction.performed += HoldOrClickPerformed;
        InputManager.instance.clickAction.canceled += ClickAction_canceled;

        _camera = Camera.main;

    }

    private void ClickAction_canceled(InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is HoldInteraction)
        {
            _held = false;
            
        }
    }

    private void Update()
    {
        if (_held && _selected)
        {
            var pos = Mouse.current.position.ReadValue();

            transform.position = _camera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, transform.position.z));
        }
    }

    private void HoldOrClickPerformed(InputAction.CallbackContext ctx)
    {

        if (ctx.interaction is HoldInteraction)
        {

            _held = true;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _selected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _selected = false;
    }
}
