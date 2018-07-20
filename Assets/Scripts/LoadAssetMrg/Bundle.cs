using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

[Hotfix]
public sealed class Bundle
{

    private const string suffixName = ".bytes";
    public string mAssetName { get; private set; }
    public UnityEngine.Object mAsset { get; set; }
    private AssetBundle mAssetBundle;

    public bool isLoaded { get; private set; }
    public bool startLoad { get; private set; }
    public int userCount { get; private set; }

    public Bundle(string _assetName)
    {
        mAssetName = _assetName;
        isLoaded = false;
        startLoad = true;
        userCount = 0;
    }
    public static string CombinSuffixName(string _mAssetName)
    {
        if (!_mAssetName.EndsWith(suffixName))
            return _mAssetName + suffixName;
        return _mAssetName;
    }

    public static string DeleteSuffixName(string _mAssetName)
    {
        if (_mAssetName.EndsWith(suffixName))
            return _mAssetName.Replace(suffixName, "");
        return _mAssetName;
    }
    public void Retain()
    {
        ++userCount;
    }
    public void Release()
    {
        if (!isLoaded) return;
        if (--userCount == 0)
        {
            Debug.Log("卸载资源---" + mAssetName);
            LoadAssetMrg.Instance.Remove(mAssetName);
            if (!(mAsset is GameObject))
                Resources.UnloadAsset(mAsset);
            mAsset = null;
            if (mAssetBundle != null)
            {
                mAssetBundle.Unload(true);
                mAssetBundle = null;
            }
        }
    }

    public void GoLoad()
    {
        string assetPath = PathHelp.GetDownLoadPath() + PathHelp.unZip + CombinSuffixName(mAssetName);
        Debug.Log("加载assetbundle---" + assetPath);
        mAssetBundle = AssetBundle.LoadFromFile(assetPath);
        if (mAssetBundle != null)
        {
            isLoaded = true;
            if (mAssetBundle.isStreamedSceneAssetBundle)
                mAsset = mAssetBundle.mainAsset;
            else
            {
                mAsset = mAssetBundle.LoadAsset(mAssetName);
                mAssetBundle.Unload(false);
                mAssetBundle = null;
            }
        }
        else
            Debug.Log("未发现assetbundle---" + assetPath);

    }

    public IEnumerator GoLoadAsync()
    {
        string assetPath = PathHelp.GetDownLoadPath() + PathHelp.unZip + CombinSuffixName(mAssetName);
        Debug.Log("异步加载assetbundle---" + assetPath);
        AssetBundleCreateRequest abrequest = AssetBundle.LoadFromFileAsync(assetPath);
        yield return abrequest;
        if (abrequest.isDone)
        {
            mAssetBundle = abrequest.assetBundle;
            abrequest = null;
            if (mAssetBundle != null)
            {
                isLoaded = true;
                if (mAssetBundle.isStreamedSceneAssetBundle)
                    mAsset = mAssetBundle.mainAsset;
                else
                {
                    AssetBundleRequest _abr = mAssetBundle.LoadAssetAsync(mAssetName);
                    yield return _abr;
                    mAsset = _abr.asset;
                    _abr = null;
                    mAssetBundle.Unload(false);
                    mAssetBundle = null;
                }
            }
            else
                Debug.Log("未发现assetbundle---" + assetPath);
        }
    }
}
