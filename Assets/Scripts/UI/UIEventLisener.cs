using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIEventLisener : EventTrigger
{
    public delegate void OnClickDelgate(GameObject go);

    public event OnClickDelgate OnClick;
    public event OnClickDelgate OnPress;
    public event OnClickDelgate OnUp;
    public event OnClickDelgate OnEnter;
    public event OnClickDelgate OnExit;


    public static UIEventLisener Get(GameObject go)
    {
        UIEventLisener li = go.GetComponent<UIEventLisener>();
        if (li == null)
        {
            li = go.AddComponent<UIEventLisener>();
        }
        return li;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick!=null)
        {
            OnClick(gameObject);
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {

    }

    public override void OnPointerUp(PointerEventData eventData)
    {

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

    }
    public override void OnPointerExit(PointerEventData eventData)
    {

    }

}
