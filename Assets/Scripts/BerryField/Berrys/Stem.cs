using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Stem : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;
    //private Vector2 stemPos;
    [SerializeField]
    private Vector3 farmPos;
    private int seedAnimLevel;   
    private Bug stemBug;

    public int stemIdx;
    public Berry instantBerry;

    /*public const float STEM_LEVEL_0 = Globalvariable.STEM_LEVEL_0;
    public const float STEM_LEVEL_1 = Globalvariable.STEM_LEVEL_1;
    public const float STEM_LEVEL_2 = Globalvariable.STEM_LEVEL_2;
    public const float STEM_LEVEL_3 = Globalvariable.STEM_LEVEL_3;
    public const float STEM_LEVEL_MAX = Globalvariable.STEM_LEVEL_MAX;*/

    RainCtrl rainCtrl;
    Globalvariable global;
    void Awake()
    {
        seedAnimLevel = 0;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        stemBug = GameManager.instance.bugList[stemIdx];

        rainCtrl = GameObject.FindGameObjectWithTag("Rain").GetComponent<RainCtrl>();
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();
        //farmPos = new Vector3(GetComponentInParent<Farm>().transform.position.x, GetComponentInParent<Farm>().transform.position.y, 0);
    }
    private void OnEnable()
    {       

        DataController.instance.gameData.berryFieldData[stemIdx].isStemEnable = true;
        
        if (DataController.instance.gameData.berryFieldData[stemIdx].createTime >= DataController.instance.gameData.stemLevel[1])
        {
            sprite.sortingOrder = 0;
        }
        else sprite.sortingOrder = 2;

        

        if (DataController.instance.gameData.berryFieldData[stemIdx].isPlant)
        {
            MakeBerry();
            updateStem();
            return;
        }  
        
        DataController.instance.gameData.berryFieldData[stemIdx].randomTime = 
            Random.Range(DataController.instance.gameData.stemLevel[1] + 2.0f, DataController.instance.gameData.stemLevel[4] - 2.0f);
        DefineBerry();
        MakeBerry();
        SetAnim(0);
    }
    private void OnDisable()
    {
        DataController.instance.gameData.berryFieldData[stemIdx].isStemEnable = false;
        // ���� �ʱ�ȭ       
        if (DataController.instance.gameData.berryFieldData[stemIdx].isPlant) return;

        DataController.instance.gameData.berryFieldData[stemIdx].canGrow = true;
        DataController.instance.gameData.berryFieldData[stemIdx].createTime = 0f;
        DataController.instance.gameData.berryFieldData[stemIdx].randomTime = 0f;

        // ���� Ʈ������ �ʱ�ȭ
        //transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        sprite.sortingOrder = 2;        
        instantBerry = null;
    }
    void Update() // �ð��� ���� ���� ����
    {       
        if (DataController.instance.gameData.berryFieldData[stemIdx].canGrow) 
        {
            DataController.instance.gameData.berryFieldData[stemIdx].createTime += Time.deltaTime * rainCtrl.mult;
            updateStem();
        }
    }
    private void updateStem()
    {      
        if (DataController.instance.gameData.berryFieldData[stemIdx].randomTime <= DataController.instance.gameData.berryFieldData[stemIdx].createTime)
        {
            stemBug.GenerateBug();
            DataController.instance.gameData.berryFieldData[stemIdx].randomTime = 200f;
        }
        if (DataController.instance.gameData.stemLevel[1] <= DataController.instance.gameData.berryFieldData[stemIdx].createTime
            && DataController.instance.gameData.berryFieldData[stemIdx].createTime < DataController.instance.gameData.stemLevel[2])
        {
            if (seedAnimLevel == 1) return;
            SetAnim(1);
        }
        else if (DataController.instance.gameData.stemLevel[2] <= DataController.instance.gameData.berryFieldData[stemIdx].createTime 
            && DataController.instance.gameData.berryFieldData[stemIdx].createTime < DataController.instance.gameData.stemLevel[3])
        {
            if (seedAnimLevel == 2) return;
            SetAnim(2);
        }
        else if (DataController.instance.gameData.stemLevel[3] <= DataController.instance.gameData.berryFieldData[stemIdx].createTime
            && DataController.instance.gameData.berryFieldData[stemIdx].createTime < DataController.instance.gameData.stemLevel[4])
        {
            if (seedAnimLevel == 3) return;
            SetAnim(3);
        }
        else if (DataController.instance.gameData.berryFieldData[stemIdx].createTime >= DataController.instance.gameData.stemLevel[4])
        {            
            DataController.instance.gameData.berryFieldData[stemIdx].canGrow = false;
            if(!GameManager.instance.isblackPanelOn)
                GameManager.instance.farmList[stemIdx].GetComponent<BoxCollider2D>().enabled = true; // ���� �ݶ��̴� �ٽ� Ȱ��ȭ

            SetAnim(4);
        }
    }
    public void SetAnim(int level)
    {
        this.seedAnimLevel = level;
        anim.SetInteger("Seed", seedAnimLevel);

        if (this.seedAnimLevel == 0)
        {
            //transform.position = new Vector2(farmPos.x, farmPos.y + 0.02f);
        }
        else if (this.seedAnimLevel == 1)
        {
            sprite.sortingOrder = 0;       
        }
        else if (this.seedAnimLevel == 2)
        {                      
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            instantBerry.transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y - 0.1f, transform.position.z);
        }
        else if (this.seedAnimLevel == 3)
        {                      
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            instantBerry.transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y - 0.06f, transform.position.z);
        }
        else if (this.seedAnimLevel == 4)
        {          
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<SpriteRenderer>().sprite = global.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx].GetComponent<SpriteRenderer>().sprite;
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);        
            instantBerry.transform.position = new Vector2(transform.position.x + 0.32f, transform.position.y + 0.06f);
        }
    }
    void DefineBerry() // ���� Ȯ�������� ������ ���� ����
    {
        int cumulative = 0, probSum = 0;
        int len = global.berryListAll.Count;

        for (int i = 0; i < len; i++)
        {
            if(DataController.instance.gameData.isBerryUnlock[i])
            {
                probSum += global.berryListAll[i].GetComponent<Berry>().berrykindProb; // �رݵ� ���⿡�� ������ �߻�Ȯ���� ������
            }           
        }
        int berryRandomChance = Random.Range(0, probSum + 1);

        for (int i = 0; i < len; i++)
        {
            if (DataController.instance.gameData.isBerryUnlock[i])
            {
                cumulative += global.berryListAll[i].GetComponent<Berry>().berrykindProb; // �رݵ� ���⿡�� ������ �߻�Ȯ���� ������
                if (berryRandomChance <= cumulative)
                {
                    DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx = i;
                    break;
                }
            }                       
        }
    }
    void MakeBerry() // ���� ����
    {
        // �ε�ɶ��� ���������ϱ� �ε� Ŭ�������� �ٽ� �����ؾߵ�(MakeBerry() ȣ���ϸ� ��)

        GameObject instantBerryObj = Instantiate(global.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx], this.transform);
        instantBerryObj.name = global.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx].name;

        instantBerry = instantBerryObj.GetComponent<Berry>();
        instantBerry.gameObject.SetActive(false);
    }
}
