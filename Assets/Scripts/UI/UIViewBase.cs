using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;


[Hotfix]
public abstract class UIViewBase : AnimateBase
{
    public string DlgName
    {
        get { return gameObject.name; }
    }
    //要传入的数据
    public object Data { get; private set; }
   
    //是否已加载
    public bool isLoaded { get; private set; }
    
    #region 加载预制物体
    private void Awake()
    {
        //异步加载
        LoadGameObjByAsync(obj =>
        {
            obj.transform.SetParent(gameObject.transform, false);
            InitAnimator(obj);
            OnInit();
            OnCreate();
            OnEnter();
            isLoaded = true;
        });
    }
    IEnumerator LoadGameObj(Action<GameObject> act)
    {
        if (!string.IsNullOrEmpty(DlgName))
        {
            Debug.Log("----加载ui预制物体--" + DlgName);
            string uipath = "UIPrefab/" + DlgName;
            GameObject obj = Resources.Load<GameObject>(uipath);
            if (obj)
            {
                GameObject temp = Instantiate<GameObject>(obj);
                if (act != null && temp)
                {
                    act(temp);
                }
            }
            else
                Debug.LogError(DlgName + "----不存在");
        }
        else
        {
            Debug.LogError("DlgName是空");
        }
        yield break;
    }

    void LoadGameObjByAsync(Action<GameObject> act)
    {
        if (!string.IsNullOrEmpty(DlgName))
        {
            LoadAssetMrg.Instance.LoadAssetAsync(DlgName, bd =>
             {
                 if (bd != null)
                 {
                     GameObject obj = (GameObject)bd.mAsset;
                     if (obj)
                     {
                         GameObject temp = Instantiate<GameObject>(obj);
                         if (act != null && temp)
                             act(temp);
                     }
                 }
                 else
                     Debug.LogError(DlgName + "----不存在");
             });
        }
        else
            Debug.LogError("DlgName是空");
    }

    void LoadGameObjBySync(Action<GameObject> act)
    {
        if (!string.IsNullOrEmpty(DlgName))
        {
            Bundle bd = LoadAssetMrg.Instance.LoadAsset(DlgName);
            if (bd != null)
            {
                GameObject obj = (GameObject)bd.mAsset;
                if (obj)
                {
                    GameObject temp = Instantiate<GameObject>(obj);
                    if (act != null && temp)
                        act(temp);
                }
            }
            else
                Debug.LogError(DlgName + "----不存在");
        }
        else
            Debug.LogError("DlgName是空");

    }
    #endregion


    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="data"></param>
    public void SetData(object data)
    {
        Data = data;
    }
}
