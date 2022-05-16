using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeNews : MonoBehaviour
{
    [Serializable]
    public class ChallengeNewsStruct
    {
        public string Title;//����
        public int rewardMedal;
        public int rewardHeart;
        public int condition_challenge;
        public string Exp_news;//���� ����
        public int clearCriterion;

        public ChallengeNewsStruct(string Title, int rewardMedal, int rewardHeart, int condition_challenge, string Exp_news,int clearCriterion)
        {
            this.Title = Title;
            this.rewardMedal = rewardMedal;
            this.rewardHeart = rewardHeart;
            this.condition_challenge = condition_challenge;
            this.Exp_news = Exp_news;
            this.clearCriterion = clearCriterion;
        }
    }
    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ChallengeNewsStruct[] Info;

    [Header("==========OBJECT==========")]
    public GameObject levelText;
    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private GameObject countText_News;
    [SerializeField]
    private GameObject lock_News;//���� ���
    [SerializeField]
    private GameObject unlock_News;//���� ��� ���� ����

    public GameObject doneButton_Challenge;

    [SerializeField]
    private GameObject gaugeConditionText_Challenge;//�������� �޼� ���� �ؽ�Ʈ
    [SerializeField]
    private GameObject gaugeText_Challenge;//���� �������� �޼� ��ġ �ؽ�Ʈ
    [SerializeField]
    private GameObject Btn_Challenge;
    public GameObject bangMark_Challenge;

    [Header("==========Gauge==========")]
    [SerializeField]
    private RectTransform Gauge_Challenge;




    [Header("==========SPRITE==========")]
    [SerializeField]
    private Sprite IngButton;
    [SerializeField]
    private Sprite DoneButton;

    [Header("==========��Ÿ==========")]
    [SerializeField]
    private bool isChallenge;

    [Header("==========�г�==========")]
    public GameObject warnningPanel;
    public GameObject confirmPanel;
    public GameObject BP;

    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;



    //�̰� 2�� �ϳ��� ���̱� ����
    GameObject newsExplanation;
    GameObject newsExp;

    
    private int[] ChallengeValue = new int[6];


    private void Start()
    {
        newsExplanation = GameObject.FindGameObjectWithTag("NewsExplanation");
        GameManager.instance.ShowMedalText();//���� �޴��� ���δ�.
        InfoInit();
    }

    private void Update()
    {
        if (isChallenge == true)
        {
            ChallengeValue[0] = DataController.instance.gameData.unlockBerryCnt;
            ChallengeValue[1] = DataController.instance.gameData.totalHarvBerryCnt;
            ChallengeValue[2] = DataController.instance.gameData.accCoin;
            ChallengeValue[3] = DataController.instance.gameData.accHeart;
            ChallengeValue[4] = DataController.instance.gameData.accAttendance;
            ChallengeValue[5] = DataController.instance.gameData.mgPlayCnt;

            for (int i = 0; i < ChallengeValue.Length; i++)
            {
                if (ChallengeValue[i] % 30 == 0&& 
                    ChallengeValue[i] == Info[prefabnum].clearCriterion * DataController.instance.gameData.challengeLevel[prefabnum] + 1) 
                { ChallengeValue[i] = 30; }
                else { ChallengeValue[i] %= 30; }
            }
        }
        InfoUpdate();
    }
    //==================================================================================================================
    //==================================================================================================================
    private void InfoInit() {
        //!!!!!!!!!!!!!!����!!!!!!!!!!!!!���� ������ ���ڿ� ���õǾ� �ִ�!!! ���� �����ؾ���
        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 6)
        { Prefabcount = 0; }
        prefabnum = Prefabcount;

        Prefabcount++;

        titleText.GetComponent<Text>().text = Info[prefabnum].Title;//����ǥ��

        //���� ����
        if (isChallenge == true) 
        {
            Info[prefabnum].rewardMedal = 1;
            Info[prefabnum].clearCriterion = 30;// �ϴ� 30���� ����
            //�ӽ�
            switch (prefabnum) 
            {
                case 0: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion; break;
                case 1: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/5; break;
                case 2: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/50; break;
                case 3: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/30; break;
                case 4: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/6; break;
                case 5: Info[prefabnum].rewardHeart = Info[prefabnum].clearCriterion/10; break;
            }
            
        }

        
    }
    public void InfoUpdate() {

        if (isChallenge == true) //CHALLENGE=============================
        {
            levelText.GetComponent<Text>().text ="Lv."+ (1+DataController.instance.gameData.challengeLevel[prefabnum]).ToString();

            //�������� ������ ����
            Gauge_Challenge.GetComponent<Image>().fillAmount = (float)ChallengeValue[prefabnum]/ Info[prefabnum].clearCriterion;
            //�������� ������ �޼� ���� ����
            gaugeConditionText_Challenge.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion.ToString();
            //�������� ������ ���簪
            gaugeText_Challenge.GetComponent<Text>().text = ChallengeValue[prefabnum].ToString();

            //�������� �޼��ϸ�
            if (ChallengeValue[prefabnum] 
                == Info[prefabnum].clearCriterion*DataController.instance.gameData.challengeLevel[prefabnum]+1)
            {
                Btn_Challenge.GetComponent<Image>().sprite = DoneButton;//�������� ��ư �̹��� ����
                bangMark_Challenge.SetActive(true);//����ǥ!! ���� (ȹ�� ������ �������� �ִ�)
            }
            
            
            
        }
        else //NEWS=============================
        {
            //���� lock�����̸� ������
            if (DataController.instance.gameData.isNewsUnlock[prefabnum] == false)
            { lock_News.SetActive(true); }
            //���� unlock�����̰� �������� �ʾ�����
            else if (DataController.instance.gameData.NewsEnd[prefabnum]==false) { unlock_News.SetActive(true); }

            
            //���� ����
            countText_News.GetComponent<Text>().text = "0" + (prefabnum+1);
        }

    }
    //==================================================================================================================
    //==================================================================================================================
    //CHALLENGE

    //�������� �޼��� �Ϸ� ��ư ������

    public void ChallengeSuccess() {
        if (ChallengeValue[prefabnum] == Info[prefabnum].clearCriterion)
        {
            //�޴� ���� ȹ��
            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal);
            //��Ʈ ���� ȹ��
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart);

            Btn_Challenge.GetComponent<Image>().sprite = IngButton;//�������� ��ư �̹��� ����
            bangMark_Challenge.SetActive(false);//����ǥ �����

            DataController.instance.gameData.challengeLevel[prefabnum]++;//��������

        }
    }

    //==================================================================================================================
    //==================================================================================================================
    //NEWS
    public void UnlockNews() 
    {

        if (DataController.instance.gameData.medal >= 1)
        {
            //�޴� �ϳ� ���
            GameManager.instance.UseMedal(1);
            DataController.instance.gameData.NewsEnd[prefabnum] = true;
            Destroy(unlock_News);
            BP.SetActive(true);
            confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "������ �رݵǾ����!";
            confirmPanel.GetComponent<PanelAnimation>().OpenScale();
        }
        else
        {
            //�޴��� ������ ��
            warnningPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "������ �����ؿ�!";
            BP.SetActive(true);
            warnningPanel.GetComponent<PanelAnimation>().OpenScale();
        }
    }


    //���� ����â
    public void Explantion() {

        newsExp = newsExplanation.transform.GetChild(0).transform.gameObject;//newsExp

        newsExp.SetActive(true);
        try
        {
            if (DataController.instance.gameData.isNewsUnlock[prefabnum] == true)
            {
                //Explanation ������ ä���.
                newsExp.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text 
                    = Info[prefabnum].Title;//�̸� ���� newsName
                newsExp.transform.GetChild(3).transform.gameObject.GetComponentInChildren<Text>().text 
                    = Info[prefabnum].Exp_news;//���� ���� newsTxt 


            }
        }
        catch
        {
            Debug.Log("ChallengeNews �ε��� ����");
        }
    }


}
