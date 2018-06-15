using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

[Hotfix]
public sealed class CustomAudioSource : MonoBehaviour
{
    private IEnumerator ie = null;
    private AudioSource continuousAds = null;
    private AudioSource newAds = null;
    private AudioSource fiexdAds = null;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartCheckIE());
    }

    private IEnumerator StartCheckIE()
    {
        while (true)
        {
            if (ie != null && ie.MoveNext())
                yield return ie.Current;
            else
                yield return null;
        }
    }
    public void Play(AudioType adtype, AudioSource audioSource, AudioClip clip, Transform emitter, float volume, float pitch, bool loop, Action action = null)
    {
        if (adtype == AudioType.Fixed)
            StartCoroutine(PlayI(audioSource, clip, volume, pitch, loop, action));
        else if (adtype == AudioType.Continuous)
        {
            if (ie == null)
                ie = PlayII(clip, emitter, volume, pitch, loop, () => ie = null);
        }
        else
        {
            PlayIII(clip, emitter, volume, pitch, loop);
        }
    }


    private IEnumerator PlayI(AudioSource audioSource, AudioClip clip, float volume, float pitch, bool loop, Action action = null)
    {
        fiexdAds = fiexdAds ?? audioSource;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length);
        if (action != null)
            action();
    }
    private IEnumerator PlayII(AudioClip clip, Transform emitter, float volume, float pitch, bool loop, Action action = null)
    {
        GameObject go = new GameObject("Audio:" + clip.name);
        go.transform.SetParent(emitter);
        go.transform.localPosition = Vector3.zero;
        continuousAds = go.AddComponent<AudioSource>();
        continuousAds.clip = clip;
        continuousAds.volume = volume;
        continuousAds.pitch = pitch;
        continuousAds.loop = loop;
        continuousAds.Play();
        if (!loop)
        {
            DestroyObject(go, clip.length);
            yield return new WaitForSeconds(clip.length);
            if (action != null)
                action();
        }

    }
    private void PlayIII(AudioClip clip, Transform emitter, float volume, float pitch, bool loop)
    {
        GameObject go = new GameObject("Audio:" + clip.name);
        go.transform.SetParent(emitter);
        go.transform.localPosition = Vector3.zero;
        newAds = go.AddComponent<AudioSource>();
        newAds.clip = clip;
        newAds.volume = volume;
        newAds.pitch = pitch;
        newAds.loop = loop;
        newAds.Play();
        if (!loop)
        {
            DestroyObject(go, clip.length);
        }
    }

    public void StopAudio(AudioType adtype)
    {
        switch (adtype)
        {
            case AudioType.Continuous:
                ie = null;
                if (continuousAds != null) continuousAds.volume = 0;
                break;
            case AudioType.Fixed:
                if (fiexdAds != null) fiexdAds.volume = 0;
                break;
            case AudioType.New:
                if (newAds != null) newAds.volume = 0;
                break;
        }
    }

}
