using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJResearch : MonoBehaviour
{
    [Serializable]
    public class PrefabStruct
    {
        public string Name;//�˹ٻ� �̸�, ���� ����
        public Sprite Picture;//����
        public Sprite FacePicture;//�˹ٻ� �� ����
        public string Explanation;//����
        public int Price;//����
        public bool isEmployed;//��� �� �ΰ�


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
    PrefabStruct[] Info;//����ü

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
    public bool PTJ;//���� �˹ٻ��ΰ� Ȯ��

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


    private GameObject PTJExplanation;

    //===================================================================================================
    void Start()
    {
        PTJExplanation = GameObject.Find("PTJExplanation");//gameManager�� ���� FInd�ʿ���� �ٵ� �ʹ� ��
        InfoUpdate();
    }

    //===================================================================================================
    
    //coin ��ư -> ���� ����, ���� ��ȭ
    public void clickCoin_Research() {

        if (DataController.instance.gameData.researchLevel[prefabnum] < 25)//���� 25�� �Ѱ�α�
        {
            //������ �ö󰣴�.
            DataController.instance.gameData.researchLevel[prefabnum]++;
            levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();
        
        //�ش� �ݾ��� ������ ����
        GameManager.instance.UseCoin(Info[prefabnum].Price);
        
        }
    }


    //coin ��ư -> �˹� ��� ����
    public void clickCoin_PTJ() 
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


    private void hire()  //�ǹ��� = �� prefabnum���� 0�� �־ �Ǵ°�
    {
        //�ش� �ݾ��� ������ ����
        GameManager.instance.UseCoin(Info[prefabnum].Price);
        
        Info[prefabnum].isEmployed = true;//���
        levelNum.GetComponent<Text>().text = "��� ��";//��������� ǥ��
        levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747 ���ڻ� ����
        PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;//��� ��������Ʈ ���� �̹����� ����

        workingList.Remove(null);
        workingList.Add(Info[prefabnum].FacePicture);//�ش� �˹ٻ� �� ����Ʈ�� �߰�
        GameManager.instance.workingApply(workingList);//GameManager workingApply�� ��� list ����


        ++employCount;//������� �˹ٻ� ���� ����
        GameManager.instance.workingCount(employCount);//�˹ٻ� ���� �����ش�
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
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;//����(�̸�) ǥ��
        if (Info[Prefabcount].Picture != null)
        {
            facePicture.GetComponent<Image>().sprite = Info[Prefabcount].Picture;//�׸� ǥ��
        }
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;//���� �ؽ�Ʈ ǥ��
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString()+"A";//��� ǥ��
        

        if (PTJ==true)//�˹�
        {    levelNum.GetComponent<Text>().text = "��� ��";    }
        else//����
        {    levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();    }


        
        Prefabcount++;
    }

    public void Explanation() {

        GameObject PTJExp = PTJExplanation.transform.GetChild(0).gameObject;

        PTJExp.SetActive(true);
        try
        {
            //Explanation ������ ä���.
            PTJExp.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Image>().sprite
                = Info[prefabnum].Picture;//�� ����

            PTJExp.transform.GetChild(3).transform.gameObject.GetComponentInChildren<Text>().text 
                = Info[prefabnum].Name;//�̸� �ؽ�Ʈ

            PTJExp.transform.GetChild(4).transform.gameObject.GetComponentInChildren<Text>().text 
                = Info[prefabnum].Explanation;//���� �ؽ�Ʈ 

        }
        catch
        {
            Debug.Log("PTJExplanation �ε���");
        }


    }

}
