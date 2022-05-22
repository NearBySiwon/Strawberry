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

        public PrefabStruct(string Name, string Explanation, int Price, int[] Prices, Sprite Picture, Sprite FacePicture, bool exist)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Picture = Picture;
            this.FacePicture = FacePicture;
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

    //���� ������ ����
    private int nowSelectPrice;

    //???
    //�� ��ƼŬ
    private ParticleSystem rainParticle;

    // �۷ι� ����
    private Globalvariable globalVar;
    //===================================================================================================
    //===================================================================================================
    private void Awake()
    {
        //�����յ鿡�� ���� ��ȣ ���̱�
        prefabnum = Prefabcount;
        Prefabcount++;


    }
    void Start()
    {
        InfoUpdate();

        //==========PTJ Panel==========
        PTJPanel = GameObject.FindGameObjectWithTag("PTJExplanation");
        PTJPanel = PTJPanel.transform.GetChild(0).gameObject;

        PTJToggle = PTJPanel.transform.GetChild(8).transform.gameObject;//10���� üũ ���
        PTJSlider = PTJPanel.transform.GetChild(9).transform.gameObject;//1���� �����̴�
        PTJSlider10 = PTJPanel.transform.GetChild(10).transform.gameObject;//10���� �����̴�

        SliderNum = PTJPanel.transform.GetChild(11).gameObject;
        price = PTJPanel.transform.GetChild(7).gameObject;

        PTJButton = PTJPanel.transform.GetChild(6).transform.gameObject;
        HireNowLock= PTJPanel.transform.GetChild(12).transform.gameObject;
        //==============================


        /*
        //������̶�� ����� ���·� ���̱�
        if (DataController.instance.gameData.PTJNum[prefabnum] != 0)
        { HireInit(prefabnum, DataController.instance.gameData.PTJNum[prefabnum]); }
        FireConfirmButton.onClick.AddListener(BtnListener);

        rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();


        warningPanel = GameObject.FindGameObjectWithTag("WarningPanel");
        warningBlackPanel2 = warningPanel.transform.GetChild(0).gameObject;
        noCoinPanel2 = warningPanel.transform.GetChild(1).gameObject;
        panelCoinText2 = noCoinPanel2.transform.GetChild(2).transform.GetChild(0).gameObject;
        */
    }
    private void Update()
    {
        /*
        //PTJNumNow ���� ����ȴٸ� set get����ȴ�.
        PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];

        //n��������� ��Ȳ�� ���δ�.
        PTJExp.transform.GetChild(12).transform.GetComponent<Text>().text
            = "���� ��� Ƚ��: " + DataController.instance.gameData.PTJNum[prefabnum].ToString() + "ȸ";
        */
    }
    /*
    int PTJNumNow
    {
        set
        {
            if (PTJ_NUM_NOW == value) return;
            PTJ_NUM_NOW = value;

            //����� ���� 0(�� �˹ٸ� ���´ٸ�)�̸� �Ʒ��� ����
            if (PTJ_NUM_NOW == 0)
            {
                //Fire() �� ���������� employment�������� �ߺ� �ð������� ����
                //=================================
                PTJToggle.GetComponent<Toggle>().isOn = false;

                //��� Ȱ��ȭ/n������� ���� ��Ȱ��ȭ
                PTJToggle.SetActive(true);
                PTJExp.transform.GetChild(12).gameObject.SetActive(false);

                //��� ����
                DataController.instance.gameData.PTJNum[prefabnum] = 0;

                //������� ���� �̹���
                employTF.GetComponent<Text>().text = "��� ��";
                employTF.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
                PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

                //main game�� ��Ȳ ���� ������â ���߿� �պ���
                --employCount;
                workingList.Remove(Info[prefabnum].FacePicture);
                workingList.Add(null);
                GameManager.instance.workingApply(workingList, prefabnum);
                workingList.Remove(null);
                GameManager.instance.workingApply(workingList, prefabnum);
                GameManager.instance.workingCount(employCount);

                GameManager.instance.workingID.Remove(prefabnum);

                //��Ÿ Ȱ��ȭ
                PTJExp.transform.GetChild(13).gameObject.SetActive(true);//nȸ �����
                PTJExp.transform.GetChild(7).transform.GetChild(0).gameObject.SetActive(true);

                PTJExp.transform.GetChild(6).transform.GetComponent<Image>().sprite = HireButtonSprite;
                PTJExp.transform.GetChild(6).transform.GetChild(0).transform.GetComponent<Text>().text = "��� �ϱ�";

                //�����̴� �ʱ�ȭ
                InitSlider();

            }
        }
        get { return PTJ_NUM_NOW; }
    }
    */
    //===================================================================================================
    //===================================================================================================
    public void InfoUpdate()
    {

        //====������ ���� ä���====
        //�̸�
        nameText.GetComponent<Text>().text = Info[prefabnum].Name;
        //�˹ٻ� ����
        facePicture.GetComponent<Image>().sprite = Info[prefabnum].Picture;

        //����
        explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
            ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "% ��" +
            (DataController.instance.gameData.researchLevel[prefabnum] + 1) * 2 + "%");

        //���
        GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price);

        //��뿩��
        employTF.GetComponent<Text>().text = "��� ��";

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

        //�˹� â ä���
        GameObject picture = PTJPanel.transform.GetChild(3).gameObject;
        GameObject name = PTJPanel.transform.GetChild(4).gameObject;
        GameObject explanation = PTJPanel.transform.GetChild(5).gameObject;

        picture.GetComponent<Image>().sprite = Info[prefabnum].Picture;
        name.GetComponent<Text>().text = Info[prefabnum].Name;
        explanation.GetComponent<Text>().text = Info[prefabnum].Explanation;

        //�˹� �����̴�
        //�����̴� �ʱ�ȭ
        InitSlider();
        //Slider�� ���� ���� -> nȸ, ��� �ݿ�
        PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { SliderApply((int)PTJSlider.GetComponent<Slider>().value); });
        PTJSlider10.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { SliderApply((int)PTJSlider10.GetComponent<Slider>().value); });

        //�˹� ��� ���� �ݿ�
        EmployStateApply();

    }
    private void EmployStateApply()
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
    public void EmployButtonClick()
    {
        Debug.Log("select=" + DataController.instance.gameData.PTJSelectNum + " price=" + nowSelectPrice);
    }
    //====================================================================================================
    //SLIDER
    public void SliderApply(int value)
    {

        //10�����̸� 10�� �����ش�.
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        { value *= 10; }

        //������� �ƴϸ�
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //"nȸ"
            SliderNum.transform.GetComponent<Text>().text = value.ToString() + "ȸ";

            //����
            price.GetComponent<Text>().text = (value * Info[prefabnum].Price).ToString();
            //GameManager.instance.ShowCoinText(price.GetComponent<Text>(), Info[prefabnum].Price); �����ּ���
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

            //nȸ
            SliderNum.transform.GetComponent<Text>().text = "10ȸ";
            //����
            nowSelectPrice = 10 * Info[DataController.instance.gameData.PTJSelectNum].Price;
            price.GetComponent<Text>().text = nowSelectPrice.ToString(); //�����ּ���
            
        }
        //1����
        else
        {
            //slider
            PTJSlider.SetActive(true);
            PTJSlider10.SetActive(false);

            PTJSlider.GetComponent<Slider>().value = 1;

            //nȸ
            SliderNum.transform.GetComponent<Text>().text = "1ȸ";
            //����
            nowSelectPrice = Info[DataController.instance.gameData.PTJSelectNum].Price;
            price.GetComponent<Text>().text = nowSelectPrice.ToString();
            //GameManager.instance.ShowCoinText(PTJPanel.transform.GetChild(7).GetComponent<Text>(), Info[prefabnum].Price);//�����ּ���
        }
    }
    
    
    /*
   
    //============================================================================================================
    public void HireFire()
    {
        AudioManager.instance.Cute1AudioPlay();
        //3�� �Ʒ��� ������̸�
        if (employCount < 3)
        {
            //������� �ƴϸ� hire
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                if (Info[prefabnum].Price <= DataController.instance.gameData.coin)
                {
                    //HIRE
                    if (PTJToggle.GetComponent<Toggle>().isOn == true)
                    { Hire(prefabnum, (int)(PTJSlider10.GetComponent<Slider>().value) * 10); }
                    else
                    { Hire(prefabnum, (int)(PTJSlider.GetComponent<Slider>().value)); }

                    warningBlackPanel.SetActive(true);
                    confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "���Ǿ����!";
                    confirmPanel.GetComponent<PanelAnimation>().OpenScale();
                }
                else
                { //��ȭ ���� ��� �г� ����
                    AudioManager.instance.Cute4AudioPlay();
                    GameManager.instance.ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
                    warningBlackPanel.SetActive(true);
                    noCoinPanel.GetComponent<PanelAnimation>().OpenScale();
                }
            }
            //�̹� ������̸� fire
            else
            {
                DataController.instance.gameData.PTJSelectNum = prefabnum;
                FireConfirm();
            }
        }
        //�̹� 3���̻� ������̸�
        else
        {
            //������� �ƴϸ� no
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                //3���̻� ������̶�� ��� �г� ����
                warningPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "��� ������ �˹� ����\n�Ѿ���!";
                warningBlackPanel.SetActive(true);
                warningPanel.GetComponent<PanelAnimation>().OpenScale();
            }
            //�̹� ������̸� fire
            else
            {
                DataController.instance.gameData.PTJSelectNum = prefabnum;
                FireConfirm();
            }
        }
    }



    private void Hire(int ID, int num)
    {
        //���λ��
        GameManager.instance.UseCoin(Info[ID].Price);
        //n����� �Ѵٰ� ����
        DataController.instance.gameData.PTJNum[ID] = num;

        HireInit(ID, num);
    }

    private void HireInit(int ID, int num)
    {
        //����� �̹���
        employTF.GetComponent<Text>().text = "��� ��";
        employTF.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
        PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;

        //������� ���� �� ����
        ++employCount;
        GameManager.instance.workingCount(employCount);

        //main game
        workingList.Remove(null);
        workingList.Add(Info[ID].FacePicture);
        GameManager.instance.workingApply(workingList, prefabnum);

        GameManager.instance.workingID.Add(prefabnum);

        //�����̴�,���
        PTJSlider.SetActive(false);
        PTJSlider10.SetActive(false);
        PTJToggle.SetActive(false);

        //��Ÿ �����
        PTJExp.transform.GetChild(13).gameObject.SetActive(false);//nȸ �����
        PTJExp.transform.GetChild(7).transform.GetChild(0).gameObject.SetActive(false);//���� �̹��� ����� �̰� �� �ȼ�����

        //n����������� ǥ��
        PTJExp.transform.GetChild(12).gameObject.SetActive(true);


        EmployButtonFire();
    }

    private void Fire(int ID)
    {

        PTJToggle.GetComponent<Toggle>().isOn = false;

        //��� Ȱ��ȭ/n������� ���� ��Ȱ��ȭ
        PTJToggle.SetActive(true);
        PTJExp.transform.GetChild(12).gameObject.SetActive(false);

        //��� ����
        DataController.instance.gameData.PTJNum[ID] = 0;

        //������� ���� �̹���
        employTF.GetComponent<Text>().text = "��� ��";
        employTF.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

        //��Ÿ Ȱ��ȭ
        PTJExp.transform.GetChild(13).gameObject.SetActive(true);//nȸ �����
        PTJExp.transform.GetChild(7).transform.GetChild(0).gameObject.SetActive(true);

        //main game�� ��Ȳ ����
        if (DataController.instance.gameData.PTJNum[prefabnum] != 0) { --employCount; }
        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList, prefabnum);
        workingList.Remove(null);
        GameManager.instance.workingApply(workingList, prefabnum);
        GameManager.instance.workingCount(employCount);

        GameManager.instance.workingID.Remove(prefabnum);



        InitSlider();
    }




    private void FireConfirm() //�ÿ� �ǵ帲
    {

        warningBlackPanel.SetActive(true);
        FirePanel.GetComponent<PanelAnimation>().OpenScale();


    }

    //�ذ� yes��ư������
    private void BtnListener() //�ÿ� �ǵ帲
    {

        Fire(DataController.instance.gameData.PTJSelectNum);
        warningBlackPanel.SetActive(false);
        FirePanel.GetComponent<PanelAnimation>().CloseScale();

        confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "�ذ��߾�� �Ф�";
        confirmPanel.GetComponent<PanelAnimation>().OpenScale();
    }
    */
}
