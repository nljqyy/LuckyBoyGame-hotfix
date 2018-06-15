using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using UnityEditor;

public sealed class ExportHelp
{

    /// <summary>
    /// 去打包
    /// </summary>
    /// <param name="_builds">打包的资源</param>
    /// <param name="updateList">变更的资源</param>
    public static void GoExport(AssetBundleBuild[] _builds, List<string> updateList)
    {
        if (_builds == null || _builds.Length == 0) return;
        string exportPath = PathHelp.GetExportPath();
        string assetPath = Path.Combine(exportPath, "androidRes");
        string zipPath = Path.Combine(exportPath, "Zip.zip");
        string version = Path.Combine(exportPath, "version.txt");
        if (Directory.Exists(assetPath))
        {
            if (File.Exists(version))
            {
                string _ver = File.ReadAllText(version);
                File.Delete(version);
                string content = (int.Parse(_ver) + 1).ToString();
                File.WriteAllText(version, content);
            }
            Directory.Delete(assetPath, true);
            Directory.CreateDirectory(assetPath);
        }
        else
        {
            Directory.CreateDirectory(assetPath);
            File.WriteAllText(version, "1");
        }
        BuildPipeline.BuildAssetBundles(assetPath, _builds, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        AssetDatabase.Refresh();
        Debug.Log("资源包创建成功");
        string updateResPath = ExportHelp.GetUpdateRes(assetPath, updateList);
        //创建压缩包
        //Zip.CreateZip(assetPath, zipPath);
        Zip.CreateZip(updateResPath, zipPath);
        AssetDatabase.Refresh();
        Debug.Log("zip创建成功");

        StartUpExe();
    }

    /// <summary>
    /// md5打包比对
    /// </summary>
    /// <param name="needExportResPath">需要打包的路径</param>
    /// <param name="fs">需要打包的资源</param>
    public static List<string> MD5Comparison(string needExportResPath, List<FileInfo> fs)
    {
        string currentPath = Directory.GetCurrentDirectory() + "\\";
        List<string> needExportList = new List<string>();
        Dictionary<string, string> md5Dic = new Dictionary<string, string>();
        string md5Path = needExportResPath + "/md5.txt";
        if (!File.Exists(md5Path))
        {
            foreach (FileInfo file in fs)
            {
                string fullName = file.FullName;
                fullName = fullName.Replace(currentPath, "");
                fullName = fullName.Replace("\\", "/");
                string md5 = GetAssetsGuid(fullName);
                md5Dic.Add(file.Name, md5);
                needExportList.Add(file.Name);
            }
        }
        else
        {
            var mdlist = File.ReadAllText(md5Path).Split('|').ToList();
            File.Delete(md5Path);
            for (int i = 0; i < mdlist.Count; i++)
            {
                if (string.IsNullOrEmpty(mdlist[i])) continue;
                string[] tt = mdlist[i].Split('-');
                md5Dic.Add(tt[0], tt[1]);
            }
            foreach (FileInfo file in fs)
            {
                string fullName = file.FullName;
                fullName = fullName.Replace(currentPath, "");
                fullName = fullName.Replace("\\", "/");
                string md5Name = GetAssetsGuid(fullName);
                if (md5Dic.ContainsKey(file.Name))
                {
                    if (md5Dic[file.Name] != md5Name)
                    {
                        needExportList.Add(file.Name);
                        md5Dic[file.Name] = md5Name;
                    }
                }
                else
                {
                    needExportList.Add(file.Name);
                    md5Dic[file.Name] = md5Name;
                }
            }
        }
        using (FileStream fstream = File.Create(md5Path))
        {
            StringBuilder build = new StringBuilder();
            foreach (var item in md5Dic)
            {
                build.Append(item.Key);
                build.Append("-");
                build.Append(item.Value);
                build.Append("|");
            }
            byte[] data = Encoding.UTF8.GetBytes(build.ToString());
            fstream.Write(data, 0, data.Length);
        }
        return needExportList;
    }


    /// <summary>
    /// 获得MD5
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public static string GetAssetsGuid(string filepath)
    {
        string md5str = "";
        using (FileStream fs = File.OpenRead(filepath))
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(fs);
            md5str = System.BitConverter.ToString(data).Replace("-", "").ToLower();
        }
        return md5str;
    }
    /// <summary>
    /// 获得更新的资源
    /// </summary>
    /// <param name="assetPath">资源路径</param>
    public static string GetUpdateRes(string assetPath, List<string> updatelist)
    {
        string dir = "androidRes";//manifest文件
        string UpdateRes = Path.Combine(PathHelp.GetExportPath(), "UpdateRes");
        if (Directory.Exists(UpdateRes))
            Directory.Delete(UpdateRes, true);
        Directory.CreateDirectory(UpdateRes);
        string[] assetsName = Directory.GetFiles(assetPath);
        foreach (var item in assetsName)
        {
            string tempFile = item.Substring(item.LastIndexOf("\\") + 1);
            if (tempFile.Contains(dir))
            {
                File.Copy(item, Path.Combine(UpdateRes, tempFile));
                continue;
            }
            foreach (var name in updatelist)
            {
                if (tempFile.Contains(name.ToLower()))
                {
                    File.Copy(item, Path.Combine(UpdateRes, tempFile));
                }
            }
        }
        updatelist.ForEach(o => Debug.Log("资源更新---" + o));
        return UpdateRes;
    }

    public static void StartUpExe()
    {
        System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName("HttpServer");
        if (ps.Length > 0)
        {
            ps[0].Kill();
        }
        if (Directory.Exists("D:/Res"))
            Directory.Delete("D:/Res", true);
        Directory.CreateDirectory("D:/Res");

        string version = Path.Combine(PathHelp.GetExportPath(), "version.txt");
        string zip = Path.Combine(PathHelp.GetExportPath(), "Zip.zip");
        string server_version = @"D:\Res\version.txt";
        string server_zip = @"D:\Res\Zip.zip";

        File.Copy(version, server_version);
        File.Copy(zip, server_zip);
        Debug.Log("启动服务器啦");
        System.Diagnostics.Process.Start(@"E:\XueXi\c#搭建http服务器\HttpServer-master\HttpServer-master\HTTPServer\HTTPServer\bin\Debug\HttpServer.exe");
    }

}
