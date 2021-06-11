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


    [field: SerializeField] public EnemyData _selectedObject { get; private set; }

    [SerializeField] private GameObject _nextWaveButton;
    [SerializeField] private GameObject _previousWaveButton;

    [SerializeField] private GameObject _enemyInfoUI;

    [SerializeField, Range(-5f, 5f)] private float _xOffset;
    [SerializeField, Range(-5f, 5f)] private float _yOffset;

    [field: SerializeField]
    public Canvas Canvas { get; private set; }

    private LevelCreator _levelCreator;

    private Mouse _mouse;


    private void Awake()
    {
        base.Awake();

        _selectedEnemyPrefab = EnemyType.NONE;

        _levelCreator = LevelCreator.instance;

        _mouse = Mouse.current;


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
            return;
        }


        GameObject instance = _enemyList[_selectedEnemyPrefab];
        GameObject newEnemy = Instantiate(instance, mousePos, Quaternion.identity);
        newEnemy.transform.Translate(0, 0, 95);
        LevelCreator.instance.AddEnemy(_selectedEnemyPrefab, _mouse.position.ReadValue());




    }

    private bool ShouldEditEnemy(InputAction.CallbackContext ctx)
    {


        var mousePos = Camera.main.ScreenToWorldPoint(_mouse.position.ReadValue());

        RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray { origin = mousePos, direction = Vector3.forward });

        if (hit && hit.transform.tag == "Enemy")
        {
            _selectedObject = hit.transform.gameObject.GetComponent<EnemyData>();

            HandleMoveEditWindow(hit);

            return true;
        }
        else if (!hit)
        {
            //_enemyInfoUI.SetActive(false);

        }




        return false;

    }

    private void HandleMoveEditWindow(RaycastHit2D hit)
    {
        _enemyInfoUI.SetActive(false);

        _enemyInfoUI.SetActive(true);

        var pos = new Vector3(hit.point.x + _xOffset, hit.point.y + _yOffset, 0f);
        pos = Canvas.transform.InverseTransformPoint(pos);
        pos = new Vector3(pos.x, pos.y, 0f);

        _enemyInfoUI.GetComponent<RectTransform>().anchoredPosition = pos;

    }

    public void OnChangeEnemyType(EnemyType type)
    {
        _selectedEnemyPrefab = type;
    }

}
