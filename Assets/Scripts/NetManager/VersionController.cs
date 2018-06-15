using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public sealed class VersionController{

    private const string version = "version.txt";
    private static Version vLocal;
    public static List<Version> vsList = new List<Version>();

    /// <summary>
    /// 读取本地版本 是否需要下载
    /// </summary>
    /// <param name="_version"></param>
    /// <returns></returns>
    public static bool ReadLocalVersion(string _version)
    {
        string savePath = PathHelp.GetDownLoadPath();
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        string version_Path = Path.Combine(savePath, version);
        if (File.Exists(version_Path))//本地存在版本号
        {
            string version_o = File.ReadAllText(version_Path);
            vLocal = new Version(version_o);
            Version v_net = new Version(_version);
            bool isNew= vLocal.CompareVersion(v_net); //为 true 网络上有新资源请求版本列表
            if (isNew)
            {
                File.Delete(version_Path);
                File.WriteAllText(version_Path,_version);
            }
            return isNew;
        }
        else
            File.WriteAllText(version_Path, "1.0.1");
        return false;
    }
    /// <summary>
    /// 读取版本列表
    /// </summary>
    /// <param name="_versionList"></param>
    /// <param name="downSize"></param>
    public static int ReadVersionList(string _versionList,out ulong downSize)
    {
        downSize = 0;
        string[] v_list = _versionList.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        Version vv = null;
        int nextIndex = 0;
        foreach (var item in v_list)
        {
            vv = new Version(item);
            vsList.Add(vv);
            if (vv.Content == vLocal.Content)
                nextIndex = vsList.Count;
        }
        for (int i = nextIndex; i < vsList.Count; i++)
        {
           downSize+=vsList[nextIndex].ContentLength;//需要下载的资源总大小
        }
        return nextIndex;
    }

}
