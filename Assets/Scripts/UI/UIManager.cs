using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

using XLua;
[Hotfix]
public sealed class UIManager : Singleton<UIManager>
{
    private const string normalPath = "uiRoot/normalUI";
    private const string hidePath = "uiRoot/hideUI";
    private const string tiptopPath = "uiRoot/tiptopUI";
    public Transform normalUI { get; private set; }
    public Transform hideUI { get; private set; }
    public Transform tiptopUI { get; private set; }
    private GameObject canves;
    private UIManager()
    {
        canves = GameObject.Find("UICanvas");
        if (canves)
        {
            normalUI = canves.transform.Find(normalPath);
            hideUI = canves.transform.Find(hidePath);
            tiptopUI = canves.transform.Find(tiptopPath);
        }
    }
    private Dictionary<string, UIViewBase> dicView = new Dictionary<string, UIViewBase>();
    private Dictionary<UIShowPos, UIContext> dicContext = new Dictionary<UIShowPos, UIContext>();

    private UIViewBase GetView(string dlgName)
    {
        if (string.IsNullOrEmpty(dlgName))
            return null;
        UIViewBase view = null;
        dicView.TryGetValue(dlgName, out view);
        if (view == null)
            view = RegisterDlgScripte(dlgName);
        return view;
    }

    //需要手动注册脚本
    private UIViewBase RegisterDlgScripte(string viewName)
    {
        UIViewBase view = null;
        if (!string.IsNullOrEmpty(viewName))
        {
            GameObject uiRoot = CreateRootObj(viewName);
            switch (viewName)
            {
                case UIMovePage.NAME:
                    view = uiRoot.AddComponent<UIMovePage>();
                    break;
                case UITimePage.NAME:
                    view = uiRoot.AddComponent<UITimePage>();
                    break;
                case UIPromptPage.NAME:
                    view = uiRoot.AddComponent<UIPromptPage>();
                    break;
                case UIMovieQRCodePage.NAME:
                    view = uiRoot.AddComponent<UIMovieQRCodePage>();
                    break;
                case UIMessagePage.NAME:
                    view = uiRoot.AddComponent<UIMessagePage>();
                    break;
            }
            if (view != null)
                dicView.Add(viewName, view);
            else
                GameObject.Destroy(uiRoot);
        }
        return view;
    }
    private GameObject CreateRootObj(string dlgName)
    {
        GameObject uiRoot = new GameObject(dlgName);
        uiRoot.SetActive(true);
        uiRoot.layer = LayerMask.NameToLayer("UI");
        return uiRoot;
    }

    public void RemoveView(string viewName)
    {
        if (dicView.ContainsKey(viewName))
            dicView.Remove(viewName);
    }


    private void PushStack(UIViewBase view)
    {
        view.gameObject.SetActive(true);
        UIContext uiContext = null;
        if (dicContext.ContainsKey(view.ShowPos))
            uiContext = dicContext[view.ShowPos];
        else
            uiContext = new UIContext(this);
        uiContext.Push(view);
        dicContext[view.ShowPos] = uiContext;
    }

    private void PopStack(UIViewBase view)
    {
        if (dicContext.ContainsKey(view.ShowPos))
        {
            UIContext uiContext = dicContext[view.ShowPos];
            uiContext.Pop();
        }
    }

    public void ShowUI(string viewName, bool isShow, object data = null, Action<GameObject> act = null)
    {
        UIViewBase view;
        view = GetView(viewName);
        if (view)
        {
            view.SetData(data);
            if (isShow)
                PushStack(view);
            else
                PopStack(view);
            if (act != null)
                act(view.gameObject);
        }


        //UIViewBase view;
        //view = GetDigLog(dlgName);
        //if (view)
        //{
        //    view.SetData(data);
        //    if (isShow)
        //    {
        //        view.gameObject.SetActive(true);
        //        if (view.isLoaded)
        //            view.OnShow();
        //    }
        //    else
        //    {
        //        view.OnHide();
        //        if (view.hidePage == HidePage.Hide)
        //            SetUIRootParent(view.gameObject, UIShowPos.Hide);
        //        else
        //        {
        //            GameObject.Destroy(view.gameObject);
        //            dicView.Remove(dlgName);
        //            LoadAssetMrg.Instance.ReleaseAsset(dlgName);
        //            return;
        //        }
        //        view.gameObject.SetActive(false);
        //    }
        //    if (act != null)
        //        act(view.gameObject);
        //}
    }


    public void Clear()
    {
        canves.SetActive(false);
        foreach (var item in dicView)
        {
            if (item.Value != null)
                GameObject.Destroy(item.Value.gameObject);
        }
        dicView.Clear();
        base.Dispose();
    }

}
