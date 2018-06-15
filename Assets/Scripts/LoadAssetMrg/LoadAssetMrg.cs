using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using XLua;
using System;
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
        base.Dispose();
        Disposes(true);
        System.GC.SuppressFinalize(this);
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
    public void GetMainfest()
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
    //根据依赖加载
    private void LoadDependencies(string _assetName,bool isAysnc=false)
    {
        string[] deps = GetDirectDependencies(_assetName);
        foreach (var assetname in deps)
        {
            string aName= assetname.Replace(suffixName, "");
            if (!isAysnc)
                LoadAsset(aName);
            else
                LoadAssetAsync(aName,null);
        }
    }
    //加载assetbundle
    public Bundle LoadAsset(string _assetName)
    {
        if (string.IsNullOrEmpty(_assetName)) return null;
        Bundle bd = null;
        if (!bundles.TryGetValue(_assetName, out bd))
        {
            bd = new Bundle(_assetName);
            bd.GoLoad();
            bundles.Add(_assetName, bd);
            LoadDependencies(_assetName);
        }
        bd.Retain();
        return bd;
    }

    public void LoadAssetAsync(string _assetName,Action<Bundle> action)
    {
        StartCoroutine(LoadAssetIe(_assetName,action));
    }
    private IEnumerator LoadAssetIe(string _assetName, Action<Bundle> action)
    {
        if (string.IsNullOrEmpty(_assetName)) yield break;
        Bundle bd = null;
        if (!bundles.TryGetValue(_assetName, out bd))
        {
            bd = new Bundle(_assetName);
            yield return bd.GoLoadAsync();
            bundles.Add(_assetName, bd);
            LoadDependencies(_assetName,true);
        }
        bd.Retain();
        if (action != null)
            action(bd);
    }
    public void Remove(string _assetName)
    {
        if (bundles.ContainsKey(_assetName))
            bundles.Remove(_assetName);
    }
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
                ReleaseAsset(assetname);
            }
        }
    }
   
    //释放所有资源
    private void ReleaseAllAsset()
    {
        foreach (var item in bundles)
        {
            ReleaseAsset(item.Key);
        }
        bundles.Clear();
        mainfest = null;
        mMainfestBundle.Unload(true);
        AssetBundle.UnloadAllAssetBundles(true);
    }
}
