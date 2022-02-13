using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabManager : MonoBehaviour
{
    [Serializable]
    public struct PrefabStruct
    {
        public string Name,Explanation;
        public int Price, Level;

        public PrefabStruct(string Name,string Explanation, int Price, int Level)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Level = Level;//PTJ�̶�� ��뿩�� �ǹ�. 0�� ������ 1�� �����
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    PrefabStruct[] Info;

    //Research Info  ������ �͵�
    [Header("==========INFO ������ ���=========")]
    public GameObject titleText;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;


    //gameManager Script
    GameManager gm1;

    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;


    //===================================================================================================
    void Start()
    {
        gm1 = GameObject.Find("GameManager").GetComponent<GameManager>();
        InfoUpdate();
    }
    void Update()
    {
       
    }

    //===================================================================================================
    
    
    //coin ��ư -> ���� ����, ���� ��ȭ
    public void clickCoin() {

        //������ �ö󰣴�.
        Info[prefabnum].Level++;
        levelNum.GetComponent<Text>().text = Info[prefabnum].Level.ToString();

        //�ش� �ݾ��� ������ ���ҵȴ�.
        gm1.coin -= Info[prefabnum].Price;
        gm1.ShowCoinText(gm1.coin);
        //gm1.CoinText.text = gm1.coin.ToString() + " A";

    }


    //coin ��ư -> �˹� ����, ��� ����
    public void clickCoin_PTJ() 
    {

        if (Info[prefabnum].Level == 0) //��� ���� �ƴ϶�� !!!!!!!!!!!!�� prefabnum���� 0�� �־ �Ǵ°�
        {
            //�ش� �ݾ��� ������ ���ҵȴ�.
            gm1.coin -= Info[prefabnum].Price;
            gm1.ShowCoinText(gm1.coin);
            //gm1.CoinText.text = gm1.coin.ToString() + " A";

            Info[prefabnum].Level = 1;
            levelNum.GetComponent<Text>().text = "�����";

        }
        else //������̶��
        {
            Info[prefabnum].Level = 0;
            levelNum.GetComponent<Text>().text = "����";
        }

    }


    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!����!!!!!!!!!!!!!7 �� ���ڴ� ������ ���ڿ� ���õǾ� �ִ�!!! ���� �����ؾ���
        if (Prefabcount >= 7)
        { Prefabcount -= 7; }
        prefabnum = Prefabcount;

        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString();

        if (Info[Prefabcount].Level == 0)//level�� 0�̶�� PTJ�� ���õ� ���̴�.
        {    levelNum.GetComponent<Text>().text = "����";    }
        else
        {    levelNum.GetComponent<Text>().text = Info[Prefabcount].Level.ToString();    }

        Debug.Log(prefabnum);
        Prefabcount++;
    }
}
