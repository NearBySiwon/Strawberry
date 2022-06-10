using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Setting : MonoBehaviour
{
    #region
    public Text versionDate_text;
    public Text version_text;
    public Text saveDate_text;
    public Button cloudLoad_btn;
    #endregion

    void Awake()
    {
        SetVersionInfo();
    }

    void Start()
    {
        SetCloudSave();
        //�α��� �Ǿ�� Ŭ���� ���̺� �����ؼ� �ּ��س��ڽ��ϴ�
        //GPGSManager.instance.OnSaveSucceed += SetCloudSave;
    }

    public void SetCloudSave()
    {
        if (DataController.instance.gameData.cloudSaveTime == System.DateTime.MinValue)
        {
            cloudLoad_btn.interactable = false;
            saveDate_text.text = "������ ����";
        }
        else
        {
            cloudLoad_btn.interactable = true;
            saveDate_text.text = "������ ���� ��¥\n"+DataController.instance.gameData.cloudSaveTime.ToString("yyyy�� MM�� dd�� HH:mm:ss");
        }
    }

    private void SetVersionInfo() // ���� ���� ����
    {
        versionDate_text.text = "22-05-09";
        version_text.text = "V 1.0.1";
    }

    public void PrivacyPolicy() // ��������ó����ħ
    {
        Application.OpenURL("https://woos-workspace.notion.site/e9c66b72d3ef4e5082909afd2f5cf0a7");
    }

    public void ContactByEmail() // �̸��Ϸ� �����ϱ�
    {
        string mailto = "teamfarmer.ttalgi@gmail.com";
        string subject = EscapeURL("[���޴��� �������] ���� / ����");
        string body = EscapeURL
            (
             "\n\n\n\n\n" +
             "__________\n" +
             "Device Model : " + SystemInfo.deviceModel + "\n\n" +
             "Device OS : " + SystemInfo.operatingSystem + "\n\n"
            );

        Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body);
    }

    private string EscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }

    public void Instagram() // ���� �ν�Ÿ ����
    {
        Application.OpenURL("https://www.instagram.com/team_farmer_/");
    }

    //public void DevCredit() // ������ ũ����
    //{
    //    Debug.Log(" �� �ĸ� ! ");
    //}
}
