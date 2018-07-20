using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class UIContext
{
    private Stack<UIViewBase> _stack = new Stack<UIViewBase>();
    private UIManager _uiMrg;
    public UIContext(UIManager uiMrg)
    {
        _uiMrg = uiMrg;
    }
    public void Push(UIViewBase view)
    {
        if (_stack.Count > 0)
        {
            foreach (var item in _stack)
                view.OnPause();
        }
        SetUIRootParent(view);
        _stack.Push(view);
        if (view.isLoaded)
            view.OnEnter();
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
        if (_stack.Count > 0)
        {
            view = _stack.Peek();
            SetUIRootParent(view);
            view.OnResume();
        }
    }
    private void SetUIRootParent(UIViewBase view)
    {
        if (view == null) return;
        if (view.ShowPos == UIShowPos.Normal)
            view.gameObject.transform.SetParent(_uiMrg.normalUI, false);
        else
            view.gameObject.transform.SetParent(_uiMrg.tiptopUI, false);
    }
}
