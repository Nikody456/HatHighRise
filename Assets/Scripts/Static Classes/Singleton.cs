using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new()
{
    static T instance;
    public static T Instance => GetInstance();

    protected Singleton() { }

    protected static T GetInstance()
    {
        if (instance == null)
        {
            instance = new T();
            instance.OnInitialize();
        }

        return instance;
    }

    protected virtual void OnInitialize() { }
}
