using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelMusic")]
public class LevelMusic : ScriptableObject
{
    [field: SerializeField]
    public AudioClip introClip;

    [field: SerializeField]
    public AudioClip loopClip;
}
