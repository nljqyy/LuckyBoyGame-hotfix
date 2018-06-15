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
    private Version local_version;
    private ulong downSize;

    // Use this for initialization
    void Awake()
    {
        tempPath = Path.Combine(PathHelp.GetDownLoadPath(), "TempRes.temp");
        httpImpl = new BestHttpImpl();
        httpImpl.SetHttpParams();
        httpImpl.AddHead("content-type", "application/json");
        PlayerPrefs.DeleteKey("DownloadLength");
    }

    public void RequestVersion(Action<float> down=null, Action callback = null)
    {
        finish = callback;
        downZip = down;
        string httpUrl = string.Format("{0}/{1}", url, version);
        SendRequest(httpUrl, false, SaveVesionToLocal);
    }
    private void RequestVersionList()
    {
        string httpUrl = string.Format("{0}/{1}", url, versionList);
        SendRequest(httpUrl, false, SaveVersionList);
    }
    private void RequestZip(string version)
    {
        string httpUrl = string.Format("{0}/{1}.zip", url, version);
        SendRequest(httpUrl, true, SaveZipToLocal);
    }

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
            httpImpl.Post(httpUrl, requestParams, isOpenStram,action);
        }
    }


    private void SaveVesionToLocal(HTTPResponse response)
    {
        Debug.Log("版本号获得成功--" + response.DataAsText);
        bool isUpdate= VersionController.ReadLocalVersion(response.DataAsText);
        if (isUpdate)//需要更新
        {
            RequestVersionList();
        }
        else
        {
            if (finish != null)
                 finish();
        }
        //string savePath = PathHelp.GetDownLoadPath();
        //if (!Directory.Exists(savePath))
        //{
        //    Directory.CreateDirectory(savePath);
        //}
        //string version_n = response.DataAsText;
        //string version_Path = Path.Combine(savePath, version);
        //if (File.Exists(version_Path))
        //{
        //    string version_o = File.ReadAllText(version_Path);
        //    if (Convert.ToInt32(version_n) > Convert.ToInt32(version_o))
        //    {
        //        File.Delete(version_Path);
        //        SaveText(version_Path, version_n);
        //        RequestZip();
        //    }
        //    else
        //    {
        //        if (finish != null)
        //            finish();
        //    }
        //}
        //else
        //{
        //    SaveText(version_Path, version_n);
        //    RequestZip();
        //}
    }

    private void SaveVersionList(HTTPResponse response)
    {
        int nextIndex= VersionController.ReadVersionList(response.DataAsText,out downSize);
        for (int i = nextIndex; i < VersionController.vsList.Count; i++)//
        {
            RequestZip(VersionController.vsList[i].Content);
        }
    }


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
            }
            StartCoroutine(Zip.UnZip(newPath, PathHelp.GetDownLoadPath()+PathHelp.unZip, finish));
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
            if (downZip != null)
                downZip(progress);
        }
    }
    private void SaveText(string textPath, string text)
    {
        byte[] bys = Encoding.UTF8.GetBytes(text);
        FileStream stream = File.Create(textPath);
        stream.Write(bys, 0, bys.Length);
        stream.Close();
        stream.Dispose();
    }
}
