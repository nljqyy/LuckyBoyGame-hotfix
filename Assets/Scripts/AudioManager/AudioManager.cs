using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Video;

public sealed class AudioManager:Singleton<AudioManager>
{
    private string mp3 = ".mp3";
    private Dictionary<string, AudioClip> soundDic = new Dictionary<string, AudioClip>();
    private AudioSource _source;
    private CustomAudioSource _cas;
    private GameObject emitter;

    protected override void StartUp()
    {
        GameObject audioSource = new GameObject("Audios");
        _source = audioSource.AddComponent<AudioSource>();
        _cas = audioSource.AddComponent<CustomAudioSource>();
        emitter = new GameObject("audios---");
        emitter.transform.SetParent(audioSource.transform);
    }
    public void PlayByName(AudioType adtype, AudioNams clipName, bool loop, Action acion = null)
    {
        AudioClip clip = FindAudioClip(clipName.ToString());
        Debug.Log("播放声音---" + clipName);
        if (adtype == AudioType.Fixed)
        {
            _cas.Play(adtype, _source, clip, null, 1, 1, loop, acion);
        }
        else
        {
            _cas.Play(adtype, null, clip, emitter.transform, 1, 1, loop, acion);
        }
    }
    
    public void StopPlayAds(AudioType adtype)
    {
        Debug.Log("停止音效---" + adtype);
        _cas.StopAudio(adtype);
    }

    private AudioClip FindAudioClip(string clipName)
    {
        AudioClip clip;
        soundDic.TryGetValue(clipName, out clip);
        if (clip == null)
        {
            Bundle bd= LoadAssetMrg.Instance.LoadAsset(clipName+mp3);
            clip = bd.mAsset as AudioClip;
            soundDic.Add(clipName, clip);
            bd = null;
        }
        return clip;
    }
    public void Clear()
    {
        soundDic.Clear();
        base.Dispose();
    }

}
