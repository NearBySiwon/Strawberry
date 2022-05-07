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

        public PrefabStruct(string Name, string Explanation, int Price, Sprite Picture, Sprite FacePicture, bool exist)
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
    public GameObject PTJwarningPanel;
    public GameObject NoCoinPanel;
    public GameObject PTJBP;
    public Text panelCoinText;


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



    //===================================================================================================
    //===================================================================================================
    void Start()
    {
        InfoUpdate();

        if (PTJ == true)
        {
            PTJSlider = PTJExp.transform.GetChild(8).transform.gameObject;//1���� �����̴�
            PTJSlider10= PTJExp.transform.GetChild(9).transform.gameObject;//10���� �����̴�
            PTJToggle= PTJExp.transform.GetChild(7).transform.gameObject;//10���� üũ ���

            //Init Slider =(10���� �����̴��� ���δ�)
            PTJSlider.SetActive(true);
            PTJSlider10.SetActive(false);
            InitSlider();
            
            //������̶�� ����� ���·� ���̱�
            if (DataController.instance.gameData.PTJNum[prefabnum] != 0)
            {  HireInit(prefabnum, DataController.instance.gameData.PTJNum[prefabnum]);  }

        }
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

    //���� ����
    public void clickCoin_Research() {

        if (DataController.instance.gameData.researchLevel[prefabnum] < 25)//���� 25�� �Ѱ�α�
        {
            switch (Info[prefabnum].Name)
            {
                case "���� ��ġ ���": IncreaseBerryPrice(); break;
                case "���� ����Ⱓ ����": DecreaseBerryGrowTime(); break;
                case "Ʈ�� ���� ���": break;
                case "���� Ȯ�� ����": DecreaseBugGenerateProb(); break;
                case "���� Ȯ�� ����": DecreaseWeedGenerateProb(); break;
                case "�ҳ��� Ȯ�� ��":Debug.Log("�ҳ����"); break;
            }

            //�ش� �ݾ��� ������ ����
            GameManager.instance.UseCoin(Info[prefabnum].Price);

            //�ش� �ݾ��� ���� ���� ���κ��� ������
            if (DataController.instance.gameData.coin >= Info[prefabnum].Price)
            {
                //������ �ö󰣴�.
                DataController.instance.gameData.researchLevel[prefabnum]++;
                levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();
            }
            else if (DataController.instance.gameData.coin < Info[prefabnum].Price) //�ش� �ݾ��� ���� ���� ���κ��� ������
            {
                //��ȭ ���� ��� �г� ����
                GameManager.instance.DisableObjColliderAll();
                GameManager.instance.ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
                PTJBP.SetActive(true);
                NoCoinPanel.SetActive(true);
                NoCoinPanel.GetComponent<PanelAnimation>().OpenScale();
            }
        }
    }
    public void IncreaseBerryPrice()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[0]) * Globalvariable.instance.coeffi;

        for (int i = 0; i < 192; i++)
        {
            if (Globalvariable.instance.berryListAll[i] == null) continue;

            if (i < 64)
                Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((Globalvariable.CLASSIC_FIRST + i * 3) * (1 + researchCoeffi));
            else if (i < 128)
                Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((Globalvariable.SPECIAL_FIRST + (i - 64) * 5) * (1 + researchCoeffi));
            else
                Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((Globalvariable.UNIQUE_FIRST + (i - 128) * 7) * (1 + researchCoeffi));
        }
    }
    public void DecreaseBerryGrowTime()
    {
        
        float researchCoeffi = (DataController.instance.gameData.researchLevel[1]) * Globalvariable.instance.coeffi;

        for (int i = 0; i < DataController.instance.gameData.stemLevel.Length; i++)
        {
            DataController.instance.gameData.stemLevel[i] = (Globalvariable.instance.STEM_LEVEL[i] * (1 - researchCoeffi));
        }

    }
    public void DecreaseBugGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[3]) * Globalvariable.instance.coeffi;
        DataController.instance.gameData.bugProb = (Globalvariable.BUG_PROB * (1 - researchCoeffi));
    }
    public void DecreaseWeedGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[4]) * Globalvariable.instance.coeffi;
        DataController.instance.gameData.weedProb = Globalvariable.WEED_PROB * (1 - researchCoeffi);
    }
    public void IncreaseRainDuration()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[5]) * Globalvariable.instance.coeffi;

    }
    //=============================================================================================================================

    //PTJ����â ����
    public void ActiveExplanation()
    {
        //â�� ����.
        PTJExp.SetActive(true);

        //PICTURE
        PTJExp.transform.GetChild(2).transform.GetComponent<Image>().sprite
            = Info[prefabnum].Picture;
        //NAME
        PTJExp.transform.GetChild(3).transform.GetComponent<Text>().text
            = Info[prefabnum].Name;
        //Explanation
        PTJExp.transform.GetChild(4).transform.GetComponent<Text>().text
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
        //Debug.Log("ID = " + prefabnum + " / TEST = " + DataController.instance.gameData.PTJNum[prefabnum]);
        
        if (PTJToggle.GetComponent<Toggle>().isOn == true) 
        { SliderNum *= 10; }//10�����̸� 10�� �����ش�.

        //������� �ƴϸ�
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //EmployButton �ؽ�Ʈ�� "n�� ���"����
            PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text
                = SliderNum.ToString() + "�� ���";
            //PRICE �ؽ�Ʈ
            PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text
               = (SliderNum * Info[prefabnum].Price).ToString();
        }

    }

    //��� ���� ��ư
    private void EmployButtonFire()
    {
        //EmployButton �ؽ�Ʈ�� "��� ����"��
        PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text = "��� ����";
        //PRICE �ؽ�Ʈ�� ��ĭ����
        PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text = "";

    }
    //============================================================================================================
    public void HireFire() {

        //3�� �Ʒ��� ������̸�
        if (employCount < 3)
        {
            //������� �ƴϸ� hire
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                //HIRE
                if (PTJToggle.GetComponent<Toggle>().isOn == true) 
                { Hire(prefabnum, (int)(PTJSlider10.GetComponent<Slider>().value)*10); }
                else 
                { Hire(prefabnum, (int)(PTJSlider.GetComponent<Slider>().value)); } 
            }
            //�̹� ������̸� fire
            else
            { Fire(prefabnum); }
        }
        //�̹� 3���̻� ������̸�
        else
        {
            //������� �ƴϸ� no
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                //3���̻� ������̶�� ��� �г� ����
                GameManager.instance.DisableObjColliderAll();
                PTJBP.SetActive(true);
                PTJwarningPanel.SetActive(true);
                PTJwarningPanel.GetComponent<PanelAnimation>().OpenScale();
            }
            //�̹� ������̸� fire
            else
            { Fire(prefabnum); }
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
        GameManager.instance.workingApply(workingList);

        //�����̴�,���
        PTJSlider.SetActive(false);
        PTJSlider10.SetActive(false);
        PTJToggle.SetActive(false);

        //n����������� ǥ��
        PTJExp.transform.GetChild(11).gameObject.SetActive(true);
        PTJExp.transform.GetChild(11).transform.GetComponent<Text>().text = num.ToString() + "�� ������̴�.";

        EmployButtonFire();
    }

    private void Fire(int ID)
    {
        FireCat();
        PTJToggle.GetComponent<Toggle>().isOn = false;

        //��� Ȱ��ȭ/n������� ���� ��Ȱ��ȭ
        PTJToggle.SetActive(true);
        PTJExp.transform.GetChild(11).gameObject.SetActive(false);

        //��� ����
        DataController.instance.gameData.PTJNum[ID] = 0;

        //������� ���� �̹���
        levelNum.GetComponent<Text>().text = "��� ��";
        levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

        //main game�� ��Ȳ ����
        --employCount;
        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList);
        GameManager.instance.workingCount(employCount);

        InitSlider();        
    }
    private void FireCat() 
    {
        if (Info[prefabnum].Name == "�����") 
        {
            //��Ÿ�� ���� ����
            GameManager.instance.isCatTime=true;
        }
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

                //EmployButton �ؽ�Ʈ
                PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text = "10�� ���";
                //PRICE �ؽ�Ʈ
                PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text = (Info[prefabnum].Price).ToString();
            }
            //1����
            else
            {
                //slider
                PTJSlider.SetActive(true);
                PTJSlider10.SetActive(false);

                PTJSlider.GetComponent<Slider>().value = 1;

                //EmployButton �ؽ�Ʈ
                PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text = "1�� ���";
                //PRICE �ؽ�Ʈ
                PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text = (Info[prefabnum].Price).ToString();
            }
        }
        //������̸�
        else
        {
            EmployButtonFire();
        }

    }

    
    
}
