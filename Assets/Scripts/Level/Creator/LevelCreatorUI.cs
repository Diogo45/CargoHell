using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelCreatorUI : Singleton<LevelCreatorUI>
{
   
    [SerializeField] private EnemyList _enemyList;
    [SerializeField] private EnemyType _selectedEnemyPrefab;

    [SerializeField] private GameObject _nextWaveButton;
    [SerializeField] private GameObject _previousWaveButton;

    private LevelCreator _levelCreator;

    private Mouse _mouse;

    private void Awake()
    {
        base.Awake();

        _levelCreator = LevelCreator.instance;

        _mouse = Mouse.current;

    }

    public void OnMouseClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        GameObject instance = _enemyList[_selectedEnemyPrefab];
        GameObject newEnemy = Instantiate(instance, Camera.main.ScreenToWorldPoint(_mouse.position.ReadValue()), Quaternion.identity);
        newEnemy.transform.Translate(0, 0, 10);
        LevelCreator.instance.AddEnemy(_selectedEnemyPrefab, _mouse.position.ReadValue());
    }
    
    
}
