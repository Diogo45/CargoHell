using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreatorInput : Singleton<LevelCreatorInput>
{


    public Vector3 mousePosition { get; private set; }
    public bool mouseDown { get; private set; }



    private void Awake()
    {
        base.Awake();
    }


    private void Update()
    {

        mouseDown = Input.GetMouseButton(0);
        mousePosition = Input.mousePosition;

    }

   

}
