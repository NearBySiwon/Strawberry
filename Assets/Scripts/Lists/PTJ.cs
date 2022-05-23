using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJ : MonoBehaviour
{
    [Serializable]
    public class PrefabStruct
    {
        public string Name;//�˹ٻ� �̸�, ���� ����
        public Sprite Picture;//����
        public Sprite FacePicture;//�˹ٻ� �� ����
        public string Explanation;//����
        public int Price;//����
        public int NowSliderNum;//���� �� ��� Ƚ��

        public PrefabStruct(string Name, string Explanation, int Price, Sprite Picture, Sprite FacePicture, int NowSliderNum)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Picture = Picture;
            this.FacePicture = FacePicture;
            this.NowSliderNum = NowSliderNum;
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    PrefabStruct[] Info;//����ü

    //Research Info  ������ �͵�
    [Header("==========INFO ������ ���=========")]
    [SerializeField]
    private GameObject PTJBackground;
    public GameObject nameText;
    public GameObject facePicture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject employTF;


    [Header("==========PTJ select Sprite===========")]
    //PTJ����߿��� ���� ��������Ʈ
    public Sprite selectPTJSprite;
    public Sprite originalPTJSprite;
    [Header("==========PTJ Button Sprite===========")]
    public Sprite FireButtonSprite;
    public Sprite HireButtonSprite;



    [Header("==========PTJ Warning Panel===========")]
    public GameObject warningPanel;
    public GameObject warningBlackPanel;
    public GameObject noCoinPanel;
    public Text noCoinPanel_text;
    public GameObject HireYNPanel;
    public Button HireYNPanel_yes;
    public GameObject confirmPanel;



    //==========Prefab���� ���� �ο�==========
    static int Prefabcount = 0;
    int prefabnum;

    //==========PTJ â==========
    private GameObject PTJPanel;
    //==========PTJ Content==========
    private GameObject PTJContent;

    //==========���� ��� �� �ο�==========
    static int employCount = 0;

    //==========���� ��� �� ���==========
    static List<Sprite> workingList = new List<Sprite>();


    //==========PTJ Panel==========
    private GameObject PTJSlider;
    private GameObject PTJSlider10;
    private GameObject PTJToggle;

    private GameObject SliderNum;
    private GameObject price;

    private GameObject PTJButton;

    private GameObject HireNowLock;

    //==========��� Ƚ��==========
    private int PTJ_NUM_NOW;


    //???
    //�� ��ƼŬ
    private ParticleSystem rainParticle;
    // �۷ι� ����
    private Globalvariable globalVar;


    //===================================================================================================
    //===================================================================================================
    private void Awake()
    {
        
    }


    void Start()
    {  
        //�����յ鿡�� ���� ��ȣ ���̱�
        prefabnum = Prefabcount;
        Prefabcount++;
        //==========PTJ Panel==========
        PTJPanel = GameObject.FindGameObjectWithTag("PTJExplanation");
        PTJPanel = PTJPanel.transform.GetChild(prefabnum).GetChild(0).gameObject;

        PTJToggle = PTJPanel.transform.GetChild(8).transform.gameObject;//10���� üũ ���
        PTJSlider = PTJPanel.transform.GetChild(9).transform.gameObject;//1���� �����̴�
        PTJSlider10 = PTJPanel.transform.GetChild(10).transform.gameObject;//10���� �����̴�

        SliderNum = PTJPanel.transform.GetChild(11).gameObject;
        price = PTJPanel.transform.GetChild(7).gameObject;

        PTJButton = PTJPanel.transform.GetChild(6).transform.gameObject;
        HireNowLock= PTJPanel.transform.GetChild(12).transform.gameObject;
        //==============================
        PTJContent = GameObject.FindGameObjectWithTag("PTJContent");

        HireYNPanel_yes.onClick.AddListener(BtnListener);

        //??============================
        rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();

        PreafbInfoUpdate();
    }

    private void Update()
    {

        //�ڽ��� ���Ƚ���� ���� �ľ�
        PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];

    }


    int PTJNumNow
    {
        set
        {
            if (PTJ_NUM_NOW == value) return;
            PTJ_NUM_NOW = value;

            //����� ���� 0(�� �˹ٸ� ���´ٸ�)�̸� �Ʒ��� ����
            if (PTJ_NUM_NOW == 0)
            {
                Debug.Log(prefabnum + "�� ° �˹ٻ� �� ����");
            }
            else 
            {
                HireNowLock.transform.GetChild(0).transform.GetComponent<Text>().text
                = "���� ��� Ƚ��: " + DataController.instance.gameData.PTJNum[DataController.instance.gameData.PTJSelectNum].ToString() + "ȸ";

            }
            //������� ���� ����
            EmployStateApply_Panel();
            EmployStateApply_Prefab();
        }
        get { return PTJ_NUM_NOW; }
    }

    //===================================================================================================
    //===================================================================================================
    public void PreafbInfoUpdate()
    {

        //====������ ���� ä���====
        //�Һ�====
        //�̸�
        nameText.GetComponent<Text>().text = Info[prefabnum].Name;
        //�˹ٻ� ����
        facePicture.GetComponent<Image>().sprite = Info[prefabnum].Picture;
        //����
        explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation;
        //���
        GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price);

        //����====
        //��� ���� �ݿ�
        EmployStateApply_Panel();
        EmployStateApply_Prefab();

    }

    //PTJPanel ����
    public void PTJPanelActive()
    {
        //ȿ����
        AudioManager.instance.Cute1AudioPlay();
        //���� ���� �˹ٻ�
        DataController.instance.gameData.PTJSelectNum = prefabnum;
        
        
        //==== �˹� â ====
        //�˹� â�� ����.
        PTJPanel.SetActive(true);
        PTJPanel.transform.GetChild(0).gameObject.SetActive(true);

        //�˹� â ä���
        GameObject picture = PTJPanel.transform.GetChild(3).gameObject;
        GameObject name = PTJPanel.transform.GetChild(4).gameObject;
        GameObject explanation = PTJPanel.transform.GetChild(5).gameObject;

        picture.GetComponent<Image>().sprite = Info[prefabnum].Picture;
        name.GetComponent<Text>().text = Info[prefabnum].Name;
        explanation.GetComponent<Text>().text = Info[prefabnum].Explanation;

        //�˹� �����̴�
        InitSlider();//�����̴� �ʱ�ȭ

        //Slider�� ���� ���� -> nȸ, ��� �ݿ�
        PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { SliderApply((int)PTJSlider.GetComponent<Slider>().value); });
        PTJSlider10.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { SliderApply((int)PTJSlider10.GetComponent<Slider>().value); });
        PTJToggle.GetComponent<Toggle>().onValueChanged.AddListener
            (delegate { ToggleChange(); });

        //�˹� ��� ���� �ݿ�
        EmployStateApply_Panel();
        EmployStateApply_Prefab();
    }

    private void EmployStateApply_Panel()
    {

        //������� �ƴϴ�
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //��ư �̹���, ���� ����
            PTJButton.transform.GetComponent<Image>().sprite = HireButtonSprite;
            PTJButton.transform.GetChild(0).transform.GetComponent<Text>().text = "��� �ϱ�";

            //HireNowLock �����
            HireNowLock.SetActive(false);
        }
        //������̴�
        else
        {
            //��ư �̹���, ���� ����
            PTJButton.transform.GetComponent<Image>().sprite = FireButtonSprite;
            PTJButton.transform.GetChild(0).transform.GetComponent<Text>().text = "";

            //HireNowLock ���̱�
            HireNowLock.SetActive(true);
        }

    }

    private void EmployStateApply_Prefab() 
    {

        //��� ���� �ƴϴ�
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            PTJBackground.GetComponent<Image>().sprite = originalPTJSprite;
            employTF.GetComponent<Text>().text = "�����";
            employTF.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
        }
        else 
        {
            PTJBackground.GetComponent<Image>().sprite = selectPTJSprite ;
            employTF.GetComponent<Text>().text = "�����";
            employTF.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
        }
        
    }

    public void EmployButtonClick()
    {
        //���� ���õ� �˹ٻ�
        //int nowPrefabNum = DataController.instance.gameData.PTJSelectNum;

        //ȿ����
        AudioManager.instance.Cute1AudioPlay();

        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //Hire(prefabnum, Info[prefabnum].NowSliderNum);
            Debug.Log("hire"+prefabnum);
        }
        else 
        {
            //DataController.instance.gameData.PTJNum[prefabnum] = 0;
            Debug.Log("fire"+prefabnum);
        }
        /*
        //=======================��� or �ذ�=======================
        //3�� �Ʒ��� ������̸�
        if (employCount < 3)
        {
            //������� �ƴϸ�
            if (DataController.instance.gameData.PTJNum[nowPrefabNum] == 0)
            {
                if (Info[nowPrefabNum].Price <= DataController.instance.gameData.coin)
                {

                    //���λ��
                    //GameManager.instance.UseCoin(Info[ID].Price);

                    //HIRE
                    Hire(nowPrefabNum, Info[nowPrefabNum].NowSliderNum);

                }
                else
                { 
                    //ȿ����
                    AudioManager.instance.Cute4AudioPlay();
                    //��ȭ ���� ��� �г�
                    GameManager.instance.ShowCoinText(noCoinPanel_text, DataController.instance.gameData.coin);
                    warningBlackPanel.SetActive(true);
                    noCoinPanel.GetComponent<PanelAnimation>().OpenScale();

                }
            }
            //�̹� ����� �̸�
            else
            {
                //Fire ���� â
                warningBlackPanel.SetActive(true);
                HireYNPanel.GetComponent<PanelAnimation>().OpenScale();
            }
        }
        //3���̻� ������̸�
        else
        {
            //������� �ƴϸ� no
            if (DataController.instance.gameData.PTJNum[nowPrefabNum] == 0)
            {
                //3���̻� ������̶�� ��� �г� ����
                confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "��� ������ �˹� ����\n�Ѿ���!";
                warningBlackPanel.SetActive(true);
                confirmPanel.GetComponent<PanelAnimation>().OpenScale();
            }
            //�̹� ������̸�
            else
            {
                //Fire ���� â
                warningBlackPanel.SetActive(true);
                HireYNPanel.GetComponent<PanelAnimation>().OpenScale();
            }
        }
        */
        //��� ���� �ݿ�
        EmployStateApply_Panel();
        EmployStateApply_Prefab();
    }
    private void Hire(int ID, int num)
    {
        //���
        DataController.instance.gameData.PTJNum[ID] = num;

        //������� �˹ٻ� �� ����
        ++employCount;
        GameManager.instance.workingCount(employCount);

        
        //main game
        workingList.Remove(null);
        workingList.Add(Info[ID].FacePicture);
        GameManager.instance.workingApply(workingList, ID);
        GameManager.instance.workingID.Add(ID);
        
    }

    private void Fire(int ID)
    {

        //��� ����
        DataController.instance.gameData.PTJNum[ID] = 0;

        //��� ���� �˹ٻ� �� ����
        --employCount;
        GameManager.instance.workingCount(employCount);

        //main game
        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList, ID);
        workingList.Remove(null);
        GameManager.instance.workingApply(workingList, ID);
        GameManager.instance.workingID.Remove(ID);

        InitSlider();
    }

    //�ذ� yes��ư������
    private void BtnListener() //�ÿ� �ǵ帲
    {
        //�ذ��ϱ�
        Fire(DataController.instance.gameData.PTJSelectNum);

        //�ذ� Ȯ�� �г� ������
        warningBlackPanel.SetActive(false);
        HireYNPanel.GetComponent<PanelAnimation>().CloseScale();

        //�ذ� �뺸 �г� �ø���
        confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "�ذ��߾�� �Ф�";
        confirmPanel.GetComponent<PanelAnimation>().OpenScale();
    }





    //====================================================================================================
    //SLIDER
    public void SliderApply(int value)
    {

        //10�����̸� 10�� �����ش�.
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        {  value *= 10;  }

        //"nȸ"
        SliderNum.transform.GetComponent<Text>().text = value.ToString() + "ȸ";
        Info[DataController.instance.gameData.PTJSelectNum].NowSliderNum = value;
            
        //����
        price.GetComponent<Text>().text = (value * Info[DataController.instance.gameData.PTJSelectNum].Price).ToString();
        //GameManager.instance.ShowCoinText(price.GetComponent<Text>(), Info[prefabnum].Price); �����ּ���

    }
    public void ToggleChange()
    {
        //10����
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        {
            PTJSlider10.SetActive(true);
            PTJSlider.SetActive(false);
            InitSlider();
        }
        else 
        {
            PTJSlider10.SetActive(false);
            PTJSlider.SetActive(true);
            InitSlider();
        }
    }
    public void InitSlider()
    {

        //10����
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        {
            //slider
            PTJSlider10.SetActive(true);
            PTJSlider.SetActive(false);

            PTJSlider10.GetComponent<Slider>().value = 1;

            Info[DataController.instance.gameData.PTJSelectNum].NowSliderNum = 10;

            //nȸ
            SliderNum.transform.GetComponent<Text>().text = "10ȸ";
            //����
            price.GetComponent<Text>().text = (10 * Info[DataController.instance.gameData.PTJSelectNum].Price).ToString(); //�����ּ���
            
        }
        //1����
        else
        {
            //slider
            PTJSlider.SetActive(true);
            PTJSlider10.SetActive(false);

            PTJSlider.GetComponent<Slider>().value = 1;

            Info[DataController.instance.gameData.PTJSelectNum].NowSliderNum = 1;

            //nȸ
            SliderNum.transform.GetComponent<Text>().text = "1ȸ";
            //����
            price.GetComponent<Text>().text = (Info[DataController.instance.gameData.PTJSelectNum].Price).ToString();
            //GameManager.instance.ShowCoinText(PTJPanel.transform.GetChild(7).GetComponent<Text>(), Info[prefabnum].Price);//�����ּ���

        }
    }

    

}
