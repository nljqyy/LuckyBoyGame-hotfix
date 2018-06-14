using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public sealed class LoadAssetMrg :Singleton<LoadAssetMrg>
{
    private LoadAssetMrg()
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
    private void LoadDependencies(string _assetName)
    {
        string[] deps = GetDirectDependencies(_assetName);
        foreach (var assetname in deps)
        {
            string aName= assetname.Replace(suffixName, "");
            LoadAsset(aName);
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
        mainfest = null;
        foreach (var item in bundles)
        {
            ReleaseAsset(item.Key);
        }
        bundles.Clear();
        AssetBundle.UnloadAllAssetBundles(true);
    }
}
