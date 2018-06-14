using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStatus
{
    public GameRunStatus runStatus;
    public int currentGameNumber;
    public bool isCatch;
    public void SetRunStatus(GameRunStatus status)
    {
        runStatus = status;
        SaveData();
    }
    public void SetGameNumber(int num)
    {
        currentGameNumber = num;
        if (num <0)
        {
            currentGameNumber = 4;
            isCatch = false;
            runStatus = GameRunStatus.GameEnd;
        }
        SaveData();
    }
    public void SetIsCatch(bool flag)
    {
        isCatch = flag;
        SaveData();
    }
    public void SetAllPro(GameRunStatus status, bool flag, int num)
    {
        runStatus = status;
        isCatch = flag;
        currentGameNumber = num;
        SaveData();
    }
    private void SaveData()
    {
        CommTool.SaveClass<GameStatus>("GameStatus", this);
    }
}
