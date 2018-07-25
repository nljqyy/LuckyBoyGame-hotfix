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
    public override UIShowPos _showPos { get { return UIShowPos.TipTop; } }
    public override HidePage _hidePage { get { return HidePage.Hide; } }
    private GameObject success;
    private GameObject fail;
    private GameObject failDrop;
    private GameObject gameEnd;
    private GameObject hasboy;
    
    protected override void OnInit()
    {
        base.OnInit();
        success = CommTool.FindObjForName(gameObject, "Success");
        fail = CommTool.FindObjForName(gameObject, "Fail");
        failDrop = CommTool.FindObjForName(gameObject, "FailDrop");
        gameEnd = CommTool.FindObjForName(gameObject, "GameEnd");
        hasboy = CommTool.FindObjForName(gameObject, "HasBoy");
      
    }
    public override void OnEnter()
    {
        base.OnEnter();
        SuccessFuc(_Data);
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

    protected override void RegExitAnimateEvent(params KeyValuePair<float, Action>[] kvpExit)
    {
        base.RegExitAnimateEvent(new KeyValuePair<float, Action>(1, () => Debug.Log("哈哈哈哈哈哈哈结束啦")),
            new KeyValuePair<float, Action>(0.5f, () => Debug.Log("退出到一半啦"))
           );
    }


}
