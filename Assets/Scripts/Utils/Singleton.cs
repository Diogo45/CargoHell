using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance { get; private set; }


    protected void Awake()
    {
        if (instance == null)
            instance = this as T;
        else
            Destroy(gameObject);
    }

}
