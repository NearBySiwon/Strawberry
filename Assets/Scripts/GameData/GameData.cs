using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* ���̺�Ǿ�� �� ������ �־��ּ���! */

public class GameData
{
    //Truck
    public int berryCnt; // Ʈ�� ���� ����

    //���� �� ������ ����
    public BerryFieldData berryFieldData = new BerryFieldData();

    //�رݵ� ����
    public bool[] isBerryUnlock = new bool[192];

    //���� �߻� Ȯ��
    public int[] berryRankProb = { 50, 35, 15 };

    //�˹� �������ִ��� ����
    //public bool[] isEmployed;

    //���� ����
    public int[] researchLevel=new int[7];

    //������ �⼮ ��¥ ����.
    //DateTime Lastday = new DateTime();

}
