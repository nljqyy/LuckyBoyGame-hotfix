using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using System;
using XLua;

[Hotfix]
public class Anim : MonoBehaviour, IMsgEvent
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform tran;
    [SerializeField]
    private Button btton;
    [SerializeField]
    private Image bg2;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private Transform leftZha;
    [SerializeField]
    private Transform rightZha;
    [SerializeField]
    private Transform bom;
    [SerializeField]
    private Transform zidan;
    [SerializeField]
    private Transform police_shoot;
    [SerializeField]
    private Image police_img;
    [SerializeField]
    private Text sayText;
    private bool isStartShoot = false;
    private int shootNum = 0;
    private int num = 0;
    private float space_time = 0.1f;
    private Vector3 vel;
    private CanvasGroup canvasgroup;
    private Tweener catchMoveTween;
    private Tweener upTween;
    private Tweener downTween;
    private Tweener leftTween;
    private Tweener rightTween;
    private Tweener policeTween1;
    private Tweener policeTween2;
    private Dictionary<BullteType, List<BulletBomEffect>> listZidan = new Dictionary<BullteType, List<BulletBomEffect>>();
    private List<ExcelTableEntity> elist;

    private void Start()
    {
        Init();
        SetPoliceImg(false);
        SetBgStart();
        GetTween();
        PoliceMove();
        this.RegisterMsgEvent(
                new MsgHandler(1, EventHandlerType.HeadPress, HeadPress),
                new MsgHandler(1, EventHandlerType.RestStart, SetBg)
          );
        //StartCoroutine(Shoot());
    }
    private void OnEnable()
    {
        StartCoroutine(Shoot());
    }
    private void Init()
    {
        vel = transform.position;
        elist = SDKManager.Instance.GetVoiceForType(VoiceType.Police);
        btton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayByName(AudioType.Fixed, AudioNams.downing2, false);
            EventHandler.ExcuteMsgEvent(EventHandlerType.HeadPress, null);
            Debug.Log("开始抓");
        });
    }

    #region 背景1切换
    //-------------------背景1切换---------------------------

    private void SetBgStart()
    {
        bg2.sprite = GetRandomSp(null);
        canvasgroup = bg2.GetComponent<CanvasGroup>();
    }

    private void SetBg(object[] o)
    {
        num++;
        PoliceMove();
        catchMoveTween.timeScale = 1;
        canvasgroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            bg2.sprite = GetRandomSp(bg2.sprite);
            canvasgroup.DOFade(1, 1);
        });
    }

    private Sprite GetRandomSp(Sprite sp)
    {
        if (num < sprites.Length)
            return sprites[num];
        return null;
    }

    #endregion


    #region 最新动画播放

    //头部按下
    private void HeadPress(object[] data)
    {
        Debug.Log("头部按下开始抓");
        StartCatch();
    }

    private void GetTween()
    {
        catchMoveTween = transform.DOLocalMoveX(181, 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).Pause().SetAutoKill(false);
        downTween = transform.DOLocalMoveY(-85, 2.5f).SetEase(Ease.Linear).Pause().SetAutoKill(false);
        upTween = transform.DOLocalMoveY(366, 3.6f).SetEase(Ease.Linear).Pause().SetAutoKill(false);
        leftTween = leftZha.DOLocalRotate(new Vector3(0, 0, 15), 0.2f).Pause().SetAutoKill(false);
        rightTween = rightZha.DOLocalRotate(new Vector3(0, 0, -15), 0.2f).Pause().SetAutoKill(false);
        policeTween1 = police_shoot.DOLocalMoveX(-220, 3).Pause().SetEase(Ease.Linear).SetAutoKill(false);
        policeTween2 = police_shoot.DOLocalMoveX(-511, 3).Pause().SetEase(Ease.Linear).SetAutoKill(false);
    }
    //爪子开始抓
    private void StartCatch()
    {
        catchMoveTween.timeScale = 0;
        downTween.Restart();
        downTween.Play().OnComplete(() =>
        {
            EventHandler.ExcuteMsgEvent(EventHandlerType.FishHookCheck, tran);//执行检测
            if (SDKManager.Instance.gameXP != null)
            {
                StartShoot(SDKManager.Instance.gameXP.catchty == CatchTy.Catch);
                StartCoroutine(SDKManager.Instance.TimeFun(1.5f, 1.5f, (ref float t) =>
                 {
                     if (isStartShoot)
                         AudioManager.Instance.PlayByName(AudioType.New, AudioNams.help, false);//救命语音
                     if (t == 0) t = 1.5f;
                     return !isStartShoot;
                 }));
            }
            leftTween.PlayForward();
            rightTween.PlayForward();
            rightTween.OnComplete(() =>
            {
                upTween.Restart();
                upTween.Play().OnComplete(() =>
                {
                    leftTween.PlayBackwards();
                    rightTween.PlayBackwards();
                    ResetValue();
                    EventHandler.ExcuteMsgEvent(EventHandlerType.UpFinish, null);//上升完成
                });
            });
        });
    }
    //警察移动
    private void PoliceMove()
    {
        catchMoveTween.Play();
        police_shoot.localRotation = Quaternion.identity;
        sayText.transform.localRotation = Quaternion.identity;
        PoliceSay();
        SetPoliceImg(false);
        policeTween1.Restart();
        policeTween1.Play().OnComplete(() =>
        {
            police_shoot.localRotation = Quaternion.identity;
            police_shoot.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            sayText.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            policeTween2.Restart();
            policeTween2.Play().OnComplete(() => PoliceMove());
        });
    }
    //射击
    private IEnumerator Shoot()
    {
        while (true)
        {
            if (isStartShoot)
            {
                AudioManager.Instance.PlayByName(AudioType.Continuous, AudioNams.shoot, false);
                BulletBomEffect zd = GetBullteBom(BullteType.Bullte);
                zd.Show();
                float time = Mathf.Abs((1400 - zd.bullet.transform.localPosition.x) * 0.4f / 1400);
                zd.bullet.transform.DOLocalMoveX(1400, time).OnComplete(() => zd.Hide());
            }
            yield return new WaitForSeconds(space_time);
        }
    }
    //开始射击
    private void StartShoot(bool isCatch)
    {
        Debug.Log("开始射击");
        SetPoliceImg(true);
        if (policeTween2.IsPlaying())
        {
            policeTween2.Pause();
            police_shoot.localRotation = Quaternion.identity;
            sayText.transform.localRotation = Quaternion.identity;
        }
        policeTween1.Pause();
        isStartShoot = true;
        PoliceSay();
        space_time = isCatch ? 0.33f : 0.1f;
    }
    //停止射击
    private void StopShoot()
    {
        isStartShoot = false;
        space_time = 0.1f;
        AudioManager.Instance.StopPlayAds(AudioType.New);
        AudioManager.Instance.StopPlayAds(AudioType.Continuous);
    }
    //获得没使用的子弹 炸弹
    private BulletBomEffect GetBullteBom(BullteType bt)
    {
        List<BulletBomEffect> tplist;
        BulletBomEffect zd;
        if (listZidan.ContainsKey(bt))
            tplist = listZidan[bt];
        else
        {
            tplist = new List<BulletBomEffect>();
            listZidan.Add(bt, tplist);
        }
        zd = tplist.FirstOrDefault(e => !e.isShoot);
        if (zd == null)
        {
            Transform md = null;
            Vector3 scal = Vector3.zero;
            if (bt == BullteType.Bullte)
            {
                scal = Vector3.one * 1.5f;
                md = zidan;
            }
            else
            {
                scal = Vector3.one * 2;
                md = bom;
            }
            GameObject go = CommTool.InstantiateObj(md.gameObject, md.parent.gameObject, Vector3.zero, scal, tplist.Count.ToString());
            zd = new BulletBomEffect(go);
            tplist.Add(zd);
            listZidan[bt] = tplist;
        }
        return zd;
    }
    //警察说话
    private void PoliceSay(bool isDorp = false)
    {
        sayText.transform.parent.gameObject.SetActive(true);
        string strs = "";
        if (isDorp)
            strs = elist[0].WinningContent;
        else
        {
            strs = !isStartShoot ? elist[0].TimeContent : elist[1].TimeContent;
            if (!isStartShoot)
            {
                int number = UnityEngine.Random.Range(0, 11);
                if (number > 6)
                    sayText.transform.parent.gameObject.SetActive(false);//隐藏
            }
        }
        string[] sts = strs.Split('|');
        sayText.text = sts[UnityEngine.Random.Range(0, sts.Length)];
    }
    //设置警察图片
    private void SetPoliceImg(bool isShoot)
    {
        string imgName = "";
        if (isShoot)
        {
            imgName = "police_shoot";
            police_img.transform.localPosition = new Vector3(53, 106, 0);
        }
        else
        {
            imgName = "police_idel";
            police_img.transform.localPosition = new Vector3(0, 116, 0);
        }
        police_img.sprite = UIAtlasManager.LoadSprite(UIAtlasName.UIMain, imgName);
        police_img.SetNativeSize();
    }
    //设置自动 爆炸特效为默认值
    private void SetDefulatValue()
    {
        foreach (var item in listZidan.Values)
        {
            foreach (var it in item)
            {
                it.Hide();
            }
        }
    }
    //上升完成重置数据
    private void ResetValue()
    {
        shootNum = 0;
        transform.position = vel;
        SDKManager.Instance.gameXP = null;
        policeTween1.Pause();
        policeTween2.Pause();
        StopShoot();
        SetDefulatValue();
    }

    //检测碰撞
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform)
        {
            int index = Convert.ToInt32(collision.gameObject.name);
            listZidan[BullteType.Bullte][Convert.ToInt32(index)].Hide();
            BulletBomEffect boe = GetBullteBom(BullteType.BomEff);
            boe.StartPlay();
            transform.DOMoveX(transform.position.x + 0.3f, 0.2f).SetLoops(2, LoopType.Yoyo);
            if (upTween.IsPlaying()) shootNum++;
            if (SDKManager.Instance.gameXP != null && shootNum >= 3)
            {
                shootNum = -100;
                if (SDKManager.Instance.gameXP.catchty == CatchTy.Drop)//掉落
                {
                    StopShoot();
                    PoliceSay(true);
                    SDKManager.Instance.gameXP.DropIe();
                }
            }
        }
    }
    #endregion

    private void OnDestroy()
    {
        this.UnRegisterMsgEvent();
    }

}
