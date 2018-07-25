using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AnimateBase : MonoBehaviour, IMsgEvent
{
    public enum AnimateState
    {
        None,
        Enter,
        Pause,
        Resume,
        Exit
    }

    public CanvasGroup _canvasGroup { get; private set; }

    public Animator _animator { get; private set; }

    public AnimateState aniState { get; private set; }

    public GameObject _clone { get; private set; }

    public abstract UIShowPos _showPos { get; }

    public abstract HidePage _hidePage { get; }

    //要传入的数据
    public object _Data { get; private set; }

    //正在退出时 要执行打开面板
    private Action exitingAct;

    /// <summary>
    /// 初始化动画组件
    /// </summary>
    /// <param name="go"></param>
    protected void InitAnimator(GameObject go)
    {
        _clone = go;
        _canvasGroup = go.GetComponent<CanvasGroup>();
        _animator = go.GetComponent<Animator>();
        aniState = AnimateState.None;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void OnInit()
    {
        Debug.Log("----界面初始化------" + gameObject.name);

    }
    /// <summary>
    /// 内部创建物体
    /// </summary>
    protected virtual void OnCreate()
    {

    }
    /// <summary>
    /// 页面进入
    /// </summary>
    public virtual void OnEnter()
    {
        aniState = AnimateState.Enter;
        if (_animator)
            _animator.SetTrigger("OnEnter");
    }
    /// <summary>
    /// 页面暂停
    /// </summary>
    public virtual void OnPause()
    {
        aniState = AnimateState.Pause;
        if (_canvasGroup)
            _canvasGroup.blocksRaycasts = false;
        if (_animator)
            _animator.SetTrigger("OnPause");
    }
    /// <summary>
    /// 页面继续
    /// </summary>
    public virtual void OnResume()
    {
        aniState = AnimateState.Resume;
        if (_canvasGroup)
            _canvasGroup.blocksRaycasts = true;
        if (_animator)
            _animator.SetTrigger("OnResume");
    }
    /// <summary>
    /// 页面退出
    /// </summary>
    public virtual void OnExit()
    {
        aniState = AnimateState.Exit;
        if (_animator)
        {
            if (GetAniClip("OnExit"))
            {
                _animator.SetTrigger("OnExit");
                return;
            }
        }
        PlayExitSuccess();
    }

    /// <summary>
    /// 退出时的动画事件
    /// </summary>
    protected void PlayExitSuccess()
    {
        aniState = AnimateState.None;
        if (this._hidePage == HidePage.Hide)
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
        if (exitingAct != null)
        {
            exitingAct();
            exitingAct = null;
        }
    }
    /// <summary>
    /// 获得动画片段
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    protected AnimationClip GetAniClip(string clipName)
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

    /// <summary>
    /// 是否正在退出
    /// </summary>
    /// <returns></returns>
    public bool IsExiting()
    {
        if (aniState == AnimateState.Exit)
        {
            exitingAct = () => UIManager.Instance.ShowUI(gameObject.name, true, _Data);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="data"></param>
    public void SetData(object data)
    {
        _Data = data;
    }
}
