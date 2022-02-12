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
            this.Level = Level;
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    PrefabStruct[] Info;


    //gameManager Script
    GameManager gm1;

    //�߰� �� Prefab ��
    static int Prefabcount=0;
    //�ڽ��� ���° Prefab����
    int prefabnum;

    //Research Info  ������ �͵�=====================================================================
    [Header("==========INFO ������ ���=========")]
    public GameObject titleText;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;
      


    void Start()
    {
        gm1 = GameObject.Find("GameManager").GetComponent<GameManager>();
        InfoUpdate();
    }


    void Update()
    {
       
    }


    //coin ��ư�� ������ ����
    public void clickCoin() {

        //������ �ö󰣴�.
        Info[prefabnum].Level++;
        levelNum.GetComponent<Text>().text = Info[prefabnum].Level.ToString();

        //�ش� �ݾ��� ������ ���ҵȴ�.
        gm1.coin -= Info[prefabnum].Price;
        gm1.CoinText.text = gm1.coin.ToString() + " A";

    }


    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!����!!!!!!!!!!!!!7 �� ���ڴ� ������ ���ڿ� ���õǾ� �ִ�!!!
        if (Prefabcount >= 7)
        { Prefabcount -= 7; }
        prefabnum = Prefabcount;

        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString();
        levelNum.GetComponent<Text>().text = Info[Prefabcount].Level.ToString();
        //prefabnum = Prefabcount;
        //Prefabcount++;

        
        Prefabcount++;
    }
}
