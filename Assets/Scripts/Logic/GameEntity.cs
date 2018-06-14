using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using XLua;

[Hotfix]
public sealed class GameEntity
{
    private Image tempImg;
    private Text dui;
    private Text tempDui;
    private Sprite sp;
    private int MoveSpeed = 200;
    private Vector3 target = new Vector3(-300, 0, 0);
    private Vector3 pos;
    private Vector3 vel;
    private float noCatchNum = 2f;//抖动
    private float catchPian = 3;//抓偏
    public GameObject self;
    public GameEntity last;
    public GameEntity next;
    public CatchTy catchty { get; private set; }
    public bool isCatchPain { get; private set; }

    public GameEntity(GameObject self)
    {
        this.self = self;
        pos = self.transform.localPosition;
    }
    public GameEntity(GameObject self, Image tempImg, Sprite sp, Text dui, Text duitemp)
    {
        this.self = self;
        this.tempImg = tempImg;
        this.sp = sp;
        this.dui = dui;
        this.tempDui = duitemp;
        tempDui.transform.parent.gameObject.SetActive(false);
        dui.transform.parent.gameObject.SetActive(false);
    }
    public void Update(float speed = 0)
    {
        if (self.transform.localPosition.x <= -300)
        {
            // self.transform.localPosition = new Vector3(1500, 0, 0);
            self.transform.localPosition = new Vector3(last.self.transform.localPosition.x + 380, 0, 0);
            dui.transform.parent.gameObject.SetActive(false);
        }
        Vector3 v1 = target - self.transform.localPosition;
        self.transform.localPosition += v1.normalized * speed;
        // pos.x = self.transform.localPosition.x - 2.7f - speed;
        //self.transform.localPosition = Vector3.SmoothDamp(self.transform.localPosition, pos, ref vel, Time.deltaTime);
    }

    public void StartWheel()
    {
        self.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * MoveSpeed);
    }

    #region 检测是否抓中
    // 80%逻辑
    //public void CheckFishHook(object data)
    //{
    //    Transform t = (Transform)data;
    //    float posX = Convert.ToSingle(t.position.x);
    //    currentposX = self.transform.position.x;
    //    if (SDKManager.Instance.checkProperty < 1f)
    //        SDKManager.Instance.checkProperty = 1f;
    //    if (SDKManager.Instance.checkProperty > 5)
    //        SDKManager.Instance.checkProperty = 5;
    //    float minX = posX - SDKManager.Instance.checkProperty;
    //    float maxX = posX + SDKManager.Instance.checkProperty;
    //    Debug.Log("CurrentPosX----" + currentposX + "-----MinX-----" + minX + "-------maxX----" + maxX);
    //    bool isCaught = currentposX >= minX && currentposX <= maxX;
    //    if (isCaught)
    //    {
    //        int number = UnityEngine.Random.Range(1, 11);
    //        number = 9;
    //        if (number <= 8)//80%抓起
    //        {
    //            self.SetActive(false);
    //            catchty = CatchTy.Catch;
    //            tempImg.transform.localPosition = self.transform.localPosition;
    //            tempImg.transform.SetParent(t.parent);
    //            tempImg.overrideSprite = sp;
    //            tempImg.gameObject.SetActive(true);
    //            tempImg.transform.DOMove(new Vector3(posX, -4, 0), 0.2f);
    //            number = UnityEngine.Random.Range(1, 11);
    //            bool flag = false;
    //            int catchCount = CommTool.GetSaveIntData(CatchTimes.Catch.ToString());//这台机器抓中过的次数
    //            int playrCatch = CommTool.GetSaveIntData(CatchTimes.PlayerCatch.ToString());//这台机器抓中过的次数
    //            if (playrCatch <= 100)
    //            {
    //                flag = catchCount >= 6;
    //                if (playrCatch == 100)
    //                    CommTool.ClearSaveData();
    //            }
    //            if (flag || SDKManager.Instance.caughtTime > 0)//已抓中过 必掉
    //                number = 1;
    //            if (number <= 2)//20%掉落
    //            {
    //                catchty = CatchTy.Drop;
    //                SDKManager.Instance.StartCoroutine(Drop());
    //            }
    //        }
    //        else
    //            catchty = CatchTy.NoCatch;
    //    }
    //    else
    //        catchty = CatchTy.CatchErrorPos;

    //}


    // ----新逻辑------------------------------------------------------------

    public void CheckFishHook(object data, bool isReach)
    {
        Transform t = (Transform)data;
        float posX = Convert.ToSingle(t.position.x);
        float currentposX = self.transform.position.x;
        float min = posX - SDKManager.Instance.checkProperty;
        float max = posX + SDKManager.Instance.checkProperty;
        Debug.Log("CurrentPosX----" + currentposX + "-----MinX-----" + min + "-------maxX----" + max + "====pos====" + posX);
        if (currentposX >= min && currentposX <= max)
        {
            self.SetActive(false);
            catchty = CatchTy.Catch;
            SDKManager.Instance.gameXP = this;
            tempImg.transform.localPosition = self.transform.localPosition;
            tempImg.transform.SetParent(t.parent);
            tempImg.overrideSprite = sp;
            tempImg.gameObject.SetActive(true);
            tempImg.transform.DOMove(new Vector3(posX, -6, 0), 0.2f);
            //是否掉
            int num = UnityEngine.Random.Range(1, 101);
            Debug.Log("概率值----" + num);
            if (isReach || SDKManager.Instance.caughtTime > 0 || num > SDKManager.Instance.probability)//已抓中过 必掉  比例达到百分之六
            {
                catchty = CatchTy.Drop;
            }
        }
        else if (currentposX >= min - noCatchNum && currentposX <= max + noCatchNum)
            catchty = CatchTy.NoCatch;
        else
            catchty = CatchTy.CatchErrorPos;
        isCatchPain = currentposX >= min - catchPian && currentposX <= max + catchPian;
    }

    #endregion
    //重置数据
    public void SetCaught()
    {
        catchty = CatchTy.CatchErrorPos;
        dui.transform.parent.gameObject.SetActive(false);
        self.SetActive(true);
    }
    //小胖说话
    public void ShowDui(Text tt = null)
    {
        Text duii = tt == null ? dui : tt;
        switch (catchty)
        {
            case CatchTy.NoCatch:
            case CatchTy.CatchErrorPos:
                duii.text = "这准头可抓不中哦~";
                duii.transform.parent.gameObject.SetActive(true);
                break;
            case CatchTy.Drop:
                duii.text = "吓死宝宝啦差点被抓走~";
                duii.transform.parent.gameObject.SetActive(true);
                break;
        }
    }
    //小胖抖动
    public void LeftRightMove()
    {
        float x = self.transform.position.x - 0.5f;
        self.transform.DOMoveX(x, 0.07f).SetLoops(5, LoopType.Yoyo).OnComplete(() => Debug.Log("晃动完成"));
    }
    //掉落
    public void DropIe()
    {
        ShowDui(tempDui);
        tempImg.transform.SetParent(self.transform.parent);
        tempImg.transform.DOMoveY(-5.5f, 1f).OnComplete(() =>
        {
            tempImg.gameObject.SetActive(false);
            tempDui.transform.parent.gameObject.SetActive(false);
        });
    }

}
