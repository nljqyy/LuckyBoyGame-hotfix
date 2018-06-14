using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;
using System.Reflection;

public static class XLuaHelper
{
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(XLuaHelper),
        typeof(Array),
        typeof(IList),
        typeof(IDictionary),
        typeof(Activator),
        typeof(Type),
        typeof(BindingFlags),
    };
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    { };

    //创建array
    public static Array CreateArrayInstance(Type itemType, int itemCount)
    {
        return Array.CreateInstance(itemType, itemCount);
    }

    public static IList CreateListInstance(Type itemType)
    {
        return (IList)Activator.CreateInstance(GetGenericListType(itemType));
    }

    public static IDictionary CreateDictionaryInstance(Type keyType,Type valueType)
    {
        return (IDictionary)Activator.CreateInstance(GetGenericDictionaryType(keyType,valueType));
    }

    public static Delegate CreateActionDelegate(Type type,string methodName,params Type[] paramTypes)
    {
       return InnerCreateDelegate(GetGenericActionType, null,type,methodName,paramTypes);
    }

    delegate Type MakeGenericDelegeteType(params Type[] parmsType);
    static Delegate InnerCreateDelegate(MakeGenericDelegeteType del,object target,Type type,string method, params Type[] paramTypes)
    {
        if (target!=null)
        {
            type = target.GetType();
        }
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        MethodInfo methodinfo = (paramTypes == null || paramTypes.Length == 0) ? type.GetMethod(method, bindingFlags) : type.GetMethod(method,bindingFlags,null,paramTypes,null);
        Type delegateType = del(paramTypes);
        return Delegate.CreateDelegate(delegateType, target, methodinfo);
    }

    public static Type GetGenericListType(Type itemType)
    {
        return typeof(List<>).MakeGenericType(itemType);
    }
    public static Type GetGenericDictionaryType(Type keyType,Type valueType)
    {
        return typeof(Dictionary<,>).MakeGenericType(keyType,valueType);
    }

    public static Type GetGenericActionType(params Type[] parmsType)
    {
        if (parmsType == null || parmsType.Length == 0)
            return typeof(Action);
        else if(parmsType.Length==1)
            return typeof(Action<>).MakeGenericType(parmsType);
        else if(parmsType.Length==2)
            return typeof(Action<,>).MakeGenericType(parmsType);
        else if(parmsType.Length==3)
            return typeof(Action<,,>).MakeGenericType(parmsType);
        return null;
    }
}
