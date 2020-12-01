using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{    
    private void Update()
    {
        //Not the best way to push parameters to materials, this feels like busywork if i ever want to scale it up.
        // Input point
        var mouse_input = new Vector2(
            (Input.mousePosition.x) / Screen.height,
            (Input.mousePosition.y) / Screen.height
        );
        Shader.SetGlobalVector("_MousePosition", mouse_input);
        
        if (Input.GetMouseButtonDown(0))
            Shader.SetGlobalVector("_MouseLClickDown", mouse_input);

        if (Input.GetMouseButtonUp(0))
            Shader.SetGlobalVector("_MouseLClickUp", mouse_input);

         Shader.SetGlobalInt("_MouseL", Input.GetMouseButton(0) ? 1 : 0);

        if (Input.GetMouseButtonDown(1))
            Shader.SetGlobalVector("_MouseRClickDown", mouse_input);

        if (Input.GetMouseButtonUp(1))
            Shader.SetGlobalVector("_MouseRClickUp", mouse_input);

        Shader.SetGlobalInt("_MouseR", Input.GetMouseButton(1) ? 1 : 0);
        
        
        if (Input.GetMouseButtonDown(2))
            Shader.SetGlobalVector("_MouseMClickDown", mouse_input);

        if (Input.GetMouseButtonUp(2))
            Shader.SetGlobalVector("_MouseMClickUp", mouse_input);

        Shader.SetGlobalInt("_MouseM", Input.GetMouseButton(2) ? 1 : 0);
    }



}
