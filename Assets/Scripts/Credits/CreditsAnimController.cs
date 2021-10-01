using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsAnimController : MonoBehaviour
{
    [SerializeField]
    private Material nebulaMat;

    [SerializeField]
    private Color nebulaColor;

    void Start()
    {
        nebulaMat.SetColor("_Color", nebulaColor);
    }

    void Update()
    {
        
    }
}
