using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInitialValueINT : MonoBehaviour
{

    public string PlayerPrefKey;
    void Start()
    {
        GetComponent<TMPro.TMP_Dropdown>().value = PlayerPrefs.GetInt(PlayerPrefKey);
    }

}
