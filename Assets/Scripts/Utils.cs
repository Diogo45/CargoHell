using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

    public static IEnumerator ActivateBehaviour( float delay, params Behaviour[] components)
    {

        yield return new WaitForSeconds(delay);

        foreach (var item in components)
        {
            item.enabled = true;
        }

        yield break;
    }
    //For some reason Unity has "enabled for both Renderer and Behaviour witch both inherit from Component, whitch DOES NOT FUCKING have enabled, so we need two separate functions
    public static IEnumerator ActivateRenderer(float delay, params Renderer[] components)
    {
        yield return new WaitForSeconds(delay);

        foreach (var item in components)
        {
            item.enabled = true;
        }

        yield break;
    }

    public static IEnumerator StrobeColor(GameObject obj, Color color, int strobes)
    {
        Renderer sprt = obj.GetComponent<Renderer>();
        Material mat = sprt.material;
        if (!mat) yield break;

        for (int i = 0; i < strobes; i++)
        {
            color.a = 1f;
            SetColor(mat, color);
            yield return new WaitForSeconds(0.1f);
            color.a = 0f;
            SetColor(mat, color);
            yield return new WaitForSeconds(0.1f);
        }

        yield break;
    }

    public static void SetColor(Material material, Color color)
    {
        material.SetColor("_Color", color);
    }

}
