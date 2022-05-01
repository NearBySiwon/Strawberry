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
    [SerializeField]
    private Sprite selectPTJSprite;
    [SerializeField]
    private Sprite originalPTJSprite;
    [Header("==========PTJ EXP===========")]
    public GameObject PTJExp;

    [Header("==========PTJ Warning Panel===========")]
    public GameObject PTJwarningPanel;
    public GameObject NoCoinPanel;
    public GameObject PTJBP;
    public Text panelCoinText;

    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;

    //��� ��������� Ȯ��
    static int employCount = 0;
    //������� �˹ٻ� ���
    static List<Sprite> workingList = new List<Sprite>();

    private GameObject PTJSlider;
    private bool isTenToggle=false;
    //===================================================================================================
    //10������ �ϴ°� ����

    void Start()
    {
        InfoUpdate();

        if (PTJ == true)
        {
            PTJSlider = PTJExp.transform.GetChild(7).transform.gameObject;//PTJSlider gameobject


            //�� �Ʒ� Hire�̶� �����ؼ� ó�� ���� ���ϰ�
            if (DataController.instance.gameData.PTJNum[prefabnum] != 0)
            {
                //����� ���·� �̹��� ��ȭ
                levelNum.GetComponent<Text>().text = "��� ��";
                levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
                PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;


                employCount++;
                GameManager.instance.workingCount(employCount);

                //main game
                workingList.Add(Info[prefabnum].FacePicture);
                GameManager.instance.workingApply(workingList);

                PTJSlider.SetActive(false);

                PTJExp.transform.GetChild(9).gameObject.SetActive(true);
                PTJExp.transform.GetChild(9).transform.GetComponent<Text>().text = DataController.instance.gameData.PTJNum[prefabnum].ToString() + "�� ������̴�.";
            }
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
        //EmployButton
        EmployButton(1);

        //Slider�� ����ɶ� ����

        PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate 
            { 
                if (isTenToggle == true) { EmployButton((int)(PTJSlider.GetComponent<Slider>().value)*10); } 
                else { EmployButton((int)(PTJSlider.GetComponent<Slider>().value)); }
            });

    }

    //��� ��ư ���� ����
    public void EmployButton(int SliderNum)
    {
        
        PTJSlider.GetComponent<Slider>().value = SliderNum;
        Debug.Log("ID = " + prefabnum + " / TEST = " + DataController.instance.gameData.PTJNum[prefabnum]);
        //������� �ƴϸ�
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
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
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                if (isTenToggle == true) { Hire(prefabnum, (int)(PTJSlider.GetComponent<Slider>().value)*10); }
                else { Hire(prefabnum, (int)(PTJSlider.GetComponent<Slider>().value)); }
                 
            }
            //�̹� ������̸�
            else
            { Fire(prefabnum); }
        }
        //�̹� 3���̻� ������̸�
        else
        {
            //������� �ƴϸ�
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                //Debug.Log("3���� �̹� ������Դϴ�.");
                //��� �г� ����
                GameManager.instance.DisableObjColliderAll();
                PTJBP.SetActive(true);
                PTJwarningPanel.SetActive(true);
                PTJwarningPanel.GetComponent<PanelAnimation>().OpenScale();
            }
            //�̹� ������̸�
            else
            { Fire(prefabnum); }
        }
    }

    private void Hire(int ID, int num)
    {


        GameManager.instance.UseCoin(Info[ID].Price);
        DataController.instance.gameData.PTJNum[ID] = num;

        //����� �̹���
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

        PTJSlider.SetActive(false);

        PTJExp.transform.GetChild(9).gameObject.SetActive(true);
        PTJExp.transform.GetChild(9).transform.GetComponent<Text>().text = num.ToString() + "�� ������̴�.";
    }
    private void Fire(int ID)
    {
        PTJSlider.SetActive(true);

        DataController.instance.gameData.PTJNum[ID] = 0;

        //������� �̹���
        levelNum.GetComponent<Text>().text = "��� ��";
        levelNum.GetComponent<Text>().color = new Color32(164, 164, 164, 255);
        PTJBackground.transform.GetComponent<Image>().sprite = originalPTJSprite;

        //main game
        --employCount;
        workingList.Remove(Info[ID].FacePicture);
        workingList.Add(null);
        GameManager.instance.workingApply(workingList);
        GameManager.instance.workingCount(employCount);

        EmployButton(1);
        DataController.instance.gameData.PTJNum[ID] = 0;

        PTJExp.transform.GetChild(9).gameObject.SetActive(false);
    }

    public void TenToggle()
    {
        if (isTenToggle == true) { isTenToggle = false; PTJSlider.GetComponent<Slider>().maxValue = 100; }
        else { 
            isTenToggle = true;
            PTJSlider.GetComponent<Slider>().maxValue = 10;
        }
    }
}
