using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ui面板位置
/// </summary>
public enum UIShowPos
{
    Normal,
    Hide,
    TipTop,
}


public enum HidePage
{
    Hide,
    Destory
}


/// <summary>
/// 注册事件类型
/// </summary>
public sealed class EventHandlerType
{
    public const string FishHookCheck = "FishHookCheck";
    public const string UpFinish = "UpFinish";
    public const string RestStart = "RestStart";
    public const string PlayingAction = "PlayingAction";
    public const string Success = "Success";
    public const string ClosePage = "ClosePage";
    public const string HeadPress = "HeadPress";
    public const string TakeAway = "TakeAway";
    public const string QRCodeSuccess = "QRCodeSuccess";
    public const string GameEnd = "GameEnd";
}
/// <summary>
/// 游戏物体类型
/// </summary>
public enum EntityType
{
    Wheel,
    XiaoPang,
}
/// <summary>
/// 动画片段名称
/// </summary>
public enum AnimationName
{
    down,
    up,
    catchs,
    release,
}
/// <summary>
/// 语音类型
/// </summary>
public enum VoiceType
{
    One = 1,
    Two,
    Three,
    Four,
    Five,
    Start,
    QRCode,
    End,
    Special,
    Police,
}
/// <summary>
/// 协程类型
/// </summary>
public enum IeType
{
    Voice,//语音
    Time,//倒计时
}

public enum EffectType
{
    eff_yanhua,
    Bom_eff,
}

public enum AudioNams
{
    shibai,
    shengli,
    downing2,
    shoot,
    help,
}

public enum CallParameter
{
    PaySuccess = 0,
    HeadDown,
    HasBoy,
    NoHas,
    TakeAway,
    NoTakeAway,
    Error,
    NoBind,
}

public enum CatchTy
{
    CatchErrorPos,
    NoCatch,
    Drop,
    Catch,
    HasBoy,
    GameEnd,
}

public enum CatchTimes
{
    PlayerCatch,
    Catch
}
//子弹 特效
public enum BullteType
{
    Bullte,
    BomEff,
}
//音频类型
public enum AudioType
{
   Fixed,
   Continuous,
   New,
}
//游戏进行状态
public enum GameRunStatus
{
    QRCode,
    InGame,
    GameEnd,
}

 
