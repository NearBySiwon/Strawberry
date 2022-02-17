using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabManager : MonoBehaviour
{
    [Serializable]
    public class PrefabStruct
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

    [Header("==========Research Or PTJ===========")]
    public bool PTJ;




    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;

    
    //��� ��������� Ȯ��
    static int employCount = 0;
    



    //===================================================================================================
    void Start()
    {
        
        InfoUpdate();
    }
    void Update()
    {
       
    }

    //===================================================================================================
    
    //coin ��ư -> ���� ����, ���� ��ȭ
    public void clickCoin_Research() {

        //������ �ö󰣴�.
        Info[prefabnum].Level++;
        levelNum.GetComponent<Text>().text = Info[prefabnum].Level.ToString();

        //�ش� �ݾ��� ������ ���ҵȴ�.
        GameManager.instance.coin -= Info[prefabnum].Price;
        GameManager.instance.ShowCoinText(GameManager.instance.coin);
        

    }


    //coin ��ư -> �˹� ��� ����
    public void clickCoin_PTJ() 
    {
        if (PTJ == true)//�׳� �ѹ��� Ȯ��
        {

            if (employCount < 3)//3�� ���ϸ� ���� ����
            {
                if (Info[prefabnum].Level == 0) //��� �߾ƴϸ� ��밡��            //�ǹ��� = �� prefabnum���� 0�� �־ �Ǵ°�
                {
                    //�ش� �ݾ��� ������ ���ҵȴ�.
                    GameManager.instance.coin -= Info[prefabnum].Price;
                    GameManager.instance.ShowCoinText(GameManager.instance.coin);

                    ++employCount;
                    Info[prefabnum].Level = 1;//1=���
                    levelNum.GetComponent<Text>().text = "���";

                }
                else //������̶�� �������� ����
                {
                    --employCount;
                    Info[prefabnum].Level = 0;//0=����
                    levelNum.GetComponent<Text>().text = "����";

                }
            }
            else 
            {
                if (Info[prefabnum].Level == 1) 
                {
                    --employCount;
                    Info[prefabnum].Level = 0;//0=����
                    levelNum.GetComponent<Text>().text = "����";
                }
                Debug.Log("3���� �Ѱ� ������� ���մϴ�."); 
            }


            //Debug.Log("count="+employCount);
            //Debug.Log("employ=" + Info[prefabnum].Level);
        }
    }


    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!����!!!!!!!!!!!!!���� 7�� ������ ���ڿ� ���õǾ� �ִ�!!! ���� �����ؾ���
        
        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 7)
        { Prefabcount -= 7; }
        prefabnum = Prefabcount;


        //Ÿ��Ʋ, ����, ���ΰ�, ����, ��뿩�� �ؽ�Ʈ�� ǥ��
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString();
        
        if (PTJ==true)
        {    levelNum.GetComponent<Text>().text = " ";    }
        else
        {    levelNum.GetComponent<Text>().text = Info[Prefabcount].Level.ToString();    }


        
        Prefabcount++;
    }
}
