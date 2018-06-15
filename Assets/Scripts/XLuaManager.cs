using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
[Hotfix]
public sealed class XLuaManager : MonoSingleton<XLuaManager>
{
    LuaEnv luaEnv;
    // Use this for initialization
    void Awake()
    {
        luaEnv = new LuaEnv();
        if (luaEnv != null)
        {
            luaEnv.AddLoader(CustomLoader);
            LoadMian();
        }
    }
    public static byte[] CustomLoader(ref string filePath)
    {
        Debug.Log("Load xLua scprit:" + filePath);
        var bd= LoadAssetMrg.Instance.LoadAsset(filePath + ".lua.txt");
        TextAsset asset= bd.mAsset as TextAsset;
        if (asset != null)
            return asset.bytes;
        bd = null;
        return null;
    }
    void LoadMian()
    {
        try
        {
            if (luaEnv != null)
                luaEnv.DoString("require 'Main'");
        }
        catch (System.Exception ex)
        {
            string msg = string.Format("xlua exception :{0}\n {1}",ex.Message,ex.StackTrace);
            Debug.LogError(msg);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (luaEnv != null)
        {
            luaEnv.Tick();
            if (Time.frameCount % 100 == 0)
            {
                luaEnv.FullGc();
            }
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        if (luaEnv != null)
        {
            luaEnv.Dispose();
            luaEnv = null;
        }
    }
}
