using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TodayDate : MonoBehaviour
{

    [SerializeField] Text today;
    DateTime Today = DateTime.Now;
    DateTime Lastday = new DateTime(2022, 3, 10);


    void Update()
    {
        //today.text = DateTime.Now.ToString("yyyy�� MM dd�� dddd");
    }

    public void isCompare()
    {
        int compare = Today.CompareTo(Lastday); // 1�̳����� ��¥ �ٸ�
        Debug.Log("compare");
    }

}
