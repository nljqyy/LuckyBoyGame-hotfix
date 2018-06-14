using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RawImage))]
public class UI3DEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;
    private GameObject effectGO;
    private RenderTexture renderTexture;
    private Camera rtCamera;
    private RawImage rawImage;
    private ParticleSystem ps;

    void Awake()
    {
        //RawImage可以手动添加，设置no alpha材质，以显示带透明的粒子
        rawImage = gameObject.GetComponent<RawImage>();
        if (rawImage == null)
        {
            rawImage = gameObject.AddComponent<RawImage>();
        }
    }

    public RectTransform rectTransform
    {
        get
        {
            return transform as RectTransform;
        }
    }

    void OnEnable()
    {
        if (effectPrefab != null)
        {
            effectGO = Instantiate(effectPrefab);
           // ps = effectGO.GetComponent<ParticleSystem>();
            GameObject cameraObj = new GameObject("UIEffectCamera");
            rtCamera = cameraObj.AddComponent<Camera>();
            renderTexture = new RenderTexture((int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y, 24);
            renderTexture.antiAliasing = 4;
            rtCamera.clearFlags = CameraClearFlags.SolidColor;
            rtCamera.backgroundColor = new Color();
            rtCamera.cullingMask = 1 << 8;
            rtCamera.targetTexture = renderTexture;

            effectGO.transform.SetParent(cameraObj.transform, false);

            rawImage.enabled = true;
            rawImage.texture = renderTexture;
        }
        else
        {
            rawImage.texture = null;
            Debug.LogError("EffectPrefab can't be null");
        }
    }

    void OnDisable()
    {
        if (effectGO != null)
        {
            Destroy(effectGO);
            effectGO = null;
        }
        if (rtCamera != null)
        {
            Destroy(rtCamera.gameObject);
            rtCamera = null;
        }
        if (renderTexture != null)
        {
            Destroy(renderTexture);
            renderTexture = null;
        }
        rawImage.enabled = false;
    }


    //IEnumerator Play(Transform tran)
    //{
    //    int num = 5;
    //    while (num > 0)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        if (ps.isStopped)
    //        {
    //            ps.Play();
    //            num--;
    //        }
    //    }
    //    while (!ps.isStopped)
    //    {
    //        yield return null;
    //    }
    //    gameObject.SetActive(false);
    //}

}