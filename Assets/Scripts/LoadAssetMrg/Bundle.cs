using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bundle  {

    private const string suffixName = ".bytes";
    public string mAssetName { get; private set; }
    public Object mAsset { get; set; }
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
    public  string CombinSuffixName()
    {
        if (!mAssetName.EndsWith(suffixName))
            return mAssetName + suffixName;
        return mAssetName;
    }

    public  string DeleteSuffixName()
    {
        if (mAssetName.EndsWith(suffixName))
            return mAssetName.Replace(suffixName,"");
        return mAssetName;
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
            Resources.UnloadAsset(mAsset as AssetBundle);
            if (mAssetBundle != null)
                mAssetBundle.Unload(true);
            mAssetBundle = null;
            mAsset = null;
        }
    }

    public void GoLoad()
    {
        string assetPath = PathHelp.GetDownLoadPath() + PathHelp.unZip + CombinSuffixName();
        Debug.Log("加载assetbundle---"+ assetPath);
        mAssetBundle=AssetBundle.LoadFromFile(assetPath);
        if (mAssetBundle!=null)
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
}
