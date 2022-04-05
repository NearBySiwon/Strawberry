using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;


[System.Serializable]
public class ObjectArray
{
    public Sprite[] Behind = new Sprite[2];
}

public class AttendanceCheck : MonoBehaviour
{
    #region �ν�����

    public ObjectArray[] Front = new ObjectArray[7];
    public Image[] image = new Image[7];
    public GameObject icon;
    int count;
    int days;

    #endregion

    private void Start()
    {
        count = 0;
        
    }

    #region �⼮ ���� ���

    public void Attendance()
    {
        DateTime today = DataController.instance.gameData.Today;
        DateTime lastday = DataController.instance.gameData.Lastday; //���� ��¥ �޾ƿ���
        bool isAttendance = DataController.instance.gameData.attendance; //�⼮ ���� �Ǵ� bool ��
        days = DataController.instance.gameData.days; // �⼮ ���� ��¥.
        int weeks; //���� ����

        //�׽�Ʈ��
        //lastday = DateTime.Parse("2022-04-05");
        //days = 6;
        //isAttendance = false;
  

        TimeSpan ts = today - lastday; //��¥ ���� ���
        int DaysCompare = ts.Days; //Days ������ ����.

        if (isAttendance == false)
        {
            icon.SetActive(true);
            if (days > 7)
            {
                weeks = days % 7;
                switch (weeks)
                {
                    //�������� Week �ؽ�Ʈ ���� ���� �߰�����.
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }
            else if (days == 0)
            {
                weeks = days;
            }
            else
            {
                weeks = days;
            }

            if (DaysCompare == 1) //���� ��¥�� ������ �⼮ ��¥���� �Ϸ� �̷���
            {
                selectDay(weeks);
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
            if (days > 7)
            {
                weeks = days % 7;
                switch (weeks)
                {
                    //�������� Week �ؽ�Ʈ ���� ���� �߰�����.
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }
            else
            {
                weeks = days;
            }
            for (int i = 0; i < weeks; i++) //�⼮�Ϸ� ��ư Ȱ��ȭ
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
        count = day;
    }

    #endregion

    #region �⼮ ���� ����

    public void AttandanceSave(int number)
    {
        
        if (number == days)
        {
            image[number].sprite = Front[number].Behind[1];
            icon.SetActive(false);

            //�⼮ ���� ����.
            DataController.instance.gameData.days += 1;
            DataController.instance.gameData.attendance = true;
            DataController.instance.gameData.Lastday = DataController.instance.gameData.Today;

            DataController.instance.gameData.accAttendance += 1; // ���� �⼮ ����
            GameManager.instance.GetHeart(10 * (number + 1)); // 10*��¥*�ּ�
                                                              // Debug.Log("���� �⼮ : " + DataController.instance.gameData.accAttendance);
                                                              // Debug.Log("���� ��Ʈ : " + DataController.instance.gameData.accHeart);
        }
    }

    public static implicit operator AttendanceCheck(GameManager v)
    {
        throw new NotImplementedException();
    }
}
    #endregion
