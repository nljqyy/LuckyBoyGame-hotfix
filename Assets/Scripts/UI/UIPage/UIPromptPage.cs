using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;
using UnityEngine.UI;

[Hotfix]
public sealed class UIPromptPage : UIViewBase
{
    public const string NAME = "UIPromptPage.prefab";
    public override UIShowPos ShowPos { get { return UIShowPos.TipTop; } }
    public override HidePage hidePage { get { return HidePage.Hide; } }
    private GameObject success;
    private GameObject fail;
    private GameObject failDrop;
    private GameObject gameEnd;
    private GameObject hasboy;
    protected override void OnInit()
    {
        _exitTime = 1;
        success = CommTool.FindObjForName(gameObject, "Success");
        fail = CommTool.FindObjForName(gameObject, "Fail");
        failDrop = CommTool.FindObjForName(gameObject, "FailDrop");
        gameEnd = CommTool.FindObjForName(gameObject, "GameEnd");
        hasboy = CommTool.FindObjForName(gameObject, "HasBoy");
        base.OnInit();
    }
    public override void OnEnter()
    {
        SuccessFuc(Data);
        base.OnEnter();
       
    }
    public override void ExitEvent()
    {
        base.ExitEvent();
    }

    private void SuccessFuc(object o)
    {
        CatchTy cath = (CatchTy)o;
        success.SetActive(cath == CatchTy.Catch);
        fail.SetActive(cath == CatchTy.CatchErrorPos || cath == CatchTy.NoCatch);
        failDrop.SetActive(cath == CatchTy.Drop);
        gameEnd.SetActive(cath == CatchTy.GameEnd);
        hasboy.SetActive(cath == CatchTy.HasBoy);
    }

}
