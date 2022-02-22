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
        public string Name;
        public Sprite Picture;
        public string Explanation;
        public int Price, Level;
        

        public PrefabStruct(string Name,string Explanation, int Price, int Level, Sprite Picture)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Level = Level;//PTJ�̶�� ��뿩�� �ǹ�. 0�� ������ 1�� �����
            this.Picture = Picture;
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    PrefabStruct[] Info;

    //Research Info  ������ �͵�
    [Header("==========INFO ������ ���=========")]
    [SerializeField]
    private GameObject PTJBackground;
    public GameObject titleText;
    public GameObject facePicture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;

    [Header("==========Research Or PTJ===========")]
    public bool PTJ;
    [Header("==========select PTJ===========")]
    [SerializeField]
    private Sprite selectPTJSprite;
    [SerializeField]
    private Sprite originalPTJSprite;


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

            if (employCount < 3)//3�� ������ �� ��� Ȥ�� ��� ����
            {
                if (Info[prefabnum].Level == 0) //����߾ƴϸ� ���            //�ǹ��� = �� prefabnum���� 0�� �־ �Ǵ°�
                {
                    //�ش� �ݾ��� ������ ���ҵȴ�.
                    GameManager.instance.coin -= Info[prefabnum].Price;
                    GameManager.instance.ShowCoinText(GameManager.instance.coin);

                    ++employCount;
                    Info[prefabnum].Level = 1;//1=���
                    levelNum.GetComponent<Text>().text = "��� ��";
                    levelNum.GetComponent<Text>().color = new Color32(245,71,71,255);//#F54747

                    PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;//�ƴ��̰� �� �ȵǳİ�

                }
                else //������̸� �������
                {
                    --employCount;
                    Info[prefabnum].Level = 0;//0=����
                    levelNum.GetComponent<Text>().text = "��� ��";
                    levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
                    PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;
                }
            }
            else //3�� �̻��� ��
            {
                if (Info[prefabnum].Level == 1)//������̸� �������
                {
                    --employCount;
                    Info[prefabnum].Level = 0;//0=����
                    levelNum.GetComponent<Text>().text = "��� ��";
                    levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
                    PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;
                }
                Debug.Log("3���� �Ѱ� ������� ���մϴ�."); 
            }

        }
    }


    private void hire() { }
    private void fire() { }

    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!����!!!!!!!!!!!!!���� 7�� ������ ���ڿ� ���õǾ� �ִ�!!! ���� �����ؾ���
        
        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 7)
        { Prefabcount -= 7; }
        prefabnum = Prefabcount;


        //Ÿ��Ʋ, ����, ���ΰ�, ����, ��뿩�� �ؽ�Ʈ�� ǥ��
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;
        if (Info[Prefabcount].Picture != null)
        {
            facePicture.GetComponent<Image>().sprite = Info[Prefabcount].Picture;
        }
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString();
        
        if (PTJ==true)
        {    levelNum.GetComponent<Text>().text = "��� ��";    }
        else
        {    levelNum.GetComponent<Text>().text = Info[Prefabcount].Level.ToString();    }


        
        Prefabcount++;
    }
}
