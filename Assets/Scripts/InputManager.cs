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
    public InputAction scrollWheel { get; private set; }
    public InputAction save { get; private set; }
    public InputAction delete { get; private set; }
    public InputAction shift { get; private set; }
    public InputAction move { get; private set; }


    private void Awake()
    {
        base.Awake();
        PlayerInputActionMap = controls.FindActionMap("Player");
        UIInputActionMap = controls.FindActionMap("UI");
        LevelCreatorInputActionMap = controls.FindActionMap("LevelCreator");

        PlayerInputActionMap.Enable();
        LevelCreatorInputActionMap.Enable();

        move = PlayerInputActionMap.FindAction("Move");

        clickAction = PlayerInputActionMap.FindAction("Interact");
        scrollWheel = UIInputActionMap.FindAction("ScrollWheel");

        save = LevelCreatorInputActionMap.FindAction("Save");
        delete = LevelCreatorInputActionMap.FindAction("Delete");
        shift = LevelCreatorInputActionMap.FindAction("Shift");



    }

    
}
