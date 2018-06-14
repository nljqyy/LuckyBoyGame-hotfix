using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

[LuaCallCSharp]
public class CoroutineRuner : MonoBehaviour {

    public void YiledAndCallback(object o,Action callback)
    {
        StartCoroutine(CoBody(o,callback));
    }
    private IEnumerator CoBody(object o, Action callback)
    {
        yield return o;
        callback();
    }
}
