using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using XLua;

[Hotfix]
public sealed class UITimePage : UIDataBase
{
    public const string NAME = "UITimePage.prefab";
    public override UIShowPos ShowPos
    {
        get
        {
            return UIShowPos.Normal;
        }
    }
    public override HidePage hidePage
    {
        get
        {
            return HidePage.Destory;
        }
    }
    private Image time_1;
    private Image time_2;
    private Text text_time;
    private Image quxian;
    private int num1 = 3;
    private int num2 = 0;
    private int time_ci = 4;
    private int succNum = 0;
    private bool isUpFinish = false;
    private Action aciton = null;
    private List<ExcelTableEntity> elist;
    private int index = 0;
  
    public override void Init()
    {
        base.Init();
        time_1 = CommTool.GetCompentCustom<Image>(gameObject, "time_1");
        time_2 = CommTool.GetCompentCustom<Image>(gameObject, "time_2");
        quxian = CommTool.GetCompentCustom<Image>(gameObject, "quxian");
        text_time = CommTool.GetCompentCustom<Text>(gameObject, "time");
        Reg();
        //elist = SDKManager.Instance.GetVoiceForType(VoiceType.Five);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        //SetQuXImg();
    }


    public override void OnShow(object data)
    {
        base.OnShow(data);
        aciton = NormalUpdate;
        OnInit();
        StartCoroutine(TimeUpdate());
    }
    private void Reg()
    {
        EventHandler.RegisterEvnet(EventHandlerType.Success, Success);
        EventHandler.RegisterEvnet(EventHandlerType.HeadPress, HeadPress);
    }
    //根据本地记录重置数据
    private void OnInit()
    {
        time_ci = SDKManager.Instance.gameStatus.currentGameNumber;
        succNum = SDKManager.Instance.gameStatus.isCatch ? 1 : 0;
        elist = GetVoiceForType(time_ci);
        text_time.text = time_ci.ToString();
    }
    //获得语音数据
    private List<ExcelTableEntity> GetVoiceForType(int times)
    {
        return SDKManager.Instance.GetVoiceForType((VoiceType)(times+1));
    }
    IEnumerator TimeUpdate()
    {
        while (num1 > 0 || (num1 >= 0 && num2 >= 0))
        {
            yield return new WaitForSeconds(1);
            if (isUpFinish)
                continue;
            if (aciton != null)
                aciton();
            if (num1 < 0 || num2 < 0)
            {
                num1 = 0;
                num2 = 0;
                aciton = null;
                //时间到自动抓
                SDKManager.Instance.StartCatchBoy();
            }
        }
    }

    private void NormalUpdate()
    {
        int number = 30 - Convert.ToInt32(num1 + "" + num2);
        PlayingAction(number);
        if (num2 == 0)
        {
            num2 = 9;
            num1--;
        }
        else
            num2--;
        if (num1 >= 0 && num2 >= 0)
        {
            time_1.overrideSprite = UIAtlasManager.LoadSprite(UIAtlasName.UIMain, num1.ToString());
            time_2.overrideSprite = UIAtlasManager.LoadSprite(UIAtlasName.UIMain, num2.ToString());
        }
    }

    private void RestStartUpdate()
    {
        num1 = 3;
        num2 = 0;
        time_1.overrideSprite = UIAtlasManager.LoadSprite(UIAtlasName.UIMain, num1.ToString());
        time_2.overrideSprite = UIAtlasManager.LoadSprite(UIAtlasName.UIMain, num2.ToString());
        aciton = null;
        aciton = NormalUpdate;
    }
    private void RestStart()
    {
        time_ci-=1;
        SDKManager.Instance.SetCaught();
        if (time_ci >=0)
        {
            //SetQuXImg();
            text_time.text = time_ci.ToString();
            isUpFinish = false;
            index = 0;
            EventHandler.ExcuteEvent(EventHandlerType.RestStart, null);
            elist = GetVoiceForType(time_ci);
            aciton = null;
            aciton = RestStartUpdate;
        }
        else//游戏结束
        {
            isUpFinish = true;
            SDKManager.Instance.SetEnd();
            UIManager.Instance.ShowUI(UIPromptPage.NAME, true, CatchTy.GameEnd);
            EventHandler.ExcuteEvent(EventHandlerType.GameEnd, null);
            elist = SDKManager.Instance.GetVoiceForType(VoiceType.End);
            StopCoroutine(TimeUpdate());
            SDKManager.Instance.Speak(elist[0].TimeContent);
            int time = Convert.ToInt32(elist[0].WinTime);
            SDKManager.Instance.WonDoll(true);
            StartCoroutine(SDKManager.Instance.TimeFun(time, 0.5f, (ref float t) =>
             {
                 if (SDKManager.Instance.isEnd && !SDKManager.Instance.isCaught)
                 {
                     SDKManager.Instance.AppQuit();
                     return true;
                 }
                 return false;
             }, SDKManager.Instance.AppQuit));//游戏推出
        }
    }
    /// <summary>
    /// 是否抓中
    /// </summary>
    /// <param name="data"></param>
    private void Success(object data)
    {
        isUpFinish = true;
        StopCoroutine(WinPlay((CatchTy)data));
        StartCoroutine(WinPlay((CatchTy)data));
    }

    IEnumerator WinPlay(CatchTy cat)
    {
        SDKManager.Instance.gameStatus.SetGameNumber(time_ci-1);
        if (cat == CatchTy.Catch)
        {
            MyFuncPerSecond func = null;
            #region 有异物
            if (!SDKManager.Instance.isTabke())//出口有异物
            {
                UIManager.Instance.ShowUI(UIPromptPage.NAME, true, CatchTy.HasBoy);
                List<ExcelTableEntity> etable = SDKManager.Instance.GetVoiceForType(VoiceType.Special);
                func = (ref float t) =>
                {
                    if (t == 20 && !SDKManager.Instance.isTakeAway)
                    {
                        SDKManager.Instance.Speak(etable[1].TimeContent);//播放音效
                    }
                    if (t == 0) t = 21;
                    return SDKManager.Instance.isTakeAway;
                };
                yield return SDKManager.Instance.TimeFun(20, 1, func);
            }
            #endregion

            #region 胜利等待取走礼物
            succNum++;
            SDKManager.Instance.gameStatus.SetIsCatch(true);
            SDKManager.Instance.RecordTimes(succNum, true);
            UIManager.Instance.ShowUI(UIPromptPage.NAME, true, CatchTy.Catch);
            SDKManager.Instance.AutoSendPresent();//自动出礼物
            CommTool.SaveIntData(CatchTimes.Catch.ToString());
            AudioManager.Instance.PlayByName(AudioType.Fixed, AudioNams.shengli, false);//播放胜利音效
            SDKManager.Instance.WonDoll(true);//摆动翅膀闪光带
            EffectMrg.ShowEffect();//播放特效
            SDKManager.Instance.Speak(elist[0].WinningContent);
            int winTime = Convert.ToInt32(elist[0].WinTime);
            int winafter = Convert.ToInt32(elist[0].WinningAfterTime) + 2;//时间间隔两秒
            yield return SDKManager.Instance.TimeFun(winTime, winTime);
            func = null;
            func = (ref float t) =>
            {
                if (SDKManager.Instance.isTakeAway)//已取走
                {
                    Debug.Log("已取走");
                    EffectMrg.StopPlayEffect();
                    SDKManager.Instance.WonDoll(false);
                    EventHandler.ExcuteEvent(EventHandlerType.ClosePage, null);
                    return true;
                }
                if (t == 0)
                {
                    SDKManager.Instance.Speak(elist[0].WinningAfter);//播放音效
                    t = winafter;
                }
                return false;
            };
            yield return SDKManager.Instance.TimeFun(winafter, 1, func);
            #endregion
        }
        else
        {
            SDKManager.Instance.RecordTimes(succNum, false);
            UIManager.Instance.ShowUI(UIPromptPage.NAME, true, cat);//失败显示
            AudioManager.Instance.PlayByName(AudioType.Fixed, AudioNams.shibai, false);
            string[] contents;
            if (cat == CatchTy.NoCatch)
                contents = elist[0].FailContent.Split('|');
            else
                contents = elist[0].FialContentDrop.Split('|');
            int index = UnityEngine.Random.Range(0, contents.Length);
            SDKManager.Instance.Speak(contents[index]);//随机语音
            int delytime = Convert.ToInt32(elist[0].FailTime);
            if(time_ci>0)
               SDKManager.Instance.Light(false,5000);
            else
               SDKManager.Instance.Light(false, (delytime-1)*1000);
            yield return new WaitForSeconds(delytime);
            EventHandler.ExcuteEvent(EventHandlerType.ClosePage, null);
        }
        RestStart();
    }

    //播放语音
    private void PlayingAction(int time)
    {
        int count = elist.Count;
        if (time == 0 && time_ci == 4)//第五次加载
        {
            SDKManager.Instance.Light(false, 5000);
            SDKManager.Instance.Wave(5000);
        }
        if (index < count && time.ToString() == elist[index].Time)
        {
            SDKManager.Instance.Speak(elist[index].TimeContent);
            index++;
        }
    }

    //设置曲线值
    private void SetQuXImg()
    {
        Sprite sp = quxian.sprite;
        int random = 0;
        while (sp == quxian.sprite)
        {
            random = UnityEngine.Random.Range(1, 4);
            sp = UIAtlasManager.LoadSprite(UIAtlasName.UIMain, "Bo-0" + random);
        }
        quxian.sprite = sp;
        SDKManager.Instance.randomQuXian = random;//就一种速度
    }

    //拍头停止计时
    private void HeadPress(object o)
    {
        isUpFinish = true;
    }
}
