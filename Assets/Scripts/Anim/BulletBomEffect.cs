using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
[Hotfix]
public sealed class BulletBomEffect {

    public bool isShoot;
    public GameObject bullet;
    public BulletBomEffect(GameObject bullet)
    {
        this.bullet = bullet;
    }
    public void StartPlay()
    {
        Show();
        SDKManager.Instance.StartCoroutine(SDKManager.Instance.TimeFun(0.5f,0.5f,null, Hide));
    }
    public void Hide()
    {
        isShoot = false;
        bullet.SetActive(false);
        bullet.transform.localPosition = Vector3.zero;
    }
    public void Show()
    {
        isShoot = true;
        bullet.SetActive(true);
    }
}



