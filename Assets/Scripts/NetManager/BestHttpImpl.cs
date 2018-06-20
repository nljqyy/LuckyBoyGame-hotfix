using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using UnityEngine;

public sealed class BestHttpImpl
{
    //头部数据
    private Dictionary<string, string> headers;
    public BestHttpImpl()
    {
        headers = new Dictionary<string, string>();
    }
    public void AddHead(string name, string value)
    {
        headers.Add(name, value);
    }
    public void SetHttpParams(bool isCache = true, int connectTimeount = 10, int requestTimeount = 30)
    {
        //是否缓存响应
        HTTPManager.IsCachingDisabled = isCache;
        //默认10
        HTTPManager.ConnectTimeout = TimeSpan.FromSeconds(connectTimeount);
        //默认30
        HTTPManager.RequestTimeout = TimeSpan.FromSeconds(requestTimeount);
    }

    private void HandleResponse(HTTPRequest request, HTTPResponse response, Action<HTTPResponse> callback = null)
    {
        if (callback != null)
        {
            string status = "";
            switch (request.State)
            {
                case HTTPRequestStates.Processing:
                    //if (!PlayerPrefs.HasKey("DownloadLength"))
                    //{
                    //    string value = response.GetFirstHeaderValue("content-length");
                    //    if (!string.IsNullOrEmpty(value))
                    //        PlayerPrefs.SetInt("DownloadLength", int.Parse(value));
                    //}
                    if (callback != null)
                        callback(response);
                    break;
                case HTTPRequestStates.Finished:
                    if (response.IsSuccess)
                    {
                        if (callback != null)
                            callback(response);
                    }
                    else
                    {
                        status = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                        request.Response.StatusCode,
                                                        request.Response.Message,
                                                        request.Response.DataAsText);
                        Debug.LogWarning(status);
                    }
                    break;
                case HTTPRequestStates.Aborted:
                    status = "Request Aborted!";
                    Debug.LogWarning(status);
                    break;
                case HTTPRequestStates.Error:
                    status = "Request Finished with Error! " + (request.Exception != null ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No Exception");
                    Debug.LogError(status);
                    break;
                case HTTPRequestStates.TimedOut:
                    status = "Processing the request Timed Out!";
                    Debug.LogError(status);
                    break;
                case HTTPRequestStates.ConnectionTimedOut:
                    status = "Connection Timed Out!";
                    Debug.LogError(status);
                    break;
            }
        }

    }

    private void AddHeads(HTTPRequest request)
    {
        if (headers != null)
        {
            foreach (var item in headers)
            {
                request.SetHeader(item.Key, item.Value);
            }
        }
    }
    private void AddParams(HTTPRequest request, Dictionary<string, string> requestParams)
    {
        if (requestParams != null && requestParams.Count > 0)
        {
            foreach (var item in requestParams)
            {
                request.AddField(item.Key, item.Value);
            }
        }
    }

    public void Get(string url, Action<HTTPResponse> callback=null)
    {
        HTTPRequest request = RequestCreate(new Uri(url), HTTPMethods.Get, (HTTPRequest requestFinish, HTTPResponse response) =>
         {
             HandleResponse(requestFinish, response, callback);
         });
        AddHeads(request);
        request.Send();
    }

    public void Post(string url, Dictionary<string, string> requestParams, bool isOpenStream = false,  Action<HTTPResponse> callback = null)
    {
        HTTPRequest request = RequestCreate(new Uri(url), HTTPMethods.Post, (HTTPRequest requestFinish, HTTPResponse response) =>
        {
            HandleResponse(requestFinish, response, callback);
        });
        if (isOpenStream)
        {
            request.UseStreaming = true;
            request.StreamFragmentSize = HTTPResponse.MinBufferSize;
        }
        AddHeads(request);
        AddParams(request, requestParams);
        request.Send();
    }

    private HTTPRequest RequestCreate(Uri uri, HTTPMethods method, OnRequestFinishedDelegate callback)
    {
        HTTPRequest request = new HTTPRequest(
            uri: uri,
            methodType: method,
            isKeepAlive: true,
            disableCache: true,
            callback: callback
            );
        return request;
    }

}
