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
    public int truckBerryCnt; // Ʈ�� ���� ����
    public int truckCoin;

    //���±��� ���� ���� �� ����
    public int totalBerryCnt;

    //���� �� ������ ����
    public BerryFieldData[] berryFieldData = new BerryFieldData[16];

    //�رݵ� ����
    //�迭 ũ�� ���� �ȵǴ°� ���� C#�迭�� Ư���̴�.
    //List�� ����ϰų� Linq����ؾ� ��
    public bool[] isBerryUnlock = new bool[192];

    //���ο� ���� ����
    public int newBerryResearch;

    //���� ����
    public int[] researchLevel=new int[7];

    //�������� �޼� ��ġ
    public int[] challengeGauge = new int[7];
    //�������� ���� ���� ����
    public bool[] challengeEnd = new bool[7];

    //����
    public bool[] isNewsUnlock=new bool[7];

    //������ �⼮ ��¥ ����.
    public DateTime Lastday = new DateTime();
    public DateTime Today = new DateTime();
    //�⼮ �� ��
    public int days=0;
    //���� �⼮ ���� �Ǵ�
    public bool attendance = false;

}
