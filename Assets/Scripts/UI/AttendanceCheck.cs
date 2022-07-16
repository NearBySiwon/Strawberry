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
    [SerializeField] TMP_Text weekTMP;
    [SerializeField] TMP_Text[] tagTMP;
    public GameObject icon;
    public GameObject m_tag;
    public bool isAtd;
    private int days;
    private int hearts;
    private int weeks;
    private int multiply_tag;
    String weeksText;

    #endregion

    #region �⼮ ���� ���

    public void Attendance()
    {

        #region ���� �ʱ�ȭ
        //�׽�Ʈ��
        DataController.instance.gameData.attendanceLastday = DateTime.Parse("2022-07-15");
        DataController.instance.gameData.accDays = 125;
        DataController.instance.gameData.isAttendance = false;

        DateTime today = DataController.instance.gameData.attendanceToday;
        DateTime lastday = DataController.instance.gameData.attendanceLastday; //���� ��¥ �޾ƿ���
        bool isAttendance = DataController.instance.gameData.isAttendance; //�⼮ ���� �Ǵ� bool ��
        days = DataController.instance.gameData.accDays; // �⼮ ���� ��¥

        TimeSpan ts = today - lastday; //��¥ ���� ���
        int DaysCompare = ts.Days; //Days ������ ����.
        #endregion

        if (isAttendance == false)
        {
            icon.SetActive(true);

            if (DaysCompare == 1) //���� �⼮
            {
                if (days > 6)
                {
                    weeks = 1 + (days % 6);
                    days %= 6;
                    multiply_tag = weeks;
                }
                else
                {
                    weeks = 1;
                    multiply_tag = 1;
                }
                selectDay(days);
            }
            else //�����⼮�� �ƴѰ��
            {
                //days 0���� �ʱ�ȭ �� day1��ư Ȱ��ȭ
                DataController.instance.gameData.accDays = 0;
                days = 0;
                weeks = 1;
                multiply_tag = 1;
                selectDay(days);
            }
        }
        else //�⼮�� �̹� �� ���´�
        {
            for (int i = 0; i < days; i++) //�⼮�Ϸ� ��ư Ȱ��ȭ
            {
                image[i].sprite = Front[i].Behind[1];
            }
        }

        if (weeks < 9 && weeks> 1)
        {
            weekTMP.text = weeks.ToString();

            for (int i = 0; i < tagTMP.Length; i++)
            {
                tagTMP[i].text = weeks.ToString();
            }
            m_tag.SetActive(true);
        }
        else if(weeks > 9)
        {
            weekTMP.text = weeks.ToString();

            for (int i = 0; i < tagTMP.Length; i++)
            {
                tagTMP[i].text = "9";
            }
            m_tag.SetActive(true);

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
        
        if (number == days&& DataController.instance.gameData.isAttendance==false)
        {
            AudioManager.instance.RewardAudioPlay();
            image[number].sprite = Front[number].Behind[1];
            icon.SetActive(false);

            //�⼮ ���� ����.
            DataController.instance.gameData.accDays += 1; 
            DataController.instance.gameData.isAttendance = true;
            DataController.instance.gameData.attendanceLastday = DataController.instance.gameData.attendanceToday;

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
        GameManager.instance.GetHeart(10 * (hearts + 1) * multiply_tag);
    }

    public static implicit operator AttendanceCheck(GameManager v)
    {
        throw new NotImplementedException();
    }
}
    #endregion


