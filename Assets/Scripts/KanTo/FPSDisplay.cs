using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour {

    [SerializeField] private int m_FrameCount;
    [SerializeField] private float m_PrevTime;
    [SerializeField] private float m_FPS;

    [SerializeField] private Text m_Text;

    private void Start()
    {
        m_FrameCount = 0;
        m_PrevTime = 0.0f;
    }

    private void Update()
    {
        m_FrameCount++;
        float m_Time = Time.realtimeSinceStartup - m_PrevTime;

        if (m_Time >= 2.0f) {
            m_FPS = m_FrameCount / m_Time;

            m_FrameCount = 0;
            m_PrevTime = Time.realtimeSinceStartup;
        }

        m_Text.text = m_FPS.ToString("N0");
    }
}
