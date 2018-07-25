using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[Hotfix]
public sealed class UIMessagePage : UIViewBase
{
    public const string NAME = "UIMessagePage.prefab";
    public override UIShowPos _showPos
    {
        get
        {
            return UIShowPos.TipTop;
        }
    }
    public override HidePage _hidePage
    {
        get
        {
            return HidePage.Destory;
        }
    }

    private Text msg;
    protected override void OnInit()
    {
        base.OnInit();
        msg = CommTool.GetCompentCustom<Text>(gameObject, "msg");
    }
    public override void OnEnter()
    {
        string content = _Data.ToString();
        msg.text = content;
        SDKManager.Instance.Speak(content);
        base.OnEnter();
    }
}
