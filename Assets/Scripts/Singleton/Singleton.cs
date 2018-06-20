using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public abstract class Singleton<T> where T : class
{
    private static T instance = null;
    public static T Instance
    {
        get {
            return Singleton<T>.instance;
        }
    }
    static Singleton()
    {
        if (Singleton<T>.instance == null)
        {
           ConstructorInfo[] constructors=typeof(T).GetConstructors(
               BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            ConstructorInfo constructor = null;
            foreach (var item in constructors)
            {
                if (item.GetParameters().Length==0)
                {
                    constructor = item;
                    break;
                }
            }
            if (constructor != null)
            {
                Singleton<T>.instance=(T)constructor.Invoke(null);
            }
        }
    }

    public virtual void Dispose()
    {
        Singleton<T>.instance = null;
    }
}
