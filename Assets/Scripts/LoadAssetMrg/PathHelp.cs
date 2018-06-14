using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class PathHelp
{
    public const string unZip = "unZip/";
    public static string GetDownLoadPath()
    {
        string path = "";
#if UNITY_EDITOR
        path = Application.dataPath + "/DownLoad/";
#elif UNITY_ANDROID
       path= Application.persistentDataPath+"/";
#endif
        return path;
    }

   public  static string GetExportPath()
    {
        return Application.dataPath + "/ExportAssetBuilds/";
        
    }
}
