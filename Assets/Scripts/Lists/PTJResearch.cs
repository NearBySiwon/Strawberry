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
        public string Name;//알바생 이름, 연구 제목
        public Sprite Picture;//사진
        public Sprite FacePicture;//알바생 얼굴 사진
        public string Explanation;//설명
        public int Price;//가격

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
    PrefabStruct[] Info;//구조체

    //Research Info  적용할 것들
    [Header("==========INFO 적용할 대상=========")]
    [SerializeField]
    private GameObject PTJBackground;
    public GameObject titleText;
    public GameObject facePicture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;

    [Header("==========Research Or PTJ===========")]
    public bool PTJ;//지금 알바생인가 확인

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

    //추가 된 Prefab 수
    static int Prefabcount = 0;
    //자신이 몇번째 Prefab인지
    int prefabnum;

    //몇명 고용중인지 확인
    static int employCount = 0;
    //고용중인 알바생 명단
    static List<Sprite> workingList = new List<Sprite>();

    private GameObject PTJSlider;
    private bool isTenToggle=false;
    //===================================================================================================
    //10단위로 하는거 구현

    void Start()
    {
        InfoUpdate();

        if (PTJ == true)
        {
            PTJSlider = PTJExp.transform.GetChild(7).transform.gameObject;//PTJSlider gameobject


            //이 아래 Hire이랑 연결해서 처리 가능 줄일것
            if (DataController.instance.gameData.PTJNum[prefabnum] != 0)
            {
                //고용중 상태로 이미지 변화
                levelNum.GetComponent<Text>().text = "고용 중";
                levelNum.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
                PTJBackground.transform.GetComponent<Image>().sprite = selectPTJSprite;


                employCount++;
                GameManager.instance.workingCount(employCount);

                //main game
                workingList.Add(Info[prefabnum].FacePicture);
                GameManager.instance.workingApply(workingList);

                PTJSlider.SetActive(false);

                PTJExp.transform.GetChild(9).gameObject.SetActive(true);
                PTJExp.transform.GetChild(9).transform.GetComponent<Text>().text = DataController.instance.gameData.PTJNum[prefabnum].ToString() + "번 고용중이다.";
            }
        }
    }
    //===================================================================================================
    //===================================================================================================
    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!주의!!!!!!!!!!!!!숫자 6은 프리팹 숫자와 관련되어 있다!!! 같이 조절 . 변수설정하기

        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 6)
        { Prefabcount -= 6; }
        prefabnum = Prefabcount;


        //타이틀, 설명, 코인값, 레벨, 고용여부 텍스트에 표시
        titleText.GetComponent<Text>().text = Info[Prefabcount].Name;//제목(이름) 표시
        if (Info[Prefabcount].Picture != null)
        {
            facePicture.GetComponent<Image>().sprite = Info[Prefabcount].Picture;//그림 표시
        }
        explanationText.GetComponent<Text>().text = Info[Prefabcount].Explanation;//설명 텍스트 표시
        coinNum.GetComponent<Text>().text = Info[Prefabcount].Price.ToString() + "A";//비용 표시


        if (PTJ == true)//알바
        { levelNum.GetComponent<Text>().text = "고용 전"; }
        else//연구
        { levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString(); }



        Prefabcount++;
    }

    //=============================================================================================================================

    //coin 버튼 -> 연구 레벨, 코인
    public void clickCoin_Research() {

        if (DataController.instance.gameData.researchLevel[prefabnum] < 25)//레벨 25로 한계두기
        {

            //해당 금액의 코인이 감소
            GameManager.instance.UseCoin(Info[prefabnum].Price);

            //해당 금액이 지금 가진 코인보다 적으면
            if (DataController.instance.gameData.coin >= Info[prefabnum].Price)
            {
                //레벨이 올라간다.
                DataController.instance.gameData.researchLevel[prefabnum]++;
                levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();
            }
            else if (DataController.instance.gameData.coin < Info[prefabnum].Price) //해당 금액이 지금 가진 코인보다 많으면
            {
                //재화 부족 경고 패널 등장
                GameManager.instance.DisableObjColliderAll();
                GameManager.instance.ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
                PTJBP.SetActive(true);
                NoCoinPanel.SetActive(true);
                NoCoinPanel.GetComponent<PanelAnimation>().OpenScale();
            }

        }
    }

    //=============================================================================================================================

    //PTJ설명창 띄운다
    public void ActiveExplanation()
    {
        //창을 띄운다.
        PTJExp.SetActive(true);

        //불변 내용 =======================================================
        //PICTURE
        PTJExp.transform.GetChild(2).transform.GetComponent<Image>().sprite
            = Info[prefabnum].Picture;
        //NAME
        PTJExp.transform.GetChild(3).transform.GetComponent<Text>().text
            = Info[prefabnum].Name;
        //Explanation
        PTJExp.transform.GetChild(4).transform.GetComponent<Text>().text
            = Info[prefabnum].Explanation;

        //가변 내용========================================================
        //EmployButton
        EmployButton(1);

        //Slider값 변경될때 마다

        PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate 
            { 
                if (isTenToggle == true) { EmployButton((int)(PTJSlider.GetComponent<Slider>().value)*10); } 
                else { EmployButton((int)(PTJSlider.GetComponent<Slider>().value)); }
            });

    }

    //고용 버튼 상태 변경
    public void EmployButton(int SliderNum)
    {
        
        PTJSlider.GetComponent<Slider>().value = SliderNum;
        Debug.Log("ID = " + prefabnum + " / TEST = " + DataController.instance.gameData.PTJNum[prefabnum]);
        //고용중이 아니면
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //n번 고용
            PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text
                = SliderNum.ToString() + "번 고용";
            //PRICE
            PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text
               = (SliderNum * Info[prefabnum].Price).ToString();
        }
        else //이미 고용중이면
        {
            //슬라이드 첫번째
            PTJSlider.GetComponent<Slider>().value = 1;
            //n번 고용 -> 고용 해제
            PTJExp.transform.GetChild(5).transform.GetChild(0).transform.GetComponent<Text>().text
                = "고용 해제";
            //PRICE -> 빈칸
            PTJExp.transform.GetChild(6).transform.GetComponent<Text>().text
                = "";
        }

    }
    //============================================================================================================
    public void HireFire() {

        //3명 아래로 고용중이면
        if (employCount < 3)
        {
            //고용중이 아니면
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                if (isTenToggle == true) { Hire(prefabnum, (int)(PTJSlider.GetComponent<Slider>().value)*10); }
                else { Hire(prefabnum, (int)(PTJSlider.GetComponent<Slider>().value)); }
                 
            }
            //이미 고용중이면
            else
            { Fire(prefabnum); }
        }
        //이미 3명이상 고용중이면
        else
        {
            //고용중이 아니면
            if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
            {
                //Debug.Log("3명을 이미 고용중입니다.");
                //경고 패널 등장
                GameManager.instance.DisableObjColliderAll();
                PTJBP.SetActive(true);
                PTJwarningPanel.SetActive(true);
                PTJwarningPanel.GetComponent<PanelAnimation>().OpenScale();
            }
            //이미 고용중이면
            else
            { Fire(prefabnum); }
        }
    }

    private void Hire(int ID, int num)
    {


        GameManager.instance.UseCoin(Info[ID].Price);
        DataController.instance.gameData.PTJNum[ID] = num;

        //고용중 이미지
        levelNum.GetComponent<Text>().text = "고용 중";
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
        PTJExp.transform.GetChild(9).transform.GetComponent<Text>().text = num.ToString() + "번 고용중이다.";
    }
    private void Fire(int ID)
    {
        PTJSlider.SetActive(true);

        DataController.instance.gameData.PTJNum[ID] = 0;

        //고용해제 이미지
        levelNum.GetComponent<Text>().text = "고용 전";
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
