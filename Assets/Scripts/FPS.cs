using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    private float m_LastUpdateShowTime = 0f;    //上一次更新帧率的时间;  

    private float m_UpdateShowDeltaTime = 0.01f;//更新帧率的时间间隔;  

    private int m_FrameUpdate = 0;//帧数;  

    private float m_FPS = 0;

    public Text text;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Use this for initialization  
    void Start()
    {
        m_LastUpdateShowTime = Time.realtimeSinceStartup;
        StartCoroutine(FPTT());
    }

    // Update is called once per frame  
    void Update()
    {
        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }
        
    }

    IEnumerator FPTT()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            text.text = m_FPS.ToString();
        }
    }

}