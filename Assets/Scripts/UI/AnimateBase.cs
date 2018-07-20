using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AnimateBase : MonoBehaviour {

    public CanvasGroup _canvasGroup { get; private set; }

    public Animator _animator { get; private set; }
    //动态指定动画事件执行时间
    public float _enterTime { get; protected set; }
    //动态指定动画事件执行时间
    public float _exitTime { get; protected set; }

    public abstract UIShowPos ShowPos { get; }

    public abstract HidePage hidePage { get; }

    protected void InitAnimator(GameObject go)
    {
        _canvasGroup = go.GetComponent<CanvasGroup>();
        _animator = go.GetComponent<Animator>();
    }

    protected virtual void OnInit()
    {
        Debug.Log("----界面初始化------" + gameObject.name);
        RegAnimateEvent();
    }
    public virtual void OnCreate()
    {

    }
    public virtual void OnEnter()
    {
        if (_animator)
            _animator.SetTrigger("OnEnter");
    }
    public virtual void OnResume()
    {
        if (_animator)
            _animator.SetTrigger("OnResume");
    }
    public virtual void OnPause()
    {
        if (_animator)
            _animator.SetTrigger("OnPause");
    }
   
    public virtual void OnExit()
    {
        if (_animator)
            _animator.SetTrigger("OnExit");
    }


    /// <summary>
    /// 进入时的动画事件
    /// </summary>
    public virtual void EnterEvent()
    {

    }
    /// <summary>
    /// 退出时的动画事件
    /// </summary>
    public virtual void ExitEvent()
    {
        if (this.hidePage == HidePage.Hide)
        {
            gameObject.transform.SetParent(UIManager.Instance.hideUI, false);
            gameObject.SetActive(false);
        }
        else
        {
            UIManager.Instance.RemoveView(gameObject.name);
            LoadAssetMrg.Instance.ReleaseAsset(gameObject.name);
            GameObject.Destroy(gameObject);
        }
    }
    /// <summary>
    /// 注册动画事件
    /// </summary>
    private void RegAnimateEvent()
    {
        AnimationClip _enterClip = GetAniClip("OnEnter");
        AnimationClip _exitClip = GetAniClip("OnExit");
        AnimationEvent ae = null;
        if (_enterClip && _enterTime > 0)
        {
            ae = new AnimationEvent();
            ae.time = _enterTime;
            ae.functionName = "EnterEvent";
            _enterClip.AddEvent(ae);
        }
        if (_exitClip && _exitTime > 0)
        {
            ae = new AnimationEvent();
            ae.time = _exitTime;
            ae.functionName = "ExitEvent";
            _exitClip.AddEvent(ae);
        }
    }
    /// <summary>
    /// 获得动画片段
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    private AnimationClip GetAniClip(string clipName)
    {
        if (_animator == null) return null;
        for (int i = 0; i < _animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (_animator.runtimeAnimatorController.animationClips[i].name == clipName)
            {
                return _animator.runtimeAnimatorController.animationClips[i];
            }
        }
        return null;
    }
    
}
