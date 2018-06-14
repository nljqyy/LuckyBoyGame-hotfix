using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class EventHandler
{
    private static Dictionary<string, Action<object>> dic = new Dictionary<string, Action<object>>();
    public static void RegisterEvnet(string type, Action<object> action)
    {
        Action<object> act = GetRegAction(type);
        act += action;
        dic[type] = act;
    }
    public static void UnRegisterEvent(string type, Action<object> action)
    {
        Action<object> act = GetRegAction(type);
        if (act != null)
        {
            act -= action;
            dic[type] = act;
        }
    }

    public static void ExcuteEvent(string type, object obj)
    {
        Action<object> act = GetRegAction(type);
        if (act != null)
            act(obj);
    }

    private static Action<object> GetRegAction(string type)
    {
        Action<object> act = null;
        dic.TryGetValue(type, out act);
        return act;
    }

    public static void Clear()
    {
        List<Action<object>> temp = dic.Values.ToList();
        for (int i = 0; i < temp.Count; i++)
        {
            temp[i] = null;
        }
        dic.Clear();
    }

}
