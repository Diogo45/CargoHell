using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class EnemyList : ScriptableObject
{
    [SerializeField]
    private List<EnemyType> _types;

    [SerializeField]
    private List<GameObject> _obj;

    private Dictionary<EnemyType, GameObject> EnemyTypes;

    public GameObject this[EnemyType t]
    {
        get { return EnemyTypes[t]; }
    }

    public List<EnemyType> Keys
    {
        get
        {
            return new List<EnemyType>(EnemyTypes.Keys);
        }
       
    }

    private void OnValidate()
    {
        EnemyTypes = new Dictionary<EnemyType, GameObject>();
        for (int i = 0; i < _types.Count; i++)
        {
            EnemyTypes.Add(_types[i], _obj[i]);
        }
    }


}
