using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* ���̺�Ǿ�� �� ������ �־��ּ���! */

public class GameData
{
    //Ŭ���� ���� ��¥
    public DateTime cloudSaveTime;

    //��ȭ
    public int coin;
    public int heart;
    public int medal;

    //Truck
    public int truckBerryCnt; // Ʈ�� ���� ����
    public int truckCoin;

    // ���� ���� �ð�
    public float[] stemLevel = { 0f, 30f, 60f, 90f, 120f};

    //���� �� ������ ����
    public BerryFieldData[] berryFieldData = new BerryFieldData[16];

    //�رݵ� ����
    //�迭 ũ�� ���� �ȵǴ°� ���� C#�迭�� Ư���̴�.
    //List�� ����ϰų� Linq����ؾ� ��
    //=====================================================================================
    public bool[] isBerryUnlock = new bool[192];

    //���ο� ���� ����
    public int newBerryResearch;

    //���� ����
    public int[] researchLevel=new int[7];

    //�������� �޼� ��ġ
    public int[] challengeGauge = new int[7];
    //�������� ���� ���� ����
    public bool[] challengeEnd = new bool[7];

    //���������� ���� ���� ���� ���� (���� : accumulate)
    public int unlockBerryCnt; // �ر� ���� ���� (����)
    public int totalHarvBerryCnt; // ��Ȯ ���� ���� (����)
    public int accCoin; // ���� ����
    public int accHeart; // ���� ��Ʈ
    public int accAttendance; // ���� �⼮
    public int mgPlayCnt; // �̴ϰ��� �÷��� Ƚ��
    //���±��� ���� ���� �� ����
    //public int totalBerryCnt; => �̰� ��Ȯ�Ѱ� ����?

    //����
    public bool[] isNewsUnlock=new bool[7];
    public bool[] NewsEnd = new bool[7];

    //PTJ
    public int[] PTJNum = new int[6];

    //=====================================================================================


    //������ �⼮ ��¥ ����.
    public DateTime Lastday = new DateTime();
    public DateTime Today = new DateTime();
    //�⼮ �� ��
    public int days=0;
    //���� �⼮ ���� �Ǵ�
    public bool attendance = false;

}
