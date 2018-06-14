using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class CommTool
{
    public static GameObject FindObjForName(GameObject uiRoot,string name)
    {
        if (uiRoot.name == name)
            return uiRoot;
        Queue<GameObject> queue = new Queue<GameObject>();
        queue.Enqueue(uiRoot);
        GameObject temp=null;
        while (queue.Count>0)
        {
            temp= queue.Dequeue();
            if (temp.name == name)
            {
                queue = null;
                return temp;
            }
            int count= temp.transform.childCount;
            if (count>0)
            {
                for (int i = 0; i < count; i++)
                {
                   queue.Enqueue(temp.transform.GetChild(i).gameObject);
                }
            }
        }
        queue = null;
        return null;
    }

    public static T GetCompentCustom<T>(GameObject uiRoot,string name)
    {
        T t = default(T);
        GameObject obj=  FindObjForName(uiRoot, name);
        if (obj)
        {
           t= obj.GetComponent<T>();
        }
        return t;
    }

    public static GameObject InstantiateObj(GameObject model, GameObject parent, Vector3 pos, Vector3 scal, string name)
    {
        GameObject temp = null;
        temp = GameObject.Instantiate<GameObject>(model);
        temp.name = name;
        temp.transform.SetParent(parent.transform);
        temp.transform.localPosition = pos;
        temp.transform.localScale = scal;
        temp.transform.localRotation = Quaternion.identity;
        temp.SetActive(true);
        return temp;
    }

    public static void SaveIntData(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, 1);
        }
        else
        {
            int catCount = PlayerPrefs.GetInt(key);
            PlayerPrefs.SetInt(key, ++catCount);
        }
    }

    public static int GetSaveIntData(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return 0;
        }
        else
        {
           return  PlayerPrefs.GetInt(key);
        }
    }
    public static void ClearSaveData()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void SaveClass<T>(string key,T source)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StringWriter sw = new StringWriter();
        serializer.Serialize(sw, source);
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.SetString(key,sw.ToString());
    }
    public static T LoadClass<T>(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(PlayerPrefs.GetString(key));
            return (T)serializer.Deserialize(reader);
        }
        return default(T);
    }
}
