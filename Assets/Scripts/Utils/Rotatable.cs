using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Rotatable : MonoBehaviour
{

    [SerializeField] private float _speed;

    private void Start()
    {
        InputManager.instance.scrollWheel.performed += Rotate; 
    }

    private void Rotate(InputAction.CallbackContext ctx)
    {
        if(LevelCreator.HitEnemy(out GameObject obj))
        {
            //Debug.Log("ROTATE " + ctx.ReadValue<Vector2>());
            obj.transform.Rotate(Vector3.forward, ctx.ReadValue<Vector2>().y / _speed);
        }             
    }
}
