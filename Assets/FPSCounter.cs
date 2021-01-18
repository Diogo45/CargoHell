using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{

    public TMPro.TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();
        StartCoroutine(UpdateFPS());
    }

    IEnumerator UpdateFPS()
    {
        yield return new WaitForSeconds(0.1f);
         
        float fps = 1 / Time.deltaTime;
        text.text = String.Format("{0,2:F1} FPS", fps);

        yield return UpdateFPS();
    }

}
