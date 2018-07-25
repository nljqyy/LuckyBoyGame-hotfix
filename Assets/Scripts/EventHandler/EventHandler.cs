using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using XLua;

public interface IMsgEvent
{
}
[Hotfix]
public sealed class MsgHandler
{
    public int eventId { get; private set; }
    public string msgType { get; private set; }
    public int _hashCode { get; private set; }
    public Action<object[]> msgAct { get; private set; }
    public MsgHandler(int _eventId, string _msgType, Action<object[]> _msgAct)
    {
        eventId = _eventId;
        msgType = _msgType;
        msgAct = _msgAct;
        _hashCode = this.GetHashCode();
    }
    public MsgHandler(string _msgType, Action<object[]> _msgAct)
    {
        eventId = 0;
        msgType = _msgType;
        msgAct = _msgAct;
        _hashCode = this.GetHashCode();
    }
    public void Reset()
    {
        msgType = null;
        msgAct = null;
    }
}
[Hotfix]
public static class EventHandler
{
    private static readonly Dictionary<string, List<MsgHandler>> msgMap = 
        new Dictionary<string, List<MsgHandler>>();

    private static readonly Dictionary<IMsgEvent, MsgHandler[]> msgEntityMap =
        new Dictionary<IMsgEvent, MsgHandler[]>();
    /// <summary>
    /// 注册消息
    /// </summary>
    /// <param name="self"></param>
    /// <param name="_msgHandler"></param>
    public static void RegisterMsgEvent(this IMsgEvent self, params MsgHandler[] _msgHandler)
    {
        if (_msgHandler.Length > 0)
        {
            if (!msgEntityMap.ContainsKey(self))
                msgEntityMap.Add(self, _msgHandler);
            List<MsgHandler> mhlist = null;
            for (int i = 0; i < _msgHandler.Length; i++)
            {
                if (_msgHandler[i] == null) continue;
                mhlist = GetRegMsgHandlers(_msgHandler[i].msgType);
                if (mhlist == null)
                {
                    mhlist = new List<MsgHandler>() { _msgHandler[i] };
                    msgMap.Add(_msgHandler[i].msgType, mhlist);
                }
                else
                {
                    mhlist.Add(_msgHandler[i]);
                    mhlist.Sort((m1, m2) => m1.eventId - m2.eventId);
                    msgMap[_msgHandler[i].msgType] = mhlist;
                }
            }
        }
    }
    /// <summary>
    /// 取消注册消息
    /// </summary>
    /// <param name="self">此对象注册的全部消息</param>
    public static void UnRegisterMsgEvent(this IMsgEvent self)
    {
        if (msgEntityMap.ContainsKey(self))
        {
            MsgHandler handler = null;
            List<MsgHandler> mhlist = null;
            for (int i = 0; i < msgEntityMap[self].Length; i++)
            {
                handler = msgEntityMap[self][i];
                if (handler == null) continue;
                mhlist = GetRegMsgHandlers(handler.msgType);
                if (mhlist != null && mhlist.Contains(handler))
                {
                    handler.Reset();
                    mhlist.Remove(handler);
                }
                handler = null;
            }
            msgEntityMap.Remove(self);
        }
    }
    /// <summary>
    /// 根据消息类型删除
    /// </summary>
    /// <param name="self"></param>
    /// <param name="msgType">消息类型</param>
    public static void UnRegisterMsgEvent(this IMsgEvent self,string msgType)
    {
        if (msgEntityMap.ContainsKey(self))
        {
            MsgHandler[] msgHandlers = msgEntityMap[self];
            int index = 0;
            for (int i = 0; i < msgHandlers.Length; i++)
            {
                if (msgHandlers[i] != null && msgHandlers[i].msgType == msgType)
                {
                    index = i;
                    break;
                }
            }
            List<MsgHandler> mhlist = null;
            msgMap.TryGetValue(msgType, out mhlist);
            if (mhlist != null)
            {
                for (int i = mhlist.Count-1; i>=0; i--)
                {
                    if (mhlist[i] == msgHandlers[index])
                    {
                        mhlist[i].Reset();
                        mhlist[i] = null;
                        mhlist.RemoveAt(i);
                        msgHandlers[index] = null;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 消息分发
    /// </summary>
    /// <param name="type"></param>
    /// <param name="paramList"></param>
    public static void ExcuteMsgEvent(string type, params object[] paramList)
    {
        List<MsgHandler> mlist = GetRegMsgHandlers(type);
        if (mlist != null && mlist.Count > 0)
        {
            for (int i = 0; i < mlist.Count; i++)
            {
                if (mlist[i] != null && mlist[i].msgType == type && mlist[i].msgAct != null)
                    mlist[i].msgAct(paramList);
            }
        }
    }

    private static List<MsgHandler> GetRegMsgHandlers(string type)
    {
        List<MsgHandler> ilist = null;
        msgMap.TryGetValue(type, out ilist);
        return ilist;
    }

    public static void Clear()
    {
        List<MsgHandler>[] mhlist = msgMap.Values.ToArray();
        List<MsgHandler> imsg = null;
        for (int i = 0; i < mhlist.Length; i++)
        {
            imsg = mhlist[i];
            for (int k = 0; k < imsg.Count; k++)
            {
                imsg[k].Reset();
                imsg[k] = null;
            }
            imsg = null;
        }
        msgMap.Clear();
        msgEntityMap.Clear();
    }
    #region  没有排序写法
    //private static Dictionary<string, Action<object[]>> dic = new Dictionary<string, Action<object[]>>();
    //public static void RegisterEvnet(string type, Action<object[]> action)
    //{
    //    Action<object[]> act = GetRegAction(type);
    //    act += action;
    //    dic[type] = act;
    //}
    //public static void UnRegisterEvent(string type, Action<object[]> action)
    //{
    //    Action<object[]> act = GetRegAction(type);
    //    if (act != null)
    //    {
    //        act -= action;
    //        dic[type] = act;
    //    }
    //}

    //public static void ExcuteEvent(string type,params object[] objs)
    //{
    //    Action<object[]> act = GetRegAction(type);
    //    if (act != null)
    //        act(objs);
    //}

    //private static Action<object[]> GetRegAction(string type)
    //{
    //    Action<object[]> act = null;
    //    dic.TryGetValue(type, out act);
    //    return act;
    //}

    //public static void Clear()
    //{
    //    List<Action<object[]>> temp = dic.Values.ToList();
    //    for (int i = 0; i < temp.Count; i++)
    //    {
    //        temp[i] = null;
    //    }
    //    dic.Clear();
    //}
    #endregion 

}
