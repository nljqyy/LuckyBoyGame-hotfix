using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IenumerMrg
{
    public sealed class IenumratorEntity
    {
        public IEnumerator currentIenumrator;
        public bool isStartUp = false;//是否启动协成
        public bool isPlayOver = false;//是否播放完成
        public IeType type;
        public IenumratorEntity()
        {
            if (!isStartUp)
            {
                isStartUp = true;
                SDKManager.Instance.StartCoroutine(this.StartUp());
            }
        }
        private IEnumerator StartUp()
        {
            while (isStartUp)
            {
                if (currentIenumrator != null && currentIenumrator.MoveNext())
                {
                    isPlayOver = false;
                    yield return currentIenumrator.Current;
                }
                else
                {
                    isPlayOver = true;
                    yield return null;
                }
            }
        }
    }

    private IenumerMrg()
    {

    }
    static IenumerMrg()
    {
        _instance = new IenumerMrg();
    }
    private static IenumerMrg _instance;
    public static IenumerMrg Instance
    {
        get { return _instance; }
    }

    private Dictionary<IeType, List<IenumratorEntity>> dicIe = new Dictionary<IeType, List<IenumratorEntity>>();
  
    /// <summary>
    /// 执行协成
    /// </summary>
    /// <param name="e"></param>
    /// <param name="type"></param>
    public void StartIenumrator(IEnumerator e, IeType type)
    {
        IenumratorEntity ie = null;
        List<IenumratorEntity> listIe = null;
        if (dicIe.TryGetValue(type, out listIe))
        {
            if (type == IeType.Voice)//没有执行完毕 可随时替换
            {
                ie = listIe[0];
            }
            else
            {
                ie = listIe.Find(tp => tp.currentIenumrator == null || tp.isPlayOver);//执行完的
                if (ie == null)
                {
                    ie = new IenumratorEntity();
                    ie.type = type;
                    listIe.Add(ie);
                    dicIe[type] = listIe;
                }
            }
        }
        else
        {
            listIe = new List<IenumratorEntity>();
            ie = new IenumratorEntity();
            ie.type = type;
            listIe.Add(ie);
            dicIe.Add(type, listIe);
        }
        ie.currentIenumrator = e;
    }

    //游戏推出时关闭携程
    public void StopAllIenumrator()
    {
        SDKManager.Instance.StopAllCoroutines();
        foreach (var item in dicIe)
        {
            item.Value.ForEach(e => e.isStartUp = false);
        }
        dicIe.Clear();
    }
    /// <summary>
    /// 根据类型停止某个协成
    /// </summary>
    /// <param name="ty"></param>
    public void StopIeByType(IeType ty)
    {
        List<IenumratorEntity> listIe = null;
        if (dicIe.TryGetValue(ty, out listIe))
        {
            listIe.ForEach(m => m.isStartUp = false);
            dicIe.Remove(ty);
        }
    }

}
