using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[Hotfix]
public sealed class UIMessagePage : UIDataBase
{
    public const string NAME = "UIMessagePage.prefab";
    public override UIShowPos ShowPos
    {
        get
        {
            return UIShowPos.TipTop;
        }
    }
    public override HidePage hidePage
    {
        get
        {
            return HidePage.Destory;
        }
    }

    private Text msg;
    protected override void Init()
    {
        base.Init();
        msg = CommTool.GetCompentCustom<Text>(gameObject, "msg");
    }
    public override void OnShow(object data)
    {
        base.OnShow(data);
        string content = data.ToString();
        msg.text = content;
        SDKManager.Instance.Speak(content);
    }
}
