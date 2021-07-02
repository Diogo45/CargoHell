using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelect : MonoBehaviour
{
    [SerializeField] private EnemyType _type;

    public void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        LevelCreatorUI.instance.OnChangeEnemyType(_type);
    }


}
