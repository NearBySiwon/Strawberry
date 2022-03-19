using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* ���̺�Ǿ�� �� ������ �־��ּ���! */

public class GameData
{
    //��ȭ
    public int coin;
    public int heart;
    public int medal;

    //Truck
    public int berryCnt; // Ʈ�� ���� ����

    //���� �� ������ ����
    public BerryFieldData[] berryFieldData = new BerryFieldData[16];

    //�رݵ� ����
    //�迭 ũ�� ���� �ȵǴ°� ���� C#�迭�� Ư���̴�.
    //List�� ����ϰų� Linq����ؾ� ��
    public bool[] isBerryUnlock = new bool[192];



    //���� ����
    public int[] researchLevel=new int[7];

    //�������� �޼� ��ġ
    public int[] challengeGauge = new int[7];
    //�������� ���� ���� ����
    public bool[] challengeEnd = new bool[7];

    //����
    public bool[] isNewsUnlock=new bool[7];

    //������ �⼮ ��¥ ����.
    public DateTime Lastday = new DateTime(2022, 3, 14);
    //�⼮ �� ��
    public int days=0;
    //���� �⼮ ���� �Ǵ�
    public bool attendance = false;

}
