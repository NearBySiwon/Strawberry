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
        public Sprite FacePicture;
        public string Explanation;
        public int Price;
        public bool isEmployed;


        public PrefabStruct(string Name,string Explanation, int Price, Sprite Picture, Sprite FacePicture, bool isEmployed, bool exist)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Picture = Picture;
            this.FacePicture = FacePicture;
            this.isEmployed=isEmployed;

            
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

    static List<Sprite> workingList = new List<Sprite>();
    


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

        if (DataController.instance.gameData.researchLevel[prefabnum] < 25)//���� 25�� �Ѱ�α�
        {
            //������ �ö󰣴�.
            DataController.instance.gameData.researchLevel[prefabnum]++;
            levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();
        
        //�ش� �ݾ��� ������ ���ҵȴ�.
        //GameManager.instance.coin -= Info[prefabnum].Price;
        DataController.instance.gameData.coin -= Info[prefabnum].Price;
        //GameManager.instance.ShowCoinText(GameManager.instance.coin);
        GameManager.instance.ShowCoinText();
        }
    }


    //coin ��ư -> �˹� ��� ����
    public void clickCoin_PTJ() 
    {
        if (PTJ == true)//�׳� �ѹ��� Ȯ��
        {
            if (employCount < 3)//3�� ������ ��
            {
                if (Info[prefabnum].isEmployed == false) //����߾ƴϸ� ���           
                {   hire();   }
                else //������̸� �������
                {   fire();   }
            }
            else //3�� �̻��� ��
            {
                if (Info[prefabnum].isEmployed == true)//������̸� �������
                {   fire();   }
            }
        }
    }


    private void hire()  //�ǹ��� = �� prefabnum���� 0�� �־ �Ǵ°�
    {
        //�ش� �ݾ��� ������ ���ҵȴ�.
        //GameManager.instance.coin -= Info[prefabnum].Price;
        //GameManager.instance.ShowCoinText(GameManager.instance.coin);
        DataController.instance.gameData.coin -= Info[prefabnum].Price;
        //GameManager.instance.UseCoin(Info[prefabnum].Price); // �Լ��� �ٲ㺽
        GameManager.instance.ShowCoinText();


        Info[prefabnum].isEmployed = true;//���
        levelNum.GetComponent<Text>().text = "��� ��";//��������� ǥ��
        levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747 ���ڻ� ����
        PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;//��潺������Ʈ �������� ����

        workingList.Remove(null);
        workingList.Add(Info[prefabnum].FacePicture);//�ش� �� ����Ʈ�� �߰�
        GameManager.instance.workingApply(workingList);//GameManager workingApply�� ������� ���� list ����


        ++employCount;
        GameManager.instance.workingCount(employCount);
    }
    private void fire() 
    {
        
        Info[prefabnum].isEmployed = false;//0=����
        levelNum.GetComponent<Text>().text = "��� ��";//��� ������ ǥ��
        levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);//���ڻ� ȸ������
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;//��� ��������Ʈ �������
       
        --employCount;

        workingList.Remove(Info[prefabnum].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList);
        GameManager.instance.workingCount(employCount);

    }

    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!����!!!!!!!!!!!!!���� 7�� ������ ���ڿ� ���õǾ� �ִ�!!! ���� �����ؾ���
        
        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 6)
        { Prefabcount -= 6; }
        prefabnum = Prefabcount;


        //Ÿ��Ʋ, ����, ���ΰ�, ����, ��뿩�� �ؽ�Ʈ�� ǥ��
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;
        if (Info[Prefabcount].Picture != null)
        {
            facePicture.GetComponent<Image>().sprite = Info[Prefabcount].Picture;
        }
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString()+"A";
        
        if (PTJ==true)
        {    levelNum.GetComponent<Text>().text = "��� ��";    }
        else
        {    levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();    }


        
        Prefabcount++;
    }


}
