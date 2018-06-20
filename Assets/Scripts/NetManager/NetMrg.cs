using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using System;
using System.IO;
using System.Text;
using XLua;

[Hotfix]
public sealed class NetMrg : MonoSingleton<NetMrg>
{
    private BestHttpImpl httpImpl;
    private const string url = "http://192.168.15.162:4050";
    private const string version = "version.txt";
    private const string versionList = "versionList.txt";
    private HTTPMethods method = HTTPMethods.Post;
    private Action finish = null;
    private Action<float> downZip = null;
    private string tempPath = "";
    private ulong downSize;//文件总大小
    private int currentVersionIndex = 0;//版本索引
    private string version_Path = "";

    // Use this for initialization
    void Awake()
    {
        tempPath = Path.Combine(PathHelp.GetDownLoadPath(), "TempRes.temp");
        if (File.Exists(tempPath))
            File.Delete(tempPath);
        httpImpl = new BestHttpImpl();
        httpImpl.SetHttpParams();
        httpImpl.AddHead("content-type", "application/json");
        PlayerPrefs.SetInt("DownloadProgress", 0);
    }
    /// <summary>
    /// 请求版本号
    /// </summary>
    /// <param name="down">下载进度委托</param>
    /// <param name="callback">下载完成委托</param>
    public void RequestVersion(Action<float> down = null, Action callback = null)
    {
        downZip = down;
        finish = callback;
        string httpUrl = string.Format("{0}/{1}", url, version);
        SendRequest(httpUrl, false, SaveVesionToLocal);
    }
    /// <summary>
    /// 请求版本列表
    /// </summary>
    private void RequestVersionList()
    {
        string httpUrl = string.Format("{0}/{1}", url, versionList);
        SendRequest(httpUrl, false, SaveVersionList);
    }
    /// <summary>
    /// 请求压缩包
    /// </summary>
    /// <param name="version">版本号</param>
    private void RequestZip(string version)
    {
        Debug.Log("请求版本-----" + version);
        string httpUrl = string.Format("{0}/{1}.zip", url, version);
        SendRequest(httpUrl, true, SaveZipToLocal);
    }
    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="httpUrl"></param>
    /// <param name="isOpenStram">是否打开多次回调模式</param>
    /// <param name="action">响应</param>
    private void SendRequest(string httpUrl, bool isOpenStram, Action<HTTPResponse> action)
    {
        Debug.Log("开始连接服务器---" + httpUrl);
        Dictionary<string, string> requestParams = new Dictionary<string, string>
        {
            //{ "name","nlj"},
            //{ "sex","boy"}
        };
        if (method == HTTPMethods.Get)
        {
            httpImpl.Get(httpUrl, action);
        }
        else if (method == HTTPMethods.Post)
        {
            httpImpl.Post(httpUrl, requestParams, isOpenStram, action);
        }
    }


    private void SaveVesionToLocal(HTTPResponse response)
    {
        string savePath = PathHelp.GetDownLoadPath();
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        version_Path = Path.Combine(savePath, version);
        Debug.Log("版本号获得成功--" + response.DataAsText);
        bool isUpdate = VersionController.ReadLocalVersion(response.DataAsText,version_Path);
        if (isUpdate)//需要更新
        {
            RequestVersionList();
        }
        else
        {
            Debug.Log("已是最新版本不需更新");
            if (finish != null)
                finish();
        }
    }

    private void SaveVersionList(HTTPResponse response)
    {
        VersionController.ReadVersionList(response.DataAsText, out downSize);
        currentVersionIndex = -1;
        RequestZipNext();
    }

    /// <summary>
    /// 保存下载的数据到本地
    /// </summary>
    /// <param name="response"></param>
    private void SaveZipToLocal(HTTPResponse response)
    {
        ProcessFragments(response.GetStreamedFragments());
        if (response.IsStreamingFinished)
        {
            Debug.Log("zip资源获得成功");
            string newPath = PathHelp.GetDownLoadPath() + "data.zip";
            if (File.Exists(tempPath))
            {
                if (File.Exists(newPath))
                {
                    File.Delete(newPath);
                }
                File.Move(tempPath, newPath);
                File.Delete(tempPath);
            }
            Zip.UnZip(newPath, PathHelp.GetDownLoadPath() + PathHelp.unZip);
            Debug.Log("解压完成");
            RequestZipNext();
        }
    }
    /// <summary>
    /// 请求下个版本
    /// </summary>
    private void RequestZipNext()
    {
        if (currentVersionIndex >= 0)
        {
            File.WriteAllText(version_Path, VersionController.vsList[currentVersionIndex].Content);//保存版本号到本地
        }
        if (++currentVersionIndex < VersionController.vsList.Count)//请求下一个版本
        {
            RequestZip(VersionController.vsList[currentVersionIndex].Content);
        }
        else
        {
            if (finish != null)
                finish();
        }
    }

    //下载进度
    private void ProcessFragments(List<byte[]> fragments)
    {
        if (fragments != null && fragments.Count > 0)
        {
            using (FileStream fs = new FileStream(tempPath, FileMode.Append))
            {
                for (int i = 0; i < fragments.Count; ++i)
                {
                    fs.Write(fragments[i], 0, fragments[i].Length);
                    int downloaded = PlayerPrefs.GetInt("DownloadProgress") + fragments[i].Length;
                    PlayerPrefs.SetInt("DownloadProgress", downloaded);
                }
            }
            PlayerPrefs.Save();
            // float  progress = PlayerPrefs.GetInt("DownloadProgress") / (float)PlayerPrefs.GetInt("DownloadLength");
            float progress = PlayerPrefs.GetInt("DownloadProgress") / (float)downSize;
            if (downZip != null && progress < 99)
                downZip(progress);
        }
    }
    
}
