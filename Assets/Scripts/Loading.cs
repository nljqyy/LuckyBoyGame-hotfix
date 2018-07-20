using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XLua;
using System;
[Hotfix]
public class Loading : MonoBehaviour {

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Text downProcess;
    [SerializeField]
    private GameObject mask;
    private void Awake()
    {
        mask.SetActive(false);
        slider.gameObject.SetActive(false);
        NetMrg.Instance.RequestVersion(DownZip,EnterGame);
         
    }
    private void DownZip(float process)
    {
        if(!mask.activeSelf)
            mask.SetActive(true);
        if (!slider.gameObject.activeSelf)
            slider.gameObject.SetActive(true);
        slider.value = process;
        downProcess.text =System.Math.Floor(process * 100).ToString();
    }
    private void EnterGame()
    {
        GC.Collect();
        LoadAssetMrg.Instance.LoadAssetAsync("main.unity",ab=> SceneManager.LoadSceneAsync("Main"));
    }
}
