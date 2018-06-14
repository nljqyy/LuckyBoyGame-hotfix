using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

public delegate bool MyFuncPerSecond(ref float time);
[Hotfix]
public class SDKManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainObj;
    private RawImage raw;
    public static SDKManager Instance;
    //是否能抓
    public bool isCaught { get; private set; }
    //是否有娃娃
    public bool isHas { get; private set; }
    //是否已取走
    public bool isTakeAway { get; private set; }
    //已抓中几次
    public int caughtTime { get; private set; }
    //是否支付成功
    public bool isPaySucess { get; private set; }
    //游戏是否结束
    public bool isEnd { get; private set; }
    //是中的小胖
    public GameEntity gameXP { get; set; }
    //速度曲线值
    public int randomQuXian { get; set; }
    //抓中不被打掉概率
    public float probability { get; private set; }
    //一百次能最多抓中几次
    public float winningTimes { get; private set; }
    //概率基数
    public float carwBasicCount { get; private set; }
    //一百次能最多抓中几次
    public string startCarwTime { get; set; }
    //二维码是否成功
    public bool getCode { get; private set; }
    //抓取最大剩余次数
    private int basicTimes = 4;
    //当前玩家游戏状态
    public GameStatus gameStatus { get; set;}
    private AndroidJavaObject androidjava;
    private Dictionary<int, List<ExcelTableEntity>> dic;


    #region 测试数据参数
    public float checkProperty { get; set; }
    public bool isOpenPay { get; set; }
    #endregion

    private void Awake()
    {
        Init();
        XLuaManager.Instance.StartUp();
    }

    private void Init()
    {
        Debug.Log("游戏启动");
        checkProperty = 1f;
        Instance = this;
        isCaught = false;
        isHas = false;
        isTakeAway = true;
        isOpenPay = true;
        isPaySucess = false;
        isEnd = false;
        getCode = false;
        caughtTime = 0;
        randomQuXian = 1;//默认就一种最慢的速度
        probability = 30;//百分之15不打掉
        carwBasicCount = 100;
        winningTimes = 6;//抓中百分六
        gameStatus = null;
        dic = new Dictionary<int, List<ExcelTableEntity>>();
        //androidjava = Current();
        GetVoiceData();
        GetProbability();
        GetGameStatusData();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //测试数据
        UIManager.Instance.ShowUI(UIMovePage.NAME, true);
        UIManager.Instance.ShowUI(UITimePage.NAME, true);
        //EnterGame();
    }

    public AndroidJavaObject Current()
    {
        if (Application.platform == RuntimePlatform.Android && androidjava == null)
            return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        else
            return null;
    }


    /// <summary>
    /// 获得语音数据
    /// </summary>
    private void GetVoiceData()
    {
        //ExcelScriptObj v = Resources.Load<ExcelScriptObj>("voiceNames");
        Bundle bd=  LoadAssetMrg.Instance.LoadAsset("voiceNames.asset");
        ExcelScriptObj v = bd.mAsset as ExcelScriptObj;
        List<ExcelTableEntity> list;
        foreach (var item in v.voiceType)
        {
            list = v.voiceData.FindAll(m => m.Type == item.Type);
            list.Sort((m, n) => Convert.ToInt32(m.Time).CompareTo(Convert.ToInt32(n.Time)));
            dic.Add(Convert.ToInt32(item.Type), list);
        }
        LoadAssetMrg.Instance.ReleaseAsset("voiceNames.asset");
        bd = null;
        Debug.Log("语音数据获得成功");
    }

    private void GetGameStatusData()
    {
        gameStatus = CommTool.LoadClass<GameStatus>("GameStatus");
        if (gameStatus == null)
        {
            gameStatus = new GameStatus();
            gameStatus.SetAllPro(GameRunStatus.GameEnd, false, basicTimes);
        }
    }

    private void EnterGame()
    {
        mainObj.SetActive(false);
        Debug.Log("gameStatus---"+gameStatus.runStatus+"----gameNuber---"+gameStatus.currentGameNumber+"---isCatch=="+gameStatus.isCatch);
        if (gameStatus.runStatus == GameRunStatus.GameEnd || gameStatus.runStatus == GameRunStatus.QRCode)
            StartEnterGame();
        else
            PaySuccess();
    }
    //开始进入游戏
    private void StartEnterGame()
    {
        gameStatus.SetRunStatus(GameRunStatus.QRCode);
#if UNITY_ANDROID
        UIManager.Instance.ShowUI(UIMovieQRCodePage.NAME, true, null, o =>
        {
            if (dic.ContainsKey((int)VoiceType.Start))
            {
                string v = dic[(int)VoiceType.Start][0].TimeContent;
                Speak(v);//播放载入语音
            }
            Wave(5000);
            Light(false, 5000);
        });
#endif
    }

    //能否开始游戏
    public bool IsCanPlay()
    {
        if (androidjava != null)
            isHas = androidjava.Call<bool>("isCanPlay");
        return isHas;
    }
    //是否取走
    public bool isTabke()
    {
        if (androidjava != null)
            isTakeAway = androidjava.Call<bool>("isTakeAway");
        return isTakeAway;
    }

    public void GetPayStatus()
    {
        Debug.Log("查询是否支付");
        if (androidjava != null)
            androidjava.Call("GetPayStatus");
    }
    public void GetQR_Code(RawImage r)//二维码载入
    {
        Debug.Log("请求二维码");
        if (raw == null) raw = r;
        if (androidjava != null)
            androidjava.Call("GetDrawQrCode");
    }

    public void RecordTimes(int num, bool isSuccess)
    {
        Debug.Log("抓中传输计数-----" + num);
        caughtTime = num;
        if (androidjava != null)
            androidjava.Call("SendCatchRecord", isSuccess, startCarwTime);
    }
    public void Speak(string msg)
    {
        Debug.Log("播放语音-----" + msg);
        if (androidjava != null)
            androidjava.Call("uspeak", msg);
    }
    public void Wave(int num)
    {
        Debug.Log("摆动翅膀");
        if (androidjava != null)
            androidjava.Call("uwave", num);
    }
    public void WonDoll(bool state)
    {
        Debug.Log("抓中摆动翅膀闪灯带--"+state);
        if (androidjava != null)
            androidjava.Call("wonDoll", state);
    }
    public void GetProbability()
    {
        Debug.Log("获得抓中概率值");
        if (androidjava != null)
            androidjava.Call("GetProbabilityValue");
    }
    public void CustomQuit()
    {
        Debug.Log("自定义退出");
        if (androidjava != null)
            androidjava.Call("CustomQuit");
    }

    public void Light(bool n, int num)
    {
        Debug.Log("灯光闪烁啊");
        if (androidjava != null)
            androidjava.Call("ulight", n, num);
    }

    public void AutoSendPresent()
    {
        Debug.Log("自动送礼物");
        isTakeAway = false;
        if (androidjava != null)
            androidjava.Call("autoPresent");
    }

    public void AndroidCall(string result)
    {
        CallParameter cp = (CallParameter)Enum.Parse(typeof(CallParameter), result);
        switch (cp)
        {
            case CallParameter.PaySuccess:
                Debug.Log("支付成功");
                PaySuccess();
                break;
            case CallParameter.HeadDown:
                Debug.Log("头部按下");
                StartCatchBoy();
                break;
            case CallParameter.HasBoy:
                Debug.Log("有娃娃");
                isHas = true;
                break;
            case CallParameter.NoHas:
                Debug.Log("无娃娃");
                isHas = false;
                break;
            case CallParameter.TakeAway:
                Debug.Log("被取走");
                isTakeAway = true;
                break;
            case CallParameter.NoTakeAway:
                Debug.Log("没被取走");
                isTakeAway = false;
                break;
            case CallParameter.Error:
                Debug.Log("android--返回异常");
                break;
            case CallParameter.NoBind:
                Debug.Log("娃娃机未绑定");
                UIManager.Instance.ShowUI(UIMessagePage.NAME, true, "娃娃机未绑定，绑定后才可以玩游戏哦");
                break;
        }
    }
    //二维码获得成功
    public void QRCodeCall(string result)
    {
        if (getCode) return;
        getCode = true;
        Debug.Log("二维码获得成功");
        QRCode.ShowCode(raw, result);
        EventHandler.ExcuteEvent(EventHandlerType.QRCodeSuccess, null);
        if (isOpenPay)
        {
            StartCoroutine(TimeFun(2, 2, (ref float t) =>
               {
                   if (!isPaySucess)
                       GetPayStatus();//检测是否支付
                   if (t == 0) t = 2;
                   return isPaySucess;
               }));
        }
        else
            //测试用
            StartCoroutine(TimeFun(3, 3, null, () => PaySuccess()));
    }
    //获得概率值
    public void GetProbabilityCall(string result)
    {
        if (string.IsNullOrEmpty(result))
            return;
        string[] res = result.Split('|');
        Debug.Log("概率值获得成功------- probability： " + res[0] + "  winningTimes： " + res[1] + "  carwBasicCount:" + res[2]);
        probability = Convert.ToSingle(res[0]);
        winningTimes = Convert.ToSingle(res[1]);
        carwBasicCount = Convert.ToInt32(res[2]);
    }
    /// <summary>
    /// 时间到做一些事情
    /// </summary>
    /// <param name="deltime">延时时间</param>
    /// <param name="spacetime">几秒检测一次</param>
    /// <param name="func">指定的时间要做什么</param>
    /// <param name="aciton">时间到要做什么</param>
    /// <returns></returns>
    public IEnumerator TimeFun(float deltime, float spacetime, MyFuncPerSecond func = null, Action aciton = null)
    {
        while (deltime >= 0)
        {
            if (func != null)
            {
                if (func(ref deltime))
                    yield break;
            }
            if (deltime > 0)
                yield return new WaitForSeconds(spacetime);
            deltime -= spacetime;
        }
        if (aciton != null)//
            aciton();
    }

    public List<ExcelTableEntity> GetVoiceForType(VoiceType type)
    {
        if (dic.ContainsKey((int)type))
        {
            return dic[(int)type];
        }
        return null;
    }

    //开始抓娃娃
    public void StartCatchBoy()
    {
        if (isEnd)
            isCaught = false;
        if (isCaught)
        {
            isCaught = false;
            AudioManager.Instance.PlayByName(AudioType.Fixed, AudioNams.downing2, false);
            EventHandler.ExcuteEvent(EventHandlerType.HeadPress, null);
        }
    }
    public void SetEnd()
    {
        isEnd = true;
    }
    public void SetCaught()
    {
        isCaught = true;
    }
    //支付成功
    private void PaySuccess()
    {
        if (isPaySucess) return;//已经支付成功
        isPaySucess = true;
        mainObj.SetActive(true);
        gameStatus.SetRunStatus(GameRunStatus.InGame);
        UIManager.Instance.ShowUI(UIMovieQRCodePage.NAME, false);
        UIManager.Instance.ShowUI(UIMovePage.NAME, true);
        UIManager.Instance.ShowUI(UITimePage.NAME, true);
        isCaught = true;//可以摸头啦
    }

    /// <summary>
    /// 游戏推出
    /// </summary>
    public void AppQuit()
    {
        Debug.Log("退出游戏AppQuit");
        WonDoll(false);
        gameStatus.SetAllPro(GameRunStatus.GameEnd,false, basicTimes);
        UIManager.Instance.Clear();
        UIAtlasManager.Clear();
        AudioManager.Instance.Clear();
        EventHandler.Clear();
        EffectMrg.Clear();
        LoadAssetMrg.Instance.Dispose();
        GC.Collect();
        Application.Quit();
    }

}
