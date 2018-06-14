using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DDebug : MonoBehaviour
{
    public Button close;
    public Button open;
    public Button checkUp;
    public Button checkDown;
    public Button clearSave;
    public Button openPay;
    public GameObject obj;
    // Use this for initialization
    void Start()
    {
        close.onClick.AddListener(() => obj.SetActive(false));
        open.onClick.AddListener(() => obj.SetActive(true));
        checkDown.onClick.AddListener(() =>
        {
            SDKManager.Instance.checkProperty -= 0.2f;
        });
        checkUp.onClick.AddListener(() =>
        {
            SDKManager.Instance.checkProperty += 0.2f;
        });
        clearSave.onClick.AddListener(() => CommTool.ClearSaveData());
        openPay.onClick.AddListener(() => SDKManager.Instance.isOpenPay = !SDKManager.Instance.isOpenPay);
    }
   
}
