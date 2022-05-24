using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;
using TMPro;


[System.Serializable]
public class ObjectArray
{
    public Sprite[] Behind = new Sprite[2];
}

public class AttendanceCheck : MonoBehaviour
{
    #region �ν����� �� ���� ����

    public ObjectArray[] Front = new ObjectArray[7];
    public Image[] image = new Image[7];
    public Text[] text = new Text[7];
    public TextMeshProUGUI text_mesh;
    public GameObject icon;
    public GameObject Tag;

    public bool isAtd;

    private int days;
    private int hearts;
    String weeks_text;




    #endregion

    #region �⼮ ���� ���

    public void Attendance()
    {
        DateTime today = DataController.instance.gameData.Today;
        DateTime lastday = DataController.instance.gameData.Lastday; //���� ��¥ �޾ƿ���
        bool isAttendance = DataController.instance.gameData.attendance; //�⼮ ���� �Ǵ� bool ��
        days = DataController.instance.gameData.days; // �⼮ ���� ��¥.
        int weeks; //���� ����


/*        //�׽�Ʈ��
        lastday = DateTime.Parse("2022-04-28");
        days = 5;
        isAttendance = false;*/


        TimeSpan ts = today - lastday; //��¥ ���� ���
        int DaysCompare = ts.Days; //Days ������ ����.


        if (days > 6)
        {
            weeks = 1+(days / 7);
            days %= 7;

            if (weeks > 9)
            {
                weeks = 9;
            }
            weeks_text = weeks.ToString();
            text_mesh.text = weeks_text;

            for (int i = 0; i < text.Length; i++)
            {
                text[i].text = weeks_text;
            }
            Tag.SetActive(true);
        }
        else 
        {
            weeks = 1;
            weeks_text = weeks.ToString();
            for (int i = 0; i < text.Length; i++)
            {
                text[i].text = weeks_text;
            }
        }


        if (isAttendance == false)
        {
            icon.SetActive(true);

            if (DaysCompare == 1) //���� ��¥�� ������ �⼮ ��¥���� �Ϸ� �̷���
            {
                selectDay(days);
            }
            else if (DateTime.Compare(today, lastday) < 0) //������ ������ ���� ������, ���� ������
            {
                DataController.instance.gameData.days = 0;
                selectDay(0);
            }
            else //�����⼮�� �ƴѰ��
            {
                //days 1���� �ʱ�ȭ �� day1��ư Ȱ��ȭ, week 1�� ����.
                DataController.instance.gameData.days = 0;
                selectDay(0);
            }
        }
        else //�⼮�� �̹� �� ���´�
        {
            for (int i = 0; i < days; i++) //�⼮�Ϸ� ��ư Ȱ��ȭ
            {
                image[i].sprite = Front[i].Behind[1];
            }
        }
    }

    #endregion

    #region ��¥ ����

    public void selectDay(int day)
    {
        if (day != 0)
        {
            for (int i = 0; i < day; i++)
            {
                image[i].sprite = Front[i].Behind[1];
            }
        }
        image[day].sprite = Front[day].Behind[0];
    }

    #endregion

    #region �⼮ ���� ����

    public void AttandanceSave(int number)
    {
        
        if (number == days&& DataController.instance.gameData.attendance==false)
        {
            AudioManager.instance.RewardAudioPlay();
            image[number].sprite = Front[number].Behind[1];
            icon.SetActive(false);

            //�⼮ ���� ����.
            DataController.instance.gameData.days += 1;
            DataController.instance.gameData.attendance = true;
            DataController.instance.gameData.Lastday = DataController.instance.gameData.Today;

            DataController.instance.gameData.accAttendance += 1; // ���� �⼮ ����
                                                                 // 10*��¥*�ּ�
                                                                 // Debug.Log("���� �⼮ : " + DataController.instance.gameData.accAttendance);
                                                                 // Debug.Log("���� ��Ʈ : " + DataController.instance.gameData.accHeart);

            hearts = number;
            Invoke("AtdHeart", 0.75f);
        }
    }

    public void AtdHeart()
    {
        GameManager.instance.GetHeart(10 * (hearts + 1));
    }

    public static implicit operator AttendanceCheck(GameManager v)
    {
        throw new NotImplementedException();
    }
}
    #endregion


