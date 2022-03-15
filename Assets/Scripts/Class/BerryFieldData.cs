using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryFieldData
{
    //Bug
    public float bugProb; // �ű�   
    public int bugIdx;
    public float scale; // �ű�
    public bool isBugEnable; // true���� �־����Ŵϱ� ���ְ� false���� �����Ŵϱ� �ǵ�������

    //Weed
    public float weedProb = 20f; // // ���Ƿ� �ʱ�ȭ ��Ų ������
    public float xPos = 0f;   
    public int weedSpriteNum; 
    public bool isWeedEnable; // true���� �־����Ŵϱ� ���ְ� false���� �����Ŵϱ� �ǵ�������

    //Farm
    public bool isPlant = false;
    public int farmIdx;
    public bool isHarvest = false;

    public bool hasWeed = false;
    public bool canGrowWeed = true;
    public float weedTime = 0f;
    public float period = 15f; // ���Ƿ� �ʱ�ȭ ��Ų ������

    //Stem
    public float createTime = 0f;
    public bool canGrow = true;
    public bool hasBug = false;

    public int stemIdx;
    public int berryPrefabNowIdx;
    public bool isStemEnable; // // true���� �־����Ŵϱ� ���ְ� false���� �����Ŵϱ� �ǵ�������
    public float randomTime = 0f;
}
