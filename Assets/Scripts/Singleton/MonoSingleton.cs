using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T> {

    private static T instance=null;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    static MonoSingleton()
    {
        instance = FindObjectOfType(typeof(T)) as T;
        if (instance == null)
        {
            GameObject go = new GameObject(typeof(T).Name);
            instance = go.AddComponent<T>();
            DontDestroyOnLoad(go);
        }
    }

    // Use this for initialization
    public void StartUp () {
		
	}
	
	private void DisposeSelf()
    {
        MonoSingleton<T>.instance = null;
        Destroy(gameObject);
    }
    public virtual void Dispose()
    {
        DisposeSelf();
    }
}
