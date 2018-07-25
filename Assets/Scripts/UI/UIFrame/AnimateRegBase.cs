using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class AnimateRegBase : AnimateBase
{
    /// <summary>
    /// 动画事件帮助类
    /// </summary>
    public sealed class AnimateEventHelp : MonoBehaviour
    {
        [HideInInspector]
        public AnimateRegBase aniReg;
        private void EnterEvent_Help(float time)
        {
            if (aniReg && aniReg.aniState == AnimateState.Enter)
            {
                var kvp = aniReg._kvpPlayEnter.SingleOrDefault(m => m.Key == time);
                if (kvp.Value != null)
                    kvp.Value();
            }
        }
        private void PauseEvent_Help(float time)
        {
            if (aniReg && aniReg.aniState == AnimateState.Pause)
            {
                var kvp = aniReg._kvpPlayPause.SingleOrDefault(m => m.Key == time);
                if (kvp.Value != null)
                    kvp.Value();
            }
        }
        private void ResumeEvent_Help(float time)
        {
            if (aniReg && aniReg.aniState == AnimateState.Resume)
            {
                var kvp = aniReg._kvpPlayResume.SingleOrDefault(m => m.Key == time);
                if (kvp.Value != null)
                    kvp.Value();
            }
        }
        private void ExitEvent_Help(float time)
        {
            if (aniReg && aniReg.aniState == AnimateState.Exit)
            {
                var kvp = aniReg._kvpPlayExit.SingleOrDefault(m => m.Key == time);
                if (kvp.Value != null)
                    kvp.Value();
            }
        }
    }

    public KeyValuePair<float, Action>[] _kvpPlayEnter { get; private set; }
    public KeyValuePair<float, Action>[] _kvpPlayExit { get; private set; }
    public KeyValuePair<float, Action>[] _kvpPlayPause { get; private set; }
    public KeyValuePair<float, Action>[] _kvpPlayResume { get; private set; }

    private AnimateEventHelp _animateHelp;
    /// <summary>
    /// 注册进入动画事件
    /// </summary>
    /// <param name="kvpEnter"></param>
    protected virtual void RegEnterAnimateEvent(params KeyValuePair<float, Action>[] kvpEnter)
    {
        _kvpPlayEnter = kvpEnter;
        RegAniEvent("OnEnter", "EnterEvent_Help", _kvpPlayEnter);
    }
    /// <summary>
    /// 注册暂停动画事件
    /// </summary>
    /// <param name="kvpPause"></param>
    protected virtual void RegPauseAnimateEvent(params KeyValuePair<float, Action>[] kvpPause)
    {
        _kvpPlayPause = kvpPause;
        RegAniEvent("OnPause", "PauseEvent_Help", _kvpPlayPause);
    }
    /// <summary>
    /// 注册继续动画事件
    /// </summary>
    /// <param name="kvpResume"></param>
    protected virtual void RegResumeAnimateEvent(params KeyValuePair<float, Action>[] kvpResume)
    {
        _kvpPlayResume = kvpResume;
        RegAniEvent("OnResume", "ResumeEvent_Help", _kvpPlayResume);
    }
    /// <summary>
    /// 注册退出动画事件
    /// </summary>
    /// <param name="kvpExit"></param>
    protected virtual void RegExitAnimateEvent(params KeyValuePair<float, Action>[] kvpExit)
    {
        _kvpPlayExit = AddExitSuccessEvent(kvpExit);
        RegAniEvent("OnExit", "ExitEvent_Help", _kvpPlayExit);
    }

    /// <summary>
    /// 当有退出动画时添加退出完成操作
    /// </summary>
    /// <param name="_kvp"></param>
    /// <returns></returns>
    private KeyValuePair<float, Action>[] AddExitSuccessEvent(KeyValuePair<float, Action>[] _kvp)
    {
        var kvplist = _kvp.ToList();
        if (kvplist.Count > 0)
        {
            int index = kvplist.FindIndex(kv => kv.Key == 1.0f);
            if (index >=0)
            {
                var kvp = kvplist[index];
                Action act = kvp.Value;
                act += PlayExitSuccess;
                kvplist[index] = new KeyValuePair<float, Action>(1, act);
                return kvplist.ToArray();
            }
        }
        kvplist.Add(new KeyValuePair<float, Action>(1, PlayExitSuccess));
        return kvplist.ToArray();
    }
    /// <summary>
    /// 批量注册动画事件
    /// </summary>
    /// <param name="animationClip">动画片段</param>
    /// <param name="functionName">函数名</param>
    /// <param name="kvp">时间 委托键值对</param>
    private void RegAniEvent(string animationClip, string functionName, KeyValuePair<float, Action>[] kvp)
    {
        if (kvp != null && kvp.Length > 0)
        {
            AnimationClip _enterClip = GetAniClip(animationClip);
            if (_enterClip)
            {
                if (!_animateHelp)
                {
                    _animateHelp = _clone.AddComponent<AnimateEventHelp>();
                    _animateHelp.aniReg = this;
                }
                AnimationEvent ae = null;
                foreach (var item in kvp)
                {
                    ae = new AnimationEvent();
                    ae.time = item.Key;
                    ae.floatParameter = item.Key;
                    ae.functionName = functionName;
                    _enterClip.AddEvent(ae);
                }
            }
        }
    }
}
