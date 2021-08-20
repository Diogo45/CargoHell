using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Rotatable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private float _speed;
    private bool _shiftHeld;
    private bool _selected;

    private Vector2 _rotate;

    private void OnEnable()
    {
        InputManager.instance.scrollWheel.performed += Rotate;
        InputManager.instance.shift.performed += Shift_performed;
        InputManager.instance.shift.canceled += Shift_canceled; ;
    }



    private void OnDisable()
    {
        InputManager.instance.scrollWheel.performed -= Rotate;
        InputManager.instance.shift.performed -= Shift_performed;
        InputManager.instance.shift.canceled -= Shift_canceled;
    }


    private void Shift_canceled(InputAction.CallbackContext obj)
    {
        _shiftHeld = false;
    }

    private void Shift_performed(InputAction.CallbackContext obj)
    {
        if (obj.interaction is HoldInteraction)
        {
            _shiftHeld = true;
        }
    }


    private void Rotate(InputAction.CallbackContext ctx)
    {
        if (!_selected) return;

        _rotate = ctx.ReadValue<Vector2>();

        float rotate = Mathf.Round(_rotate.normalized.x + _rotate.normalized.y);

        //rotate = Mathf.Clamp(rotate, -1f, 1f);
     
        transform.Rotate(Vector3.forward, rotate * _speed);

        var rotation = transform.eulerAngles;

        if (_shiftHeld)
            transform.eulerAngles = new Vector3(rotation.x, rotation.y, Mathf.Round(rotation.z / _speed) * _speed);
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
