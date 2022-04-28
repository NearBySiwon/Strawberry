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
    [Header("==========PTJ EXP===========")]
    public GameObject PTJExp;

    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;

    
    //��� ��������� Ȯ��
    static int employCount = 0;
    //������� �˹ٻ� ���
    static List<Sprite> workingList = new List<Sprite>();

    private GameObject PTJSlider;
    
    //===================================================================================================
    void Start()
    {
        InfoUpdate();        
    }
    //===================================================================================================
    //===================================================================================================
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
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString() + "A";//��� ǥ��


        if (PTJ == true)//�˹�
        { levelNum.GetComponent<Text>().text = "��� ��"; }
        else//����
        { levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString(); }



        Prefabcount++;
    }

    //=============================================================================================================================

    //coin ��ư -> ���� ����, ����
    public void clickCoin_Research() {

        if (DataController.instance.gameData.researchLevel[prefabnum] < 25)//���� 25�� �Ѱ�α�
        {
            
            //�ش� �ݾ��� ������ ����
            GameManager.instance.UseCoin(Info[prefabnum].Price);

            //�ش� �ݾ��� ���� ���� ���κ��� ������
            if (DataController.instance.gameData.coin >= Info[prefabnum].Price)
            {
                //������ �ö󰣴�.
                DataController.instance.gameData.researchLevel[prefabnum]++;
                levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();
            }
            
        }
    }

    //=============================================================================================================================

    //PTJ����â ����
    public void ActiveExplanation() 
    {
        //â�� ����.
        PTJExp.SetActive(true);

        //�Һ� ���� =======================================================
        //PICTURE
        PTJExp.transform.GetChild(2).transform.GetComponent<Image>().sprite 
            = Info[prefabnum].Picture;
        //NAME
        PTJExp.transform.GetChild(3).transform.GetComponent<Text>().text 
            = Info[prefabnum].Name;
        //Explanation
        PTJExp.transform.GetChild(4).transform.GetComponent<Text>().text
            = Info[prefabnum].Explanation;

        //���� ����========================================================
        
        PTJSlider = PTJExp.transform.GetChild(7).transform.gameObject;//PTJSlider gameobject
        PTJSlider.GetComponent<Slider>().value = 1;
        
        //EmployButton
        EmployButton(1);
        
        //Slider�� ����ɶ� ����
        PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { EmployButton((int)(PTJSlider.GetComponent<Slider>().value)); });
        
    }

    //��� ��ư ���� ����
    public void EmployButton(int SliderNum) 
    {
        Debug.Log("ID = "+prefabnum+" / TEST = "+ DataController.instance.gameData.PTJNum[prefabnum]);
        //������� �ƴϸ�
        if (DataController.instance.gameData.PTJNum[prefabnum]==0) 
        {
            //n�� ���
            PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text
                = SliderNum.ToString() + "�� ���";
            //PRICE
            PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text
               = (SliderNum * Info[prefabnum].Price).ToString();
        }
        else //�̹� ������̸�
        {
            //�����̵� ù��°
            PTJSlider.GetComponent<Slider>().value = 1;
            //n�� ��� -> ��� ����
            PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text
                = "��� ����";
            //PRICE -> ��ĭ
            PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text
                = "";
        }
        
    }
    //============================================================================================================
    public void HireFire() {
        
        //3�� �Ʒ��� ������̸�
        if (employCount < 3)
        {
            //������� �ƴϸ�
            if (Info[prefabnum].isEmployed == false) 
            { Hire(prefabnum, (int)(PTJSlider.GetComponent<Slider>().value)); }
            //�̹� ������̸�
            else 
            { Fire(prefabnum); }
        }
        //�̹� 3���̻� ������̸�
        else 
        {
            //������� �ƴϸ�
            if (Info[prefabnum].isEmployed == false) { Debug.Log("3���� �̹� ������Դϴ�."); }
            //�̹� ������̸�
            else { Fire(prefabnum); }
        }
    }

    private void Hire(int ID,int num) 
    {

        GameManager.instance.UseCoin(Info[ID].Price);

        DataController.instance.gameData.PTJNum[ID] = num;

        
        Info[ID].isEmployed = true;
        levelNum.GetComponent<Text>().text = "��� ��";
        levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
        PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;

        //main game
        workingList.Remove(null);
        workingList.Add(Info[ID].FacePicture);
        GameManager.instance.workingApply(workingList);

        ++employCount;
        GameManager.instance.workingCount(employCount);

        EmployButton(1);

        DataController.instance.gameData.PTJNum[ID] = num;
    }
    private void Fire(int ID) 
    {
        DataController.instance.gameData.PTJNum[ID] = 0;
        Info[ID].isEmployed = false;
        levelNum.GetComponent<Text>().text = "��� ��";
        levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

        --employCount;

        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList);
        GameManager.instance.workingCount(employCount);

        EmployButton(1);
        DataController.instance.gameData.PTJNum[ID] = 0;
    }

}
