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
        public int[] Prices = new int[25];//����

        public PrefabStruct(string Name, string Explanation, int Price, int[] Prices, Sprite Picture, Sprite FacePicture, bool exist)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Prices = Prices;   // ���� ������ �迭��
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
    public GameObject titleText;
    public GameObject facePicture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;

    [Header("==========Research Or PTJ===========")]
    public bool PTJ;//���� �˹ٻ��ΰ� Ȯ��

    [Header("==========select PTJ===========")]
    //PTJ����߿��� ���� ��������Ʈ
    [SerializeField]
    private Sprite selectPTJSprite;
    [SerializeField]
    private Sprite originalPTJSprite;

    [Header("==========PTJ EXP===========")]
    //PTJ ����â
    public GameObject PTJExp;


    [Header("==========PTJ Warning Panel===========")]
    public GameObject warningPanel;
    public GameObject noCoinPanel;
    public GameObject warningBlackPanel;
    public GameObject confirmPanel;
    public GameObject FirePanel;
    public Text panelCoinText;
    public Button FireConfirmButton;

    public Sprite FireSprite;
    public Sprite HireSprite;

    //test
    private GameObject warningBlackPanel2;
    private GameObject noCoinPanel2;
    private GameObject panelCoinText2;

    //Prefab���� ���� �ο�
    static int Prefabcount = 0;
    int prefabnum;


    //��� ��������� Ȯ��
    static int employCount = 0;

    //������� �˹ٻ� ���
    static List<Sprite> workingList = new List<Sprite>();
    

    //PTJ Exp�� �����̵� ����
    private GameObject PTJSlider;
    private GameObject PTJSlider10;
    private GameObject PTJToggle;

    //���Ƚ��
    private int PTJ_NUM_NOW;

    //�� ��ƼŬ
    private ParticleSystem rainParticle;

    //������� �ұ�� Ȯ�� �г�
    private GameObject HireYNPanel;

    // �۷ι� ����
    private Globalvariable globalVar;
    //===================================================================================================
    //===================================================================================================

    void Start()
    {
        InfoUpdate();

        if (PTJ == true)
        {
            PTJToggle = PTJExp.transform.GetChild(8).transform.gameObject;//10���� üũ ���
            PTJSlider = PTJExp.transform.GetChild(9).transform.gameObject;//1���� �����̴�
            PTJSlider10= PTJExp.transform.GetChild(10).transform.gameObject;//10���� �����̴�

            //Init Slider =(10���� �����̴��� ���δ�)
            PTJSlider.SetActive(true);
            PTJSlider10.SetActive(false);
            InitSlider();
            
            //������̶�� ����� ���·� ���̱�
            if (DataController.instance.gameData.PTJNum[prefabnum] != 0)
            {  HireInit(prefabnum, DataController.instance.gameData.PTJNum[prefabnum]);  }
            FireConfirmButton.onClick.AddListener(BtnListener);
        }
        rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();


        warningPanel = GameObject.FindGameObjectWithTag("WarningPanel");
        warningBlackPanel2 = warningPanel.transform.GetChild(0).gameObject;
        noCoinPanel2 = warningPanel.transform.GetChild(1).gameObject;
        panelCoinText2 = noCoinPanel2.transform.GetChild(2).transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        if (PTJ == true)
        {
            //PTJNumNow ���� ����ȴٸ� set get����ȴ�.
            PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];

            //n��������� ��Ȳ�� ���δ�.
            PTJExp.transform.GetChild(12).transform.GetComponent<Text>().text
                = "���� ��� Ƚ��: "+DataController.instance.gameData.PTJNum[prefabnum].ToString() +"ȸ";
        }
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
                //Fire() �� ���������� employment�������� �ߺ� �ð������� ����
                //=================================
                PTJToggle.GetComponent<Toggle>().isOn = false;

                //��� Ȱ��ȭ/n������� ���� ��Ȱ��ȭ
                PTJToggle.SetActive(true);
                PTJExp.transform.GetChild(12).gameObject.SetActive(false);

                //��� ����
                DataController.instance.gameData.PTJNum[prefabnum] = 0;

                //������� ���� �̹���
                levelNum.GetComponent<Text>().text = "��� ��";
                levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
                PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

                //main game�� ��Ȳ ���� ������â ���߿� �պ���
                --employCount;
                workingList.Remove(Info[prefabnum].FacePicture);
                workingList.Add(null);
                GameManager.instance.workingApply(workingList,prefabnum);
                workingList.Remove(null);
                GameManager.instance.workingApply(workingList,prefabnum);
                GameManager.instance.workingCount(employCount);

                GameManager.instance.workingID.Remove(prefabnum);

                //��Ÿ Ȱ��ȭ
                PTJExp.transform.GetChild(13).gameObject.SetActive(true);//nȸ �����
                PTJExp.transform.GetChild(7).transform.GetChild(0).gameObject.SetActive(true);

                PTJExp.transform.GetChild(6).transform.GetComponent<Image>().sprite = HireSprite;
                PTJExp.transform.GetChild(6).transform.GetChild(0).transform.GetComponent<Text>().text = "��� �ϱ�";

                //�����̴� �ʱ�ȭ
                InitSlider();

            }
        }
        get { return PTJ_NUM_NOW; }
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

        Info[Prefabcount].Prices = new int[25];

        for (int i=0; i<25; i++)
        {
            Info[prefabnum].Prices[i] = 100 * (i+1);
        }

        //Ÿ��Ʋ, ����, ���ΰ�, ����, ��뿩�� �ؽ�Ʈ�� ǥ��
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;//����(�̸�) ǥ��
        if (Info[Prefabcount].Picture != null)
        {
            facePicture.GetComponent<Image>().sprite = Info[Prefabcount].Picture;//�׸� ǥ��
        }
        
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation+"\n"+
            ((DataController.instance.gameData.researchLevel[prefabnum]*2) + "% ��" + 
            (DataController.instance.gameData.researchLevel[prefabnum]+1)*2 + "%");//���� �ؽ�Ʈ ǥ��

        //coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString() + "A";
        if (PTJ == true)//�˹�
            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[Prefabcount].Price); //��� ǥ��
        else
            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[Prefabcount].Prices[DataController.instance.gameData.researchLevel[prefabnum]]); //��� ǥ��


        if (PTJ == true)//�˹�
        { levelNum.GetComponent<Text>().text = "��� ��"; }
        else//����
        { levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString(); }



        Prefabcount++;
    }

    //=============================================================================================================================

    //���� ����
    public void clickCoin_Research() {
        AudioManager.instance.Cute1AudioPlay();
        if (DataController.instance.gameData.researchLevel[prefabnum] < 26)//���� 25�� �Ѱ�α�
        {
            //�ش� �ݾ��� ���� ���� ���κ��� ������
            if (DataController.instance.gameData.coin >= Info[prefabnum].Prices[DataController.instance.gameData.researchLevel[prefabnum]])
            {
                //�ش� �ݾ��� ������ ����
                GameManager.instance.UseCoin(Info[prefabnum].Prices[DataController.instance.gameData.researchLevel[prefabnum]]);
                levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();
                DataController.instance.gameData.researchLevel[prefabnum]++;
                levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();

                if (DataController.instance.gameData.researchLevel[prefabnum] == 25)
                {
                    coinNum.GetComponent<Text>().text = "Max";
                    explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
                        ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%");//���� �ؽ�Ʈ ǥ��
                }
                else
                {
                    GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Prices[DataController.instance.gameData.researchLevel[prefabnum]]);

                    switch (Info[prefabnum].Name)
                    {
                        case "���� ���� �ݰ�": IncreaseBerryPrice(); break;
                        case "���Ⱑ ����": DecreaseBerryGrowTime(); break;
                        case "�θ��� �� ��": break;
                        case "������� ������": DecreaseBugGenerateProb(); break;
                        case "���� ���̹���": DecreaseWeedGenerateProb(); break;
                        case "�ÿ��� �ҳ���": IncreaseRainDuration(); break;
                    }

                    explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
                    ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%" + "��" +
                    (DataController.instance.gameData.researchLevel[prefabnum] + 1) * 2 + "%");//���� �ؽ�Ʈ ǥ��
                }
            }
            else
            {
                //��ȭ ���� ��� �г� ����
                AudioManager.instance.Cute4AudioPlay();
                //panelCoinText2.GetComponent<Text>().text= DataController.instance.gameData.coin.ToString();
                GameManager.instance.ShowCoinText(panelCoinText2.GetComponent<Text>(), DataController.instance.gameData.coin);
                warningBlackPanel2.SetActive(true);
                noCoinPanel2.GetComponent<PanelAnimation>().OpenScale();
            }
        }
        else
        {
            // ���� �̹� �ƽ���~ �г� �ʿ� (�ƴԸ���)
            levelNum.GetComponent<Text>().text = "Max";
            coinNum.GetComponent<Text>().text = "Max";
            explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
                ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%");//���� �ؽ�Ʈ ǥ��
            //Debug.Log("���� �� �ƽ���");
        }
            

    }
    public void IncreaseBerryPrice()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[0]) * globalVar.getEffi();     
        for (int i = 0; i < 192; i++)
        {
            if (globalVar.berryListAll[i] == null) continue;

            if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((globalVar.CLASSIC_FIRST + i * 3) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((globalVar.SPECIAL_FIRST + (i - 64) * 5) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((globalVar.UNIQUE_FIRST + (i - 128) * 7) * (1 + researchCoeffi));
        }
    }
    public void DecreaseBerryGrowTime()
    {
        
        float researchCoeffi = (DataController.instance.gameData.researchLevel[1]) * Globalvariable.instance.getEffi();

        for (int i = 0; i < DataController.instance.gameData.stemLevel.Length; i++)
        {
            DataController.instance.gameData.stemLevel[i] = (Globalvariable.instance.STEM_LEVEL[i] * (1 - researchCoeffi));
        }

    }
    public void DecreaseBugGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[3]) * Globalvariable.instance.getEffi();     
        DataController.instance.gameData.bugProb = (Globalvariable.BUG_PROB * (1 - researchCoeffi));
    }
    public void DecreaseWeedGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[4]) * Globalvariable.instance.getEffi();
        DataController.instance.gameData.weedProb = Globalvariable.WEED_PROB * (1 - researchCoeffi);
    }
    public void IncreaseRainDuration()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[5]) * Globalvariable.instance.getEffi();
        var main = rainParticle.main;
        main.duration = Globalvariable.RAIN_DURATION * (1 + researchCoeffi);
        DataController.instance.gameData.rainDuration = Globalvariable.RAIN_DURATION * (1 + researchCoeffi);
    }
    //=============================================================================================================================


    //PTJ����â ����
    public void ActiveExplanation()
    {
        AudioManager.instance.Cute1AudioPlay();
        //â�� ����.
        PTJExp.SetActive(true);

        //PICTURE
        PTJExp.transform.GetChild(3).transform.GetComponent<Image>().sprite
            = Info[prefabnum].Picture;
        //NAME
        PTJExp.transform.GetChild(4).transform.GetComponent<Text>().text
            = Info[prefabnum].Name;
        //Explanation
        PTJExp.transform.GetChild(5).transform.GetComponent<Text>().text
            = Info[prefabnum].Explanation;

        //EmployButton Init
        InitSlider();



        //Slider�� ����ɶ� ���� -> n�����, ��뿡 �ݿ�
        PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { EmployButtonHire((int)(PTJSlider.GetComponent<Slider>().value)); });
        PTJSlider10.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { EmployButtonHire((int)(PTJSlider10.GetComponent<Slider>().value)); });

    }

    //��� ��ư ���� ����
    public void EmployButtonHire(int SliderNum)
    {
        PTJExp.transform.GetChild(6).transform.GetComponent<Image>().sprite = HireSprite;
        PTJExp.transform.GetChild(6).transform.GetChild(0).transform.GetComponent<Text>().text = "��� �ϱ�";
        if (PTJToggle.GetComponent<Toggle>().isOn == true) 
        { SliderNum *= 10; }//10�����̸� 10�� �����ش�.

        //������� �ƴϸ�
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //"n�� ���"
            PTJExp.transform.GetChild(13).transform.GetComponent<Text>().text
                = SliderNum.ToString() + "ȸ";
            //PRICE �ؽ�Ʈ
            GameManager.instance.ShowCoinText(PTJExp.transform.GetChild(7).GetComponent<Text>(), SliderNum * Info[prefabnum].Price);
        }

    }

    //��� ���� ��ư
    private void EmployButtonFire()
    {
        //EmployButton �ؽ�Ʈ�� "��� ����"��
        PTJExp.transform.GetChild(6).transform.GetComponent<Image>().sprite = FireSprite;
        PTJExp.transform.GetChild(6).transform.GetChild(0).transform.GetComponent<Text>().text = "";
        //PRICE �ؽ�Ʈ�� ��ĭ����
        PTJExp.transform.GetChild(7).transform.GetComponent<Text>().text = "";

    }
    //============================================================================================================
    public void HireFire() {
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
                DataController.instance.gameData.PTJFireConfirm = prefabnum; 
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
                DataController.instance.gameData.PTJFireConfirm = prefabnum;
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

        HireInit(ID,num);
    }

    private void HireInit(int ID,int num) 
    {
        //����� �̹���
        levelNum.GetComponent<Text>().text = "��� ��";
        levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
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
        levelNum.GetComponent<Text>().text = "��� ��";
        levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

        //��Ÿ Ȱ��ȭ
        PTJExp.transform.GetChild(13).gameObject.SetActive(true);//nȸ �����
        PTJExp.transform.GetChild(7).transform.GetChild(0).gameObject.SetActive(true);

        //main game�� ��Ȳ ����
        if (DataController.instance.gameData.PTJNum[prefabnum] != 0){  --employCount;}
        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList, prefabnum);
        workingList.Remove(null);
        GameManager.instance.workingApply(workingList, prefabnum);
        GameManager.instance.workingCount(employCount);

        GameManager.instance.workingID.Remove(prefabnum);

        

        InitSlider();        
    }


    public void InitSlider() 
    {
        //������� �ƴϸ�
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //10����
            if (PTJToggle.GetComponent<Toggle>().isOn == true)
            {
                //slider
                PTJSlider10.SetActive(true);
                PTJSlider.SetActive(false);

                PTJSlider10.GetComponent<Slider>().value = 1;

                //Slider Num �ؽ�Ʈ
                PTJExp.transform.GetChild(13).transform.GetComponent<Text>().text = "10ȸ";
                //PRICE �ؽ�Ʈ
                GameManager.instance.ShowCoinText(PTJExp.transform.GetChild(7).GetComponent<Text>(), Info[prefabnum].Price);
            }
            //1����
            else
            {
                //slider
                PTJSlider.SetActive(true);
                PTJSlider10.SetActive(false);

                PTJSlider.GetComponent<Slider>().value = 1;

                //Slider Num�ؽ�Ʈ
                PTJExp.transform.GetChild(13).transform.GetComponent<Text>().text = "1ȸ";
                //PRICE �ؽ�Ʈ
                GameManager.instance.ShowCoinText(PTJExp.transform.GetChild(7).GetComponent<Text>(), Info[prefabnum].Price);
            }
        }
        //������̸�
        else
        {
            EmployButtonFire();
        }
    }

    private void FireConfirm() //�ÿ� �ǵ帲
    {

        warningBlackPanel.SetActive(true);
        FirePanel.GetComponent<PanelAnimation>().OpenScale();
        

    }

    //�ذ� yes��ư������
    private void BtnListener() //�ÿ� �ǵ帲
    {
        
        Fire(DataController.instance.gameData.PTJFireConfirm);
        warningBlackPanel.SetActive(false);
        FirePanel.GetComponent<PanelAnimation>().CloseScale();

        confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "�ذ��߾�� �Ф�";
        confirmPanel.GetComponent<PanelAnimation>().OpenScale();
    }
}
