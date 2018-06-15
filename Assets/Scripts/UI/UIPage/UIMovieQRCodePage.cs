using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Video;
using DG.Tweening;
using XLua;

[Hotfix]
public sealed class UIMovieQRCodePage : UIDataBase
{
    public const string NAME = "UIMovieQRCodePage.prefab";
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
    private GameObject qrCode;
    private RawImage raw;
    private VideoPlayer vplayer;
    private GameObject loading;
    private Transform yun;
    private Transform fly;
    private Animator animator;
    private Image xiaoP;
    private List<ExcelTableEntity> elist;
    protected override void Init()
    {
        base.Init();
        qrCode = CommTool.FindObjForName(gameObject, "QR-code");
        raw = CommTool.GetCompentCustom<RawImage>(qrCode, "RawImage");
        loading = CommTool.FindObjForName(qrCode, "loading");
        fly = CommTool.FindObjForName(qrCode, "fly").transform;
        yun = CommTool.FindObjForName(qrCode, "yun").transform;
        vplayer = CommTool.GetCompentCustom<VideoPlayer>(gameObject, "movie");
        xiaoP = CommTool.GetCompentCustom<Image>(qrCode, "xiaopang");
        animator = CommTool.GetCompentCustom<Animator>(qrCode, "xiaopang");
        animator.enabled = false;
        elist = SDKManager.Instance.GetVoiceForType(VoiceType.QRCode);
        Reg();
    }
    private void Reg()
    {
        EventHandler.RegisterEvnet(EventHandlerType.QRCodeSuccess, QRCodeSuccess);
    }
    private void UnReg()
    {
        EventHandler.UnRegisterEvent(EventHandlerType.QRCodeSuccess, QRCodeSuccess);
    }

    public override void OnShow(object data)
    {
        base.OnShow(data);
        qrCode.SetActive(false);
        vplayer.gameObject.SetActive(true);
        PlayMovie();
    }
    public override void OnHide()
    {
        base.OnHide();
        elist.Clear();
        UnReg();
    }

    void PlayMovie()
    {
        vplayer.loopPointReached += MovieOver;
        vplayer.Play();
    }
    void MovieOver(VideoPlayer p)
    {
        Debug.Log("视频播放完毕");
        vplayer.gameObject.SetActive(false);
        qrCode.SetActive(true);
        loading.SetActive(true);
        raw.gameObject.SetActive(false);
        if (SDKManager.Instance.IsCanPlay())
        {
            #region 获取二维码
            bool flagQuit = false;
            StartCoroutine(SDKManager.Instance.TimeFun(60, 5, (ref float t) =>
               {
                   //if (Application.internetReachability != NetworkReachability.NotReachable)
                   //    return true;
                   if (!SDKManager.Instance.getCode)
                   {
                       SDKManager.Instance.GetQR_Code(raw);
                       if (t == 0 && !flagQuit)
                       {
                           List<ExcelTableEntity> etable = SDKManager.Instance.GetVoiceForType(VoiceType.Special);
                           SDKManager.Instance.Speak(etable[0].TimeContent);//播放没有网络音效
                           t = Convert.ToInt32(etable[0].WinTime);
                           if (t < 10) t = 10;
                           flagQuit = true;
                       }
                       else if (t == 0)// 没有网络时间到退出
                       {
                           SDKManager.Instance.AppQuit();//游戏推出
                           return true;
                       }
                       return false;
                   }
                   else
                       return true;
               }));
            #endregion
        }
        else
        {
            string msg = "没有吉娃娃不能开始游戏";
            SDKManager.Instance.Speak(msg);
            StartCoroutine(SDKManager.Instance.TimeFun(3, 3, null, SDKManager.Instance.AppQuit));
        }
    }

    //二维码语音
    IEnumerator PlayVoiceIe()
    {
        int tTime = 0;
        int index = 0;
        int cTime = 0;
        int cRunTime = 0;
        int count = elist.Count;
        while (tTime <= 60)
        {
            if (cRunTime >= cTime)//此时语音已播完
            {
                animator.enabled = false;
                xiaoP.sprite = UIAtlasManager.LoadSprite(UIAtlasName.UIQRCode, "1");
            }
            if (index < count && elist[index].Time == tTime.ToString())
            {
                animator.enabled = true;
                SDKManager.Instance.Speak(elist[index].TimeContent);
                cTime = Convert.ToInt32(elist[index].WinTime);
                cRunTime = 0;
                index++;
            }
            yield return new WaitForSeconds(1);
            cRunTime++;
            tTime++;
        }
        SDKManager.Instance.CustomQuit();
    }

    //二维码返回成功
    void QRCodeSuccess(object data)
    {
        loading.SetActive(false);
        raw.gameObject.SetActive(true);
        StartCoroutine(PlayVoiceIe());//二维码界面计时
    }
}
