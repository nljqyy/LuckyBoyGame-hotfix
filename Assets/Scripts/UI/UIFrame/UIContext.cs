using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class UIContext
{
    private Stack<UIViewBase> _stack = new Stack<UIViewBase>();
    private UIManager _uiMrg;
    public UIShowPos _showPos { get; private set; }
    public int _count { get { return _stack.Count; } }
    public UIContext(UIManager uiMrg, UIShowPos showps)
    {
        _uiMrg = uiMrg;
        _showPos = showps;
    }
    public void Push(UIViewBase view)
    {
        Pause();
        SetUIRootParent(view.gameObject);
        _stack.Push(view);
        if (view._isLoaded)
        {
            if (!view.IsExiting())
                view.OnEnter();
        }
    }
    public void Pause()
    {
        if (_stack.Count > 0)
        {
            foreach (var item in _stack)
                item.OnPause();
        }
    }

    public void Resume()
    {
        if (_stack.Count > 0)
        {
            foreach (var item in _stack)
                item.OnResume();
        }
    }
    public void Pop()
    {
        UIViewBase view = null;
        if (_stack.Count > 0)
        {
            view = _stack.Peek();
            _stack.Pop();
            view.OnExit();
        }
        Resume();
    }

    private void SetUIRootParent(GameObject go)
    {
        if (go == null) return;
        if (_showPos == UIShowPos.Normal)
            go.transform.SetParent(_uiMrg.normalUI, false);
        else
            go.transform.SetParent(_uiMrg.tiptopUI, false);
    }
}
