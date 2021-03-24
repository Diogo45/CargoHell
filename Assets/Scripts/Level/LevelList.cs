using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelList : ScriptableObject
{
    [field: SerializeField]
    public Level[] _levels { get; private set; }

    public Level this[int i]
    {
        get => _levels[i];
        set => _levels[i] = value;
    }

}
