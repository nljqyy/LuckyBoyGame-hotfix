using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public sealed class UIAtlasManager
{
    private const string suffixName = ".asset";
    private static Dictionary<string, Sprite[]> atlasDic = new Dictionary<string, Sprite[]>();

    public static Sprite LoadSprite(string atlasName, string spriteName)
    {
        Sprite sp = FindSprite(atlasName, spriteName);
        if (sp == null)
        {
            Sprite[] sps = null;
            string newPath = atlasName + suffixName;
            Bundle bd = LoadAssetMrg.Instance.LoadAsset(newPath);
            if (UIAtlasName.UIMain == atlasName)
                sps = (bd.mAsset as UIAtlasMain_Scriptable).sprites;
            else
                sps = (bd.mAsset as UIAtlasQRCode_Scriptable).sprites;
            sp = GetSpriteForAtlas(sps, spriteName);
            atlasDic.Add(atlasName, sps);
            LoadAssetMrg.Instance.ReleaseAsset(newPath);
        }
        return sp;
    }

    private static Sprite FindSprite(string atlasName, string spriteName)
    {
        if (atlasDic.ContainsKey(atlasName))
        {
            Sprite[] sp = atlasDic[atlasName];
            return GetSpriteForAtlas(sp, spriteName);
        }
        return null;
    }

    private static Sprite GetSpriteForAtlas(Sprite[] sps, string spriteName)
    {
        for (int i = 0; i < sps.Length; i++)
        {
            if (sps[i].GetType() == typeof(Sprite))
            {
                if (sps[i].name == spriteName)
                    return sps[i];
            }
        }
        Debug.LogWarning("图片名:" + spriteName + ";在图集中找不到");
        return null;
    }

    public static void Clear()
    {
        foreach (var item in atlasDic)
        {
            foreach (var sp in item.Value)
            {
                Resources.UnloadAsset(sp);
            }
        }
        atlasDic.Clear();
    }
}
