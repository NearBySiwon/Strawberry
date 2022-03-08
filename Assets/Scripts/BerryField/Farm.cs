using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public Weed weed; // �굵 �迭�� ������ҵ�? ���� ����

    public bool isPlant = false;
    public int farmIdx;
    public bool isHarvest = false;
    
    public bool hasWeed = false;
    public bool canGrowWeed = true;
    public float weedTime = 0f;
    public float period = 12f;  // ���� �� �ű�  
    
    void Awake()
    {
           
    }    
    void Update()
    {
        if (hasWeed || !canGrowWeed) return;

        if (weedTime <= period)
        {
            weedTime += Time.deltaTime;
        }
        else
        {
            weed.GenerateWeed();
            weedTime = 0f;
        }
    }
    
}
