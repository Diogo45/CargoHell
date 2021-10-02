using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class NebulaExposeProprieties : MonoBehaviour
{
    [SerializeField] private Vector4 _tilling;
    [SerializeField] private Vector4 _scrollSpeed;

    private Vector4 __tilling;
    private Vector4 __scrollSpeed;

    private void OnEnable()
    {
        GetComponent<Image>().material.SetVector("_ScrollSpeed", new Vector4(0.05f, 0.05f, 1, 1));
        GetComponent<Image>().material.SetVector("_Tilling", new Vector4(1f, 1, 1, 1));

        __tilling = GetComponent<Image>().material.GetVector("_Tilling");
        __scrollSpeed = GetComponent<Image>().material.GetVector("_ScrollSpeed");
    }

    private void LateUpdate()
    {
        if (_tilling != __tilling)
        {
            __tilling = _tilling;
            GetComponent<Image>().material.SetVector("_Tilling", _tilling);
        }

        if (_scrollSpeed != __scrollSpeed)
        {
            //__scrollSpeed += new Vector4(1f, 0, 0, 0) / 50f;
            __scrollSpeed = _scrollSpeed;
            GetComponent<Image>().material.SetVector("_ScrollSpeed", _scrollSpeed);
        }

    }
}
