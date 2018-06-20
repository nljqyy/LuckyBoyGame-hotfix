using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;

public sealed class VersionController
{
    private const string version = "version.txt";
    private static Version vLocal;
    public static List<Version> vsList = new List<Version>();

    /// <summary>
    /// 读取本地版本 是否需要下载
    /// </summary>
    /// <param name="_version">最新版本号</param>
    /// <param name="version_Path">本地版本文件路径</param>
    /// <returns></returns>
    public static bool ReadLocalVersion(string _version,string version_Path)
    {
        if (File.Exists(version_Path))//本地存在版本号
        {
            string version_o = File.ReadAllText(version_Path);
            vLocal = new Version(version_o);
            Version v_net = new Version(_version);
            bool isNew = vLocal.CompareVersion(v_net); //为 true 网络上有新资源请求版本列表
            v_net = null;
            return isNew;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// 读取版本列表
    /// </summary>
    /// <param name="_versionList"></param>
    /// <param name="downSize"></param>
    public static void ReadVersionList(string _versionList, out ulong downSize)
    {
        downSize = 0;
        List<string> v_list = _versionList.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        Version vv = null;
        int Index = v_list.FindIndex(m => vLocal != null && m.Contains(vLocal.Content));
        for (int i = Index + 1; i < v_list.Count; i++)
        {
            vv = new Version(v_list[i]);
            vsList.Add(vv);
            downSize += vv.ContentLength;//需要下载的资源总大小
        }
        vv = null;
        v_list = null;
    }

}
