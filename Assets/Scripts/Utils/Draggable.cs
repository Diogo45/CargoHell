using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Draggable : MonoBehaviour
{
    private bool _held;

    private GameObject _selectedObject;

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
            _selectedObject = null;
        }
    }

    private void Update()
    {
        if (_held)
        {
            var pos = Mouse.current.position.ReadValue();

            _selectedObject.transform.position = _camera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, _selectedObject.transform.position.z));
        }
    }

    private void HoldOrClickPerformed(InputAction.CallbackContext ctx)
    {

        if (ctx.interaction is HoldInteraction)
        {
            if (LevelCreator.HitEnemy(out GameObject hit))
            {
                _held = true;

                _selectedObject = hit;
            }

          
        }
   
    }

    

}
