using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class LevelCreatorUI : Singleton<LevelCreatorUI>
{

    [field: SerializeField]
    public Canvas Canvas { get; private set; }

    [SerializeField] private EnemyList _enemyList;
    [SerializeField] private EnemyType _selectedEnemyPrefab;
    [SerializeField] private RectTransform _enemyTab;

    private List<GameObject> _waveEnemies;

    [SerializeField] private Button _nextWaveButton;
    [SerializeField] private Button _previousWaveButton;
    [SerializeField] private Button _addWaveButton;
    [SerializeField] private Button _removeWaveButton;

    [SerializeField] private TMPro.TMP_Text _waveText;

    
    [SerializeField] private GameObject _enemyInfoUI;

    [SerializeField, Range(-5f, 5f)] private float _xOffset;
    [SerializeField, Range(-5f, 5f)] private float _yOffset;

    [field: SerializeField] public EnemyData _selectedObject { get; private set; }

    private LevelCreator _levelCreator;

    private Mouse _mouse;


    private void Awake()
    {
        base.Awake();
    }


    private void Start()
    {

        InputManager.instance.clickAction.performed += HoldOrClickPerformed;
        InputManager.instance.save.performed += Save;
        InputManager.instance.delete.performed += Delete_performed;

        _waveEnemies = new List<GameObject>();

        ReadCurrentWave();
        UpdateWaveText();

        _nextWaveButton.onClick.AddListener(NextWave);
        _previousWaveButton.onClick.AddListener(PrevWave);
        _addWaveButton.onClick.AddListener(AddWave);
        _removeWaveButton.onClick.AddListener(RemoveWave);


        _selectedEnemyPrefab = EnemyType.NONE;

        _levelCreator = LevelCreator.instance;

        _mouse = Mouse.current;
    }

    private void Delete_performed(InputAction.CallbackContext obj)
    {
        if(LevelCreator.HitEnemy(out GameObject hit))
        {
            if(hit != null)
            {
                _waveEnemies.Remove(hit);
                Destroy(hit);
            }
        }
    }

    private void HoldOrClickPerformed(InputAction.CallbackContext ctx)
    {

        if (!(ctx.interaction is HoldInteraction))
        {
            OnClick();
        }
       
    }

    public void UpdateWaveText()
    {
        _waveText.text = String.Format("{0:00}|{1:00}", LevelCreator.instance.WaveNumber + 1, LevelCreator.instance._level.LevelConfig.Count);
    }

    public void AddWave()
    {
        LevelCreator.instance.AddWave();

        NextWave();

        UpdateWaveText();
    }

    public void RemoveWave()
    {
        LevelCreator.instance.DeleteWave();

        PrevWave();

        UpdateWaveText();
    }

    public void NextWave()
    {
        LevelCreator.instance.NextWave();

        ReadCurrentWave();

        UpdateWaveText();
    }

    public void PrevWave()
    {
        LevelCreator.instance.PreviousWave();

        ReadCurrentWave();

        UpdateWaveText();
    }

    private void ReadCurrentWave()
    {

        foreach (var item in _waveEnemies)
        {
            Destroy(item);
        }

        _waveEnemies.Clear();

        var enemies = LevelCreator.instance.currentWaveEnemies();

        foreach (var item in enemies)
        {
            var enemy = Instantiate(_enemyList[item.enemyType], Camera.main.ViewportToWorldPoint(item.viewportPosition), Quaternion.identity);

            var data = enemy.GetComponent<EnemyData>();

            data.Delay = item.delay;
            data.enemyType = item.enemyType;
            data.ShouldMove = item.shouldMove;
            data.Speed = item.speed;

            enemy.transform.Translate(0, 0, 95);
            enemy.transform.up = item.direction;
            _waveEnemies.Add(enemy);

        }


    }

    public void OnClick()
    {
        //if (!value) return;


        if (LevelCreator.HitEnemy(out GameObject hit) || _selectedEnemyPrefab == EnemyType.NONE)
        {
            if (hit != null)
                HandleMoveEditWindow(hit);
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
        _waveEnemies.Add(newEnemy);
        var data = newEnemy.GetComponent<EnemyData>();
        data.enemyType = _selectedEnemyPrefab;


    }



    public void Save(InputAction.CallbackContext ctx)
    {
        LevelCreator.instance.Save(_waveEnemies);
    }

    



    private void HandleMoveEditWindow(GameObject hit)
    {
        _selectedObject = hit.GetComponent<EnemyData>();

        _enemyInfoUI.SetActive(false);

        _enemyInfoUI.SetActive(true);

        var pos = new Vector3(hit.transform.position.x + _xOffset, hit.transform.position.y + _yOffset, 0f);
        pos = Canvas.transform.InverseTransformPoint(pos);
        pos = new Vector3(pos.x, pos.y, 0f);

        _enemyInfoUI.GetComponent<RectTransform>().anchoredPosition = pos;

    }

    public void OnChangeEnemyType(EnemyType type)
    {
        _selectedEnemyPrefab = type;
    }

    public void ToggleEnemyTab(bool value)
    {
        _enemyTab.gameObject.SetActive(value);


    }

}
