using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LevelCreatorUI : Singleton<LevelCreatorUI>
{

    [SerializeField] private EnemyList _enemyList;
    [SerializeField] private EnemyType _selectedEnemyPrefab;

    [SerializeField] private GameObject _nextWaveButton;
    [SerializeField] private GameObject _previousWaveButton;

    [SerializeField] private GameObject _enemyInfoUI;


    [field: SerializeField]
    public Canvas Canvas { get; private set; }



    private LevelCreator _levelCreator;

    private Mouse _mouse;

    private Camera Camera;

    private void Awake()
    {
        base.Awake();

        _selectedEnemyPrefab = EnemyType.NONE;

        _levelCreator = LevelCreator.instance;

        _mouse = Mouse.current;

        Camera = Camera.main;

    }

    public void OnMouseClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (ShouldEditEnemy(ctx) || _selectedEnemyPrefab == EnemyType.NONE)
        {
            return;
        }

        var mousePos = Camera.main.ScreenToWorldPoint(_mouse.position.ReadValue());

        var pointerEventData = new PointerEventData(EventSystem.current) { position = _mouse.position.ReadValue() };
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            var result = raycastResults[0];
            //Debug.Log(result.gameObject.name);
            if (result.gameObject.tag == "SpawnableArea")
            {
                GameObject instance = _enemyList[_selectedEnemyPrefab];
                GameObject newEnemy = Instantiate(instance, mousePos, Quaternion.identity);
                newEnemy.transform.Translate(0, 0, 95);
                LevelCreator.instance.AddEnemy(_selectedEnemyPrefab, _mouse.position.ReadValue());
            }

        }


    }

    private bool ShouldEditEnemy(InputAction.CallbackContext ctx)
    {


        var mousePos = Camera.main.ScreenToWorldPoint(_mouse.position.ReadValue());

        RaycastHit2D hit = Physics2D.GetRayIntersection( new Ray { origin = mousePos, direction = Vector3.forward });

        if (hit && hit.transform.tag == "Enemy")
        {
          

            return true;
        }


        return false;

    }

    public void OnChangeEnemyType(EnemyType type)
    {
        _selectedEnemyPrefab = type;
    }

}
