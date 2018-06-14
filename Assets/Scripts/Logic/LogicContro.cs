using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicContro
{
    private static LogicContro instance;
    public static LogicContro Instance
    {
        get { return instance; }
    }
    private LogicContro()
    { }
    static LogicContro()
    {
        instance = new LogicContro();
    }

    private Dictionary<EntityType, List<GameEntity>> dic = new Dictionary<EntityType, List<GameEntity>>();


    public void CreateGameObject(EntityType type,string name)
    {


    }





}
