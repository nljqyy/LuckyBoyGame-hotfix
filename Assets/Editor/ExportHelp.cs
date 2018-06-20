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
    #region 公有函数
    /// <summary>
    /// 去打包
    /// </summary>
    /// <param name="_builds">打包的资源</param>
    /// <param name="updateList">变更的资源</param>
    public static void GoExport(AssetBundleBuild[] _builds, List<string> updateList)
    {
        if (_builds == null || _builds.Length == 0) return;
        string zipPath;
        string zipContent;
        string assetPath = WriteVersionToLocal(out zipPath, out zipContent);
        BuildPipeline.BuildAssetBundles(assetPath, _builds, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        AssetDatabase.Refresh();
        Debug.Log("资源包创建成功");
        string updateResPath = ExportHelp.GetUpdateRes(assetPath, updateList);
        //创建压缩包
        //Zip.CreateZip(assetPath, zipPath);
        long zipSize = Zip.CreateZip(updateResPath, zipPath);
        WriteVersionListToLocal(zipContent, zipSize);
        AssetDatabase.Refresh();
        Debug.Log("zip创建成功");

        StartUpExe(zipContent);
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
    /// 获得更新的资源 打包
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

    /// <summary>
    /// 清空debug信息
    /// </summary>
    public static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }

    #endregion

    #region 私有函数
    /// <summary>
    /// 启动服务器
    /// </summary>
    private static void StartUpExe(string zipContent)
    {
        System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName("HttpServer");
        if (ps.Length > 0)
        {
            ps[0].Kill();
        }
        if (!Directory.Exists("D:/Res"))
            Directory.CreateDirectory("D:/Res");
        zipContent += ".zip";
        string version = Path.Combine(PathHelp.GetExportPath(), "version.txt");
        string versionList = Path.Combine(PathHelp.GetExportPath(), "versionList.txt");
        string zip = Path.Combine(PathHelp.GetExportPath(), zipContent);
        string server_version = @"D:\Res\version.txt";
        string server_versionList = @"D:\Res\versionList.txt";
        string server_zip = @"D:\Res\" + zipContent;
        if (File.Exists(server_version)) File.Delete(server_version);
        if (File.Exists(server_versionList)) File.Delete(server_versionList);
        if (File.Exists(server_zip)) File.Delete(server_zip);
        File.Copy(version, server_version);
        File.Copy(versionList, server_versionList);
        File.Copy(zip, server_zip);
        Debug.Log("启动服务器啦");
        System.Diagnostics.Process.Start(@"E:\XueXi\c#搭建http服务器\HttpServer-master\HttpServer-master\HTTPServer\HTTPServer\bin\Debug\HttpServer.exe");
    }
    /// <summary>
    /// 更新版本号
    /// </summary>
    /// <param name="zipPath">打包资源压缩路径</param>
    /// <returns>打包资源存储路径</returns>
    private static string WriteVersionToLocal(out string zipPath, out string zipContent)
    {
        zipPath = string.Empty;
        zipContent = string.Empty;
        string exportPath = PathHelp.GetExportPath();
        string assetPath = Path.Combine(exportPath, "androidRes");
        string version = Path.Combine(exportPath, "version.txt");
        if (Directory.Exists(assetPath))
            Directory.Delete(assetPath, true);
        Directory.CreateDirectory(assetPath);
        if (File.Exists(version))
        {
            string _ver = File.ReadAllText(version);
            string[] vers = _ver.Split('.');
            if (vers.Length > 1)
            {
                if (vers[2] == "9")
                {
                    vers[2] = "0";
                    if (Convert.ToInt32(vers[1]) < 9)
                        vers[1] = (Convert.ToInt32(vers[1]) + 1).ToString();
                    else
                    {
                        vers[1] = "0";
                        if (Convert.ToInt32(vers[0]) < 9)
                            vers[0] = (Convert.ToInt32(vers[0]) + 1).ToString();
                    }
                }
                else
                    vers[2] = (Convert.ToInt32(vers[2]) + 1).ToString();
                zipContent = string.Join(".", vers);
                zipPath = Path.Combine(exportPath, zipContent + ".zip");
                File.WriteAllText(version, zipContent);
            }
            else
                Debug.LogError("版本号格式错误");
        }
        else
        {
            zipContent = "1.0.0";
            File.WriteAllText(version, zipContent);
            zipPath = Path.Combine(exportPath, zipContent + ".zip");
        }
        return assetPath;

    }
    /// <summary>
    /// 向版本列表写入数据
    /// </summary>
    /// <param name="zipConent">版本号</param>
    /// <param name="zipSize">资源大小</param>
    private static void WriteVersionListToLocal(string zipConent, long zipSize)
    {
        string exportPath = PathHelp.GetExportPath();
        string versionListPath = Path.Combine(exportPath, "versionList.txt");
        string vv = string.Empty;
        if (File.Exists(versionListPath))
            vv = File.ReadAllText(versionListPath);
        StringBuilder str = new StringBuilder(vv);
        str.AppendLine();
        str.Append(zipConent);
        str.Append("-");
        str.Append(zipSize);
        File.WriteAllText(versionListPath, str.ToString());
    }
  
    #endregion


}
