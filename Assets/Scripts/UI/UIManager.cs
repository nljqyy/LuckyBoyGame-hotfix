using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

using XLua;
[Hotfix]
public sealed class UIManager : Singleton<UIManager>
{
    private Transform normalUI;
    private Transform hideUI;
    private Transform tiptopUI;
    private GameObject canves;
    private UIManager() { }
    protected override void StartUp()
    {
        base.StartUp();
        canves = GameObject.Find("UICanvas");
        if (canves)
        {
            normalUI = canves.transform.Find("uiRoot/normalUI");
            hideUI = canves.transform.Find("uiRoot/hideUI");
            tiptopUI = canves.transform.Find("uiRoot/tiptopUI");
        }
    }
    private Dictionary<string, UIDataBase> dicDlg = new Dictionary<string, UIDataBase>();

    private UIDataBase GetDigLog(string dlgName, out bool isHas)
    {
        isHas = false;
        if (string.IsNullOrEmpty(dlgName))
            return null;
        UIDataBase dlg = null;
        GameObject uiRoot = null;
        if (dicDlg.TryGetValue(dlgName, out dlg))
        {
            uiRoot = dlg.gameObject;
            isHas = true;
        }
        else
        {
            dlg = RegisterDlgScripte(dlgName, out uiRoot);
        }
        SetUIRootParent(uiRoot, dlg.ShowPos);
        return dlg;
    }
    private void SetUIRootParent(GameObject uiRoot, UIShowPos type)
    {
        Transform Parent = null;
        if (type == UIShowPos.Normal)
            Parent = normalUI;
        else if (type == UIShowPos.TipTop)
            Parent = tiptopUI;
        else
            Parent = hideUI;
        uiRoot.transform.SetParent(Parent);
        uiRoot.transform.localPosition = Vector3.zero;
        uiRoot.transform.localScale = Vector3.one;
        uiRoot.transform.localRotation = Quaternion.identity;
    }

    //需要手动注册脚本
    private UIDataBase RegisterDlgScripte(string dlgName, out GameObject uiRoot)
    {
        UIDataBase dlg = null;
        uiRoot = null;
        if (!string.IsNullOrEmpty(dlgName))
        {
            uiRoot = SetRootPro(dlgName);
            switch (dlgName)
            {
                case UIMovePage.NAME:
                    dlg = uiRoot.AddComponent<UIMovePage>();
                    break;
                case UITimePage.NAME:
                    dlg = uiRoot.AddComponent<UITimePage>();
                    break;
                case UIPromptPage.NAME:
                    dlg = uiRoot.AddComponent<UIPromptPage>();
                    break;
                case UIMovieQRCodePage.NAME:
                    dlg = uiRoot.AddComponent<UIMovieQRCodePage>();
                    break;
                case UIMessagePage.NAME:
                    dlg = uiRoot.AddComponent<UIMessagePage>();
                    break;
            }
            if (dlg != null)
                dicDlg.Add(dlgName, dlg);
            else
                GameObject.Destroy(dlg.gameObject);
        }
        return dlg;
    }


    private GameObject SetRootPro(string dlgName)
    {
        GameObject uiRoot = new GameObject(dlgName);
        uiRoot.SetActive(true);
        uiRoot.layer = LayerMask.NameToLayer("UI");
        return uiRoot;
    }
    public void ShowUI(string dlgName, bool isShow, object data = null, Action<GameObject> act = null)
    {
        UIDataBase dlg;
        bool isHas;
        dlg = GetDigLog(dlgName, out isHas);
        if (dlg)
        {
            dlg.Data = data;
            if (isHas && isShow)
                dlg.OnShow(data);
            else if (!isShow)
            {
                if (dlg.hidePage == HidePage.Hide)
                {
                    dlg.OnHide();
                    SetUIRootParent(dlg.gameObject, UIShowPos.Hide);
                }
                else
                {
                    GameObject.Destroy(dlg.gameObject);
                    dicDlg.Remove(dlgName);
                    LoadAssetMrg.Instance.ReleaseAsset(dlgName);
                    return;
                }
            }
            dlg.gameObject.SetActive(isShow);
            if (act != null)
                act(dlg.gameObject);
        }
    }


    public void Clear()
    {
        canves.SetActive(false);
        foreach (var item in dicDlg)
        {
            if (item.Value != null)
                GameObject.Destroy(item.Value.gameObject);
        }
        dicDlg.Clear();
        base.Dispose();
    }

}
