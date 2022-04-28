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

    private GameObject PTJExplanations;

    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;

    
    //��� ��������� Ȯ��
    static int employCount = 0;
    //������� �˹ٻ� ���
    static List<Sprite> workingList = new List<Sprite>();


    private GameObject PTJExplanation;
    private GameObject PTJExp;
    private GameObject PTJSlider;
    //===================================================================================================
    void Start()
    {
        if (PTJ == true)
        {
            PTJExplanations = GameObject.FindGameObjectWithTag("PTJExplanation");
            PTJExplanation = transform.GetChild(prefabnum).gameObject;
            PTJExp = PTJExplanation.transform.GetChild(0).gameObject;
            PTJSlider = PTJExp.transform.GetChild(7).transform.gameObject;
        }
        InfoUpdate();
    }
    //===================================================================================================

    //coin ��ư -> ���� ����, ����
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


    //coin ��ư -> �˹� ���
    public void clickCoin_PTJ(int ID, int num) 
    {
        //ID=prefabnum
        Debug.Log("ID="+ID+"  num="+num);
        //DataController.instance.gameData.PTJNum[ID] = num;//�ش� ID�� n�� ���Ǿ����� ����


        //������̸� �������
        if (Info[ID].isEmployed == true) { fire(ID); }
        //������� �ƴ϶�� ���
        else
        {
            if (employCount < 3)//3�� ������ ��
            {
                hire(ID,num);
            }
            else //3�� �̻��� ��
            {   Debug.Log("�̹� 3���� ������Դϴ�.");   }
        }
    }


    private void hire(int ID,int num)
    {
        //�ش� �ݾ��� ������ ����
        GameManager.instance.UseCoin(Info[ID].Price);

        DataController.instance.gameData.PTJNum[ID] = num;

        //���������� �ð������� ���̱�=======================================
        Info[ID].isEmployed = true;//���
        levelNum.GetComponent<Text>().text = "��� ��";//��������� ǥ��
        levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747 ���ڻ� ����
        PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;//��� ��������Ʈ ���� �̹����� ����

        //main game�� ������� �˹ٻ� ���̱�======================
        workingList.Remove(null);
        workingList.Add(Info[ID].FacePicture);//�ش� �˹ٻ� �� ����Ʈ�� �߰�
        GameManager.instance.workingApply(workingList);//GameManager workingApply�� ��� list ����

        //�˹ٻ� ����=============================================
        ++employCount;//������� �˹ٻ� ���� ����
        GameManager.instance.workingCount(employCount);//�˹ٻ� ���� �����ش�

        //ExplanationUpdate((int)PTJSlider.GetComponent<Slider>().value);//Explanationâ�� ���������� ���̱�
    }

    private void fire(int ID) 
    {
        DataController.instance.gameData.PTJNum[ID] = 0;
        Info[ID].isEmployed = false;//0=����
        levelNum.GetComponent<Text>().text = "��� ��";//��� ������ ǥ��
        levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);//���ڻ� ȸ������
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;//��� ��������Ʈ �������
       
        --employCount;

        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList);
        GameManager.instance.workingCount(employCount);
        //ExplanationUpdate((int)PTJSlider.GetComponent<Slider>().value);

    }
    //=============================================================================================================================
    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!����!!!!!!!!!!!!!���� 6�� ������ ���ڿ� ���õǾ� �ִ�!!! ���� ���� . ���������ϱ�
        
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

    //=============================================================================================================================
    public void Explanation() {

        PTJExp.SetActive(true);//����â ����
        try
        {
            //Explanation ������ ä���.
            PTJExp.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Image>().sprite
                = Info[prefabnum].Picture;//�� ����

            PTJExp.transform.GetChild(3).transform.gameObject.GetComponentInChildren<Text>().text 
                = Info[prefabnum].Name;//�̸� �ؽ�Ʈ

            PTJExp.transform.GetChild(4).transform.gameObject.GetComponentInChildren<Text>().text 
                = Info[prefabnum].Explanation;//���� �ؽ�Ʈ 
            
            
            

            ExplanationUpdate(1);//ó������ 1�� ������� ����
            PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
                (delegate { ExplanationUpdate((int)PTJSlider.GetComponent<Slider>().value); });//�����̴� �� �ٲ𶧸���


            PTJExp.transform.GetChild(5).transform.GetComponent<Button>().onClick.AddListener
                (delegate { clickCoin_PTJ(prefabnum, (int)PTJSlider.GetComponent<Slider>().value); });//���� ��ư ����

            //��������� ������
        }
        catch
        {
            Debug.Log("PTJExplanation �ε���");
        }
    }



    public void ExplanationUpdate(int value) 
    {
        if (workingList.Contains(Info[prefabnum].FacePicture))//�̹� ������̶�� (��� ����Ʈ�� �̹� �˹ٻ��� ������)
        {
            //������� ��ư
            PTJExp.transform.GetChild(6).transform.gameObject.GetComponentInChildren<Text>().text
                = "";
            PTJExp.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text
                = "�������";
        }
        else//��� ���� �ƴ϶��
        {
            //��� ���� ���̱�
            PTJExp.transform.GetChild(6).transform.gameObject.GetComponentInChildren<Text>().text
                = (Info[prefabnum].Price * value).ToString() + "A";//������ ���� ���̱�
            PTJExp.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text
           = value.ToString() + "�� ���";//��� ����ϴ���
        }
    }


}
