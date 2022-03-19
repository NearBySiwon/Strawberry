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
        public string Title;
        public int[] reward;
        public bool isDone_c;
        //public bool isUnlock_n;
        public string Exp_news;
        //public int Gauge_c;


        public ChallengeNewsStruct(string Title, int[] reward, bool isDone_c, bool isUnlock_n, string Exp_news, int Gauge_c)
        {
            this.Title = Title;
            this.reward = reward;
            this.isDone_c = isDone_c;
            //this.isUnlock_n = isUnlock_n;
            this.Exp_news = Exp_news;
            //this.Gauge_c = Gauge_c;
        }
    }
    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ChallengeNewsStruct[] Info;

    [Header("==========OBJECT==========")]
    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private GameObject countText_News;
    [SerializeField]
    private GameObject lock_News;
    public GameObject doneButton_Challenge;


    [SerializeField]
    private GameObject gaugeText_Challenge;
    [SerializeField]
    private GameObject Btn_Challenge;

    [Header("==========Gauge==========")]
    [SerializeField]
    private RectTransform GaugeContainer_Challenge;
    [SerializeField]
    private RectTransform Gauge_Challenge;




    [Header("==========SPRITE==========")]
    [SerializeField]
    private Sprite[] BtnImage_Challenge;

    [Header("==========��Ÿ==========")]
    [SerializeField]
    private bool isChallenge;

    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;

    GameObject newsExplanation;
    GameObject newsExp;

    GameObject medalText;

    private void Start()
    {
        
        newsExplanation = GameObject.Find("newsExplanation");//��׵� find ���� ��ũ��Ʈ�� ���� �ұ� ����غ���
        medalText = GameObject.Find("medalCount");
        InfoUpdate();
    }
    public void InfoUpdate() {

        //!!!!!!!!!!!!!!����!!!!!!!!!!!!!���� ������ ���ڿ� ���õǾ� �ִ�!!! ���� �����ؾ���
        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 7)
        { Prefabcount =0; }
        prefabnum = Prefabcount;



        //����ǥ��=======================================================
        titleText.GetComponent<Text>().text = Info[prefabnum].Title;

        
        if (isChallenge == true) //CHALLENGE=============================
        {
            if (DataController.instance.gameData.challengeGauge[prefabnum] == 30)//�������� �Ϸ�, 30�� ������ ���߿� �ٲܰ�
            {
                medalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();//�޴� ��Ȳ �ؽ�Ʈ�� ����
                Btn_Challenge.GetComponent<Image>().sprite = BtnImage_Challenge[1];
                
            }
            //�������� ������ ��ġ
            Gauge_Challenge.GetComponent<Image>().fillAmount = (float)DataController.instance.gameData.challengeGauge[prefabnum] / 30;
            //�������� ������ ��ġ ����
            gaugeText_Challenge.GetComponent<Text>().text = DataController.instance.gameData.challengeGauge[prefabnum].ToString();

        }
        else //NEWS=======================================================
        {
            if (DataController.instance.gameData.isNewsUnlock[prefabnum] == false) 
            { lock_News.SetActive(true); }
            countText_News.GetComponent<Text>().text = "0" + (prefabnum+1);
        }



        Prefabcount++;

    }

    //�������� �޼��� �Ϸ� ��ư ������
    public void ChallengeSuccess() {
        if (DataController.instance.gameData.challengeGauge[prefabnum] == 30)
        {
            DataController.instance.gameData.medal += 3;//�޴� �� ����
            
            doneButton_Challenge.SetActive(true);//���̻� ��ư ���������ϰ��ϱ�(���� �Ϸ� ��ư �̹��� �־)
        }
    }

    public void Explantion() {

        newsExp = newsExplanation.transform.GetChild(0).transform.gameObject;//newsExp

        newsExp.SetActive(true);
        try
        {
            if (DataController.instance.gameData.isNewsUnlock[prefabnum] == true)
            {
                //Explanation ������ ä���.
                newsExp.transform.GetChild(2).transform.gameObject.GetComponentInChildren<Text>().text = Info[prefabnum].Title;//�̸� ���� newsName
                newsExp.transform.GetChild(3).transform.gameObject.GetComponentInChildren<Text>().text = Info[prefabnum].Exp_news;//���� ���� newsTxt
                
            }
        }
        catch
        {
            //ExpChildren.SetActive(false);
            Debug.Log("ChallengeNews �ε��� ����");
        }


    }
}
