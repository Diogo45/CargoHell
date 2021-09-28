using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoHell;

public class LvlButtonColor : MonoBehaviour
{
   
    void Start()
    {
        Level level = LevelController.instance._level;
        var button = GetComponent<UnityEngine.UI.Button>();
        var lvlColor = level.ShaderVariables.color;
        var newColors = button.colors;
        newColors.highlightedColor = new Color(lvlColor.r * 1.5f, lvlColor.g * 1.5f, lvlColor.b * 1.5f);
        newColors.pressedColor = lvlColor;
        button.colors = newColors;
    }

}
