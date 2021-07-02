using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private InputActionAsset controls;

    public InputActionMap PlayerInputActionMap { get; private set; }
    public InputActionMap UIInputActionMap { get; private set; }
    public InputActionMap LevelCreatorInputActionMap { get; private set; }

    public InputAction clickAction { get; private set; }
    public InputAction holdAction { get; private set; }
    public InputAction scrollWheel { get; private set; }


    private void Awake()
    {
        base.Awake();
        PlayerInputActionMap = controls.FindActionMap("Player");
        UIInputActionMap = controls.FindActionMap("UI");
        LevelCreatorInputActionMap = controls.FindActionMap("LevelCreator");

        clickAction = PlayerInputActionMap.FindAction("Interact");
        holdAction = UIInputActionMap.FindAction("HoldClick");

        scrollWheel = UIInputActionMap.FindAction("ScrollWheel");

    }

    
}
