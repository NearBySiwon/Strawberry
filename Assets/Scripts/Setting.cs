using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    #region
    public Text versionDate_text;
    public Text version_text;
    #endregion

    void Awake()
    {
        SetVersionInfo();
    }

    private void SetVersionInfo() // ���� ���� ����
    {
        versionDate_text.text = "22.04.04";
        version_text.text = "V 1.0.1";
    }

    public void PrivacyPolicy() // ��������ó����ħ
    {
        Application.OpenURL("https://www.google.com/search?q=%EA%B0%9C%EC%9D%B8%EC%A0%95%EB%B3%B4%EC%B2%98%EB%A6%AC%EB%B0%A9%EC%B9%A8+%EC%98%88%EC%8B%9C&oq=%EA%B0%9C%EC%9D%B8%EC%A0%95%EB%B3%B4%EC%B2%98%EB%A6%AC%EB%B0%A9%EC%B9%A8&aqs=chrome.4.69i57j69i59j0i20i263i512j0i512l7.7768j0j4&sourceid=chrome&ie=UTF-8");
        Debug.Log("���߿� �߰� ����");
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
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void Instagram() // ���� �ν�Ÿ ����
    {
        Application.OpenURL("https://www.instagram.com/team_farmer_/");
    }

    public void DevCredit() // ������ ũ����
    {
        Debug.Log(" �� �ĸ� ! ");
    }
}
