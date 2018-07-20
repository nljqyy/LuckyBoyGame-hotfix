using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using XLua;
using System;
using System.Linq;
[Hotfix]
public sealed class LoadAssetMrg :MonoSingleton<LoadAssetMrg>
{
    private void Awake()
    {
        GetMainfest();
    }
    private const string suffixName = ".bytes";
    private const string mMainfestName = "androidRes";
    private AssetBundle mMainfestBundle;
    private AssetBundleManifest mainfest;
    private bool isDisposed = false;
    private Dictionary<string, Bundle> bundles = new Dictionary<string, Bundle>();

    public override void Dispose()
    {
        Disposes(true);
        System.GC.SuppressFinalize(this);
        base.Dispose();
    }
    private void Disposes(bool dispose)
    {
        if (isDisposed) return;
        if (dispose)
        {
            //释放托管资源
        }
        //释放非托管资源
        ReleaseAllAsset();
        isDisposed = true;
    }
    ~LoadAssetMrg()
    {
        Disposes(false);
    }
    //获得Mainfest
    private void GetMainfest()
    {
        string mainPath = PathHelp.GetDownLoadPath()+PathHelp.unZip + mMainfestName;
        Debug.Log("加载mainfest---"+mainPath);
        mMainfestBundle = AssetBundle.LoadFromFile(mainPath);
        if (mMainfestBundle != null)
        {
            mainfest= mMainfestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            mMainfestBundle.Unload(false);
            mMainfestBundle = null;
        }
    }
    //获得资源依赖
    private string[] GetDirectDependencies(string _assetName)
    {
        if (mainfest == null) return null;
        return mainfest.GetDirectDependencies(_assetName+suffixName);
    }
    /// <summary>
    /// 根据依赖加载
    /// </summary>
    /// <param name="_assetName"></param>
    /// <param name="isAysnc">是否异步</param>
    public string[] LoadDependencies(string _assetName)
    {
        string[] deps = GetDirectDependencies(_assetName);
        string aName = "";
        for (int i = 0; i < deps.Length; i++)
        {
            aName = deps[i].Replace(suffixName, "");
            deps[i] = aName;
        }
        return deps;
    }

    /// <summary>
    /// 同步加载assetbundle
    /// </summary>
    /// <param name="_assetName"></param>
    /// <returns></returns>
    public Bundle LoadAsset(string _assetName)
    {
        if (string.IsNullOrEmpty(_assetName)) return null;
        Bundle bd = null;
        if (!bundles.TryGetValue(_assetName, out bd))
        {
            bd = new Bundle(_assetName);
            bd.GoLoad();
            bundles.Add(_assetName, bd);
            string[] _assets=LoadDependencies(_assetName);
            for (int i = 0; i < _assets.Length; i++)
            {
                LoadAsset(_assets[i]);
            }
        }
        bd.Retain();
        return bd;
    }
    /// <summary>
    /// 异步加载assetbundle
    /// </summary>
    /// <param name="_assetName"></param>
    /// <param name="action"></param>
    public void LoadAssetAsync(string _assetName,Action<Bundle> action)
    {
        StartCoroutine(LoadAssetIe(_assetName,action));
    }
    public IEnumerator LoadAssetIe(string _assetName, Action<Bundle> action)
    {
        if (string.IsNullOrEmpty(_assetName)) yield break;
        Bundle bd = null;
        if (!bundles.TryGetValue(_assetName, out bd))
        {
            bd = new Bundle(_assetName);
            bundles.Add(_assetName, bd);
            yield return bd.GoLoadAsync();
            string[] _assets = LoadDependencies(_assetName);
            for (int i = 0; i < _assets.Length; i++)
            {
                yield return LoadAssetIe(_assets[i], null);
            }
        }
        bd.Retain();
        if (action != null)
            action(bd);
    }
    /// <summary>
    /// 根据assetbundle移除
    /// </summary>
    /// <param name="_assetName"></param>
    public void Remove(string _assetName)
    {
        if (bundles.ContainsKey(_assetName))
            bundles.Remove(_assetName);
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="_assetName"></param>
    //释放资源
    public void ReleaseAsset(string _assetName)
    {
        if (bundles.ContainsKey(_assetName))
        {
            Bundle bd = bundles[_assetName];
            bd.Release();
            string[] deps = GetDirectDependencies(_assetName);
            foreach (var assetname in deps)
            {   
                ReleaseAsset(Bundle.DeleteSuffixName(assetname));
            }
        }
    }
   /// <summary>
   /// 释放所有资源
   /// </summary>
    //释放所有资源
    private void ReleaseAllAsset()
    {
        string[] bds=  bundles.Keys.ToArray();
        foreach (var item in bds)
        {
            ReleaseAsset(item);
        }
        bundles.Clear();
        mainfest = null;
        AssetBundle.UnloadAllAssetBundles(true);
    }
}
