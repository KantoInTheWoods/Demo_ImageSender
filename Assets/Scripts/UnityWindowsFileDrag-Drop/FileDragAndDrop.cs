using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
using B83.Win32;
using Shibuya24.Utility;
using UnityEngine.UI;
using System.IO;

public class FileDragAndDrop : MonoBehaviour
{
    [SerializeField] private ImageManager m_ImageManager;

    [SerializeField] private bool m_Dropped;

    private void OnEnable ()
    {
        m_Dropped = false;

#if UNITY_STANDALONE_OSX

        UniDragAndDrop.onDragAndDropFilePath = x => m_ImageManager.m_Path = x;
        UniDragAndDrop.Initialize();

#elif UNITY_STANDALONE_WIN
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;
#endif

    }

    private void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }

#if UNITY_STANDALONE_OSX
    private void Update()
    {
        if(m_ImageManager.m_Path == m_ImageManager.m_LastPath || m_Dropped == true) return;

        m_Dropped = true;

        bool m_VallidFormats = m_ImageManager.GetTheImageNameFromFolderPath();

        if(!m_VallidFormats)
        {
            m_Dropped = false;
            return;
        }

        StartCoroutine(m_ImageManager.GetTexture());

        m_Dropped = false;
    }
#endif

    private void OnFiles(List<string> aFiles, POINT aPos)
    {
        string m_FilePath = aFiles[0];
        m_ImageManager.m_Path = m_FilePath;

        bool m_VallidFormats = m_ImageManager.GetTheImageNameFromFolderPath();

        if(!m_VallidFormats) return;

        StartCoroutine(m_ImageManager.GetTexture());
    }
}
