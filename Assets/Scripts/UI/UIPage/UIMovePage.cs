using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using XLua;

[Hotfix]
public sealed class UIMovePage : UIDataBase
{
    public const string NAME = "UIMovePage.prefab";
    public override UIShowPos ShowPos
    {
        get
        {
            return UIShowPos.Normal;
        }
    }
    public override HidePage hidePage
    {
        get
        {
            return HidePage.Destory;
        }
    }
    private GameObject modelWheel;
    private GameObject modelPang;
    private GameObject parentWheel;
    private GameObject parentPang;
    private GameObject tempPange;
    private bool Stop = false;
    private List<GameEntity> list = new List<GameEntity>();
    public List<GameEntity> listPang = new List<GameEntity>();
    private float xiaoPang = 0.7f;
    private float wheel = 1.2f;
    #region  小胖速度参数
    private float timeRun = 0;
    private int speedType = 3;
    private float maxSpeed = 1000;
    private float minSpeed = 400;
    private float currentSpeed = 0;
    private float randomSpeed = 0;
    private int speednum = 0;
    private float[] speeds = new float[] { 400, 600, 800, 1000 };
    #endregion

    public override void Init()
    {
        base.Init();
        modelWheel = CommTool.FindObjForName(gameObject, "wheel_img");
        parentWheel = CommTool.FindObjForName(gameObject, "grid");
        modelPang = CommTool.FindObjForName(gameObject, "pang");
        parentPang = CommTool.FindObjForName(gameObject, "xiaopang");
        Reg();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        CreateGameObject();
    }

    private void Reg()
    {
        EventHandler.RegisterEvnet(EventHandlerType.FishHookCheck, FishHookCheck);
        EventHandler.RegisterEvnet(EventHandlerType.UpFinish, UpFinish);
        EventHandler.RegisterEvnet(EventHandlerType.RestStart, RestStart);
        EventHandler.RegisterEvnet(EventHandlerType.GameEnd, GameEnd);

    }
    private void UnReg()
    {
        EventHandler.UnRegisterEvent(EventHandlerType.FishHookCheck, FishHookCheck);
        EventHandler.UnRegisterEvent(EventHandlerType.UpFinish, UpFinish);
        EventHandler.UnRegisterEvent(EventHandlerType.RestStart, RestStart);
        EventHandler.UnRegisterEvent(EventHandlerType.GameEnd, GameEnd);
    }

    //创建物体
    void CreateGameObject()
    {
        GameObject tempObj = null;
        GameEntity entity = null;
        if (modelWheel && parentWheel)
        {
            Image img;
            Text duiImg;
            Text duiImg2;
            Sprite sp = null;
            if (modelWheel && parentWheel)
            {
                for (int i = 0; i < 14; i++)
                {
                    float posX = i * 86;
                    if (i == 0)
                    {
                        tempObj = modelWheel;
                        tempObj.SetActive(true);
                    }
                    else
                    {
                        tempObj = CommTool.InstantiateObj(modelWheel, parentWheel, new Vector3(posX, 0, 0), Vector3.one * wheel, "wheels" + i);
                    }
                    tempObj.transform.GetComponent<Image>().overrideSprite = sp;
                    entity = new GameEntity(tempObj);
                    list.Add(entity);
                }
            }
            if (modelPang && parentPang)
            {
                tempPange = CommTool.InstantiateObj(modelPang, parentPang, Vector3.zero, Vector3.one * xiaoPang, "tempPange");
                img = tempPange.GetComponent<Image>();
                duiImg2 = CommTool.GetCompentCustom<Text>(tempPange, "dui");
                tempPange.SetActive(false);
                string img_name = "j-";
                string new_name = string.Empty;
                Vector3 pos = Vector3.zero;
                for (int i = 0; i < 4; i++)
                {
                    int num = i + 1;
                    new_name = img_name + num;
                    if (i == 0)
                    {
                        tempObj = modelPang;
                        tempObj.name = new_name;
                        tempObj.SetActive(true);
                    }
                    else
                    {
                        pos.x = i * 380;
                        tempObj = CommTool.InstantiateObj(modelPang, parentPang, pos, Vector3.one * xiaoPang, new_name);
                    }
                    sp = UIAtlasManager.LoadSprite(UIAtlasName.UIMain, new_name);
                    tempObj.transform.GetComponent<Image>().overrideSprite = sp;
                    duiImg = CommTool.GetCompentCustom<Text>(tempObj, "dui");
                    entity = new GameEntity(tempObj, img, sp, duiImg, duiImg2);
                    listPang.Add(entity);
                }

                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                        listPang[i].last = listPang[3];
                    else
                        listPang[i].last = listPang[i - 1];
                }
            }
        }
    }


    void Update()
    {
        if (!Stop)
        {
            for (int i = 0; i < listPang.Count; i++)
            {
                if (listPang[i] != null)
                    listPang[i].Update(GetSpeed());
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                    list[i].StartWheel();
            }
        }

    }

    private float GetSpeed()
    {
        if (SDKManager.Instance.randomQuXian == 1) return 160 * Time.deltaTime;
        else if (SDKManager.Instance.randomQuXian == 2) return 400 * Time.deltaTime;
        else
        {
            timeRun += Time.deltaTime;
            float t = 0;
            if (timeRun <= 6)
            {
                randomSpeed = RandomSpeed(randomSpeed, 7);
                t = randomSpeed / 100.0f;
                if (timeRun <= 5)
                {
                    currentSpeed += t * Time.deltaTime;
                }
                else
                {
                    currentSpeed -= t * Time.deltaTime;
                }
            }
            else if (timeRun <= 12)
            {
                randomSpeed = RandomSpeed(randomSpeed, 10);
                t = randomSpeed / 100.0f;
                if (timeRun <= 8)
                {
                    currentSpeed += t * Time.deltaTime;
                }
                else
                {
                    currentSpeed -= t * Time.deltaTime;
                }
            }
            else if (timeRun <= 18)
            {
                randomSpeed = RandomSpeed(randomSpeed, 16);
                t = randomSpeed / 100.0f;
                if (timeRun <= 15)
                {
                    currentSpeed += t * Time.deltaTime;
                }
                else
                {
                    currentSpeed -= t * Time.deltaTime;
                }
            }
            else
            {
                timeRun = 0;
            }
        }
        if (currentSpeed >= maxSpeed * Time.deltaTime) currentSpeed = maxSpeed * Time.deltaTime;
        if (currentSpeed <= minSpeed * Time.deltaTime) currentSpeed = minSpeed * Time.deltaTime;
        return currentSpeed;
    }
    private float RandomSpeed(float speed, int t = 0)
    {
        float sp = speed;
        while (sp == speed)
        {
            int index = UnityEngine.Random.Range(0, speeds.Length);
            sp = speeds[index];
        }
        return sp;
    }

    private void FishHookCheck(object data)
    {
        SDKManager.Instance.startCarwTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        bool isReach = ComputeTimes();
        foreach (var item in listPang)
        {
            item.CheckFishHook(data, isReach);
            if (item.catchty != CatchTy.CatchErrorPos)//抓中
            {
                if (item.catchty == CatchTy.NoCatch)//没抓起
                {
                    item.ShowDui();
                    item.LeftRightMove();//左右晃动
                }
                break;
            }
            else //偏啦
            {
                if (item.isCatchPain)
                {
                    item.ShowDui();
                }
            }
        }
    }

    private void UpFinish(object data)
    {
        CommTool.SaveIntData(CatchTimes.PlayerCatch.ToString());
        Stop = true;
        GameEntity ge = listPang.Find(e => e.catchty != CatchTy.CatchErrorPos);//抓中的
        CatchTy tempCatch = ge == null ? CatchTy.CatchErrorPos : ge.catchty;
        EventHandler.ExcuteEvent(EventHandlerType.Success, tempCatch);
        if (ge != null && ge.catchty == CatchTy.Catch)
        {
            tempPange.SetActive(false);
            tempPange.transform.SetParent(parentPang.transform);
            tempPange.transform.localPosition = Vector3.zero;
        }
    }
    //重新开始
    private void RestStart(object data)
    {
        Stop = false;
        listPang.ForEach(e => e.SetCaught());
    }
    //游戏结束
    private void GameEnd(object data)
    {
        Stop = true;
    }
    //计算次数 是否达到
    private bool ComputeTimes()
    {
        int catchCount = CommTool.GetSaveIntData(CatchTimes.Catch.ToString());//这台机器抓中过的次数
        int playrCatch = CommTool.GetSaveIntData(CatchTimes.PlayerCatch.ToString());//这台机器抓中过的次数
        Debug.Log("抓中次数---" + catchCount + "---抓了多少次---" + playrCatch);
        bool flag = catchCount >= SDKManager.Instance.winningTimes;
        if (playrCatch >= SDKManager.Instance.carwBasicCount)
        {
            CommTool.ClearSaveData();
        }
        return flag;
    }

    private void OnDestroy()
    {
        UnReg();
        foreach (var item in list)
        {
            Destroy(item.self);
        }
        list.Clear();
        foreach (var item in listPang)
        {
            Destroy(item.self);
        }
        listPang.Clear();
        Destroy(tempPange);
    }
}
