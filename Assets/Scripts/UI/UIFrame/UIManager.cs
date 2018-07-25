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
            if (view != null && !dicView.ContainsKey(viewName))
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
        if (dicContext.ContainsKey(view._showPos))
            uiContext = dicContext[view._showPos];
        else
            uiContext = new UIContext(this, view._showPos);
        if (view._showPos == UIShowPos.TipTop)//是顶层  下面都暂停
        {
            if (dicContext.ContainsKey(UIShowPos.Normal))
                dicContext[UIShowPos.Normal].Pause();
        }
        uiContext.Push(view);
        dicContext[view._showPos] = uiContext;
    }

    private void PopStack(UIViewBase view)
    {
        UIContext uiContext;
        dicContext.TryGetValue(view._showPos, out uiContext);
        if (uiContext!=null)
        {
            uiContext.Pop();
            if (uiContext._count == 0)
            {
                if (uiContext._showPos == UIShowPos.TipTop)//是顶层  下面都继续
                {
                    if (dicContext.ContainsKey(UIShowPos.Normal))
                        dicContext[UIShowPos.Normal].Resume();//继续
                }
            }
        }
    }

    public void ShowUI(string viewName, bool isShow, object data = null)
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
        }
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
        dicContext.Clear();
        base.Dispose();
    }

}
