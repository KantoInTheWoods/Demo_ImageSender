using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class ImageManager : MonoBehaviour
{
    [SerializeField] public string m_Path;
    [SerializeField] public string m_LastPath;
    [SerializeField] private string m_ImageFile;
    [SerializeField] private string m_OAImageFileExtension;

    [SerializeField] private int m_MainTakeCount = 0;

    [SerializeField] private bool m_MainTakeBool = false;

    [SerializeField] private RawImage m_OAImage;

    [SerializeField] private Image m_OATallyImage;
    [SerializeField] private Image m_SpaceImage;

    [SerializeField] private Button m_MainTakeButton;

    [SerializeField] private Text m_OAFileName;
    [SerializeField] private Text[] m_ControlStatus;

    [SerializeField] private Camera m_TakeCamera;


    private void Update () 
    {
        if (Input.GetKeyDown (KeyCode.Space))
        {
            MainTAKE();
        }

        if (Input.GetKey (KeyCode.Space)) 
        {
            ChangeTheImageColor(m_SpaceImage, new Color(1f, 0f, 0f, 1f));
        }        
        else
        {
            ChangeTheImageColor(m_SpaceImage, new Color(1f, 1f, 1f, 1f));
        }
    }

    public IEnumerator GetTexture()
    {
#if UNITY_STANDALONE_OSX
        if(m_Path.Substring(0, 8) != "file:///")
        {
            m_Path = "file://" + m_Path;
        }
#endif

        Console.WriteLine(DateTime.Now + "\n" + "Request to \"" + m_Path + "\".\n");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(m_Path);
        
        Console.WriteLine(DateTime.Now + "\n" + "Requested to \"" + m_Path + "\".\n");

        yield return www.SendWebRequest();

        Console.WriteLine(DateTime.Now + "\n" + "The Request passed to \"" + m_Path + "\".\n");

        Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

        m_OAImage.texture = myTexture;

        Console.WriteLine(DateTime.Now + "\n" + "OA Image: \""  + m_ImageFile + "\"\n");

        m_LastPath = m_Path;

        ChangeTheFileName();

        Resources.UnloadUnusedAssets();
    }

    public bool GetTheImageNameFromFolderPath()
    {
        m_OAImageFileExtension = Path.GetExtension(m_Path);
        Console.WriteLine(DateTime.Now + "\n" + "File Extension: \"" + m_OAImageFileExtension + "\" was dropped.\n");

        m_ImageFile = Path.GetFileNameWithoutExtension(m_Path) + m_OAImageFileExtension;
        Console.WriteLine(DateTime.Now + "\n" + "File Name: \"" + m_ImageFile + "\" will be loaded.\n");

        if(m_OAImageFileExtension != ".png" && m_OAImageFileExtension != ".PNG")
        {
            Console.WriteLine(DateTime.Now + "\n" + "File Extension was not " + "\".png\". " + "Stop loading.\n");
            return false;
        }

        return true;
    }

    private void MainTAKE()
    {
        Console.WriteLine(DateTime.Now + "\n" + "The TAKE Button was pressed.\n");
        m_MainTakeCount ++;
        m_MainTakeBool = m_MainTakeCount % 2 == 0 ? false : true;
        ChangeTheTakeButtonColor(m_MainTakeButton);
        
        if(m_MainTakeBool == false)
        {
            m_TakeCamera.cullingMask = 0 << 10;

            Console.WriteLine(DateTime.Now + "\n" + "The OA Image is untake.\n");
            ChangeTheImageColor(m_OATallyImage, new Color(1f, 1f, 1f, 1f));
        }
        else
        {
            m_TakeCamera.cullingMask = 1 << 10;

            Console.WriteLine(DateTime.Now + "\n" + "The OA Image is take.\n");
            ChangeTheImageColor(m_OATallyImage, new Color(1f, 0f, 0f, 1f));
        }
    }

    private void ChangeTheFileName()
    {
        m_OAFileName.text = ("FileName: "+ m_ImageFile);
        
        Console.WriteLine(DateTime.Now + "\n" + "OA File Name: " + m_OAFileName.text + "\n");
    }

    private void ChangeTheTakeButtonColor(Button m_Button)
    {
        ColorBlock m_ColorBlock = m_Button.colors;

        if(!m_MainTakeBool )
        {
            m_ColorBlock.normalColor = new Color(0f, 0f, 0f, 1f);
            m_ColorBlock.highlightedColor = new Color(0.3490196f, 0.3490196f, 0.3490196f, 1f);
        }
        else
        {
            m_ColorBlock.normalColor = new Color(1.0f, 0f, 0f, 1f);
            m_ColorBlock.highlightedColor = new Color(1f, 0.3490196f, 0.3490196f, 1f);
        }

        m_Button.colors = m_ColorBlock;
    }

    private void ChangeTheImageColor(Image m_Image, Color m_ImageColor)
    {
        m_Image.color = m_ImageColor;
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        if(focusStatus == false)
        {
            Console.WriteLine(DateTime.Now + "\n" + "App Window is inactive.\n");

            for(int i = 0; i < 2; i++)
            {
                m_ControlStatus[i].text = "ー Out Of Control ー";
                m_ControlStatus[i].color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
        }
        else
        {
            Console.WriteLine(DateTime.Now + "\n" + "App Window is active.\n");

            for(int i = 0; i < 2; i++)
            {
                m_ControlStatus[i].text = "ー Under Control ー";
                m_ControlStatus[i].color = new Color(0f, 1.0f, 0.0f, 1.0f);
            }
        }
    }
}
