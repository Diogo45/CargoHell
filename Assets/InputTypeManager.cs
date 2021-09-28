using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTypeManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown InputType;

  

    private void Start()
    {
        InputType.onValueChanged.AddListener(ChangeInputType);
    }


    private void ChangeInputType(int type)
    {
        PlayerPrefs.SetInt("InputType", type);
    }

}
