using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* ���̺�Ǿ�� �� ������ �־��ּ���! */

public class GameData
{
    //��ȭ
    //public int coin;
    //public int heart;

    //Truck
    public int berryCnt; // Ʈ�� ���� ����

    //���� �� ������ ����
    public BerryFieldData[] berryFieldData = new BerryFieldData[16];
   
    //�رݵ� ����
    public bool[] isBerryUnlock = new bool[192];

    //�˹� �������ִ��� ����
    //public bool[] isEmployed;

    //���� ����
    public int[] researchLevel=new int[7];

    //������ �⼮ ��¥ ����.
    //DateTime Lastday = new DateTime();

}
