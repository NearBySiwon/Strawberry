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
        public int[] reward_challenge;//���� = [0]�޴� / [1]��Ʈ
        public int condition_challenge;
        public string Exp_news;//���� ����


        public ChallengeNewsStruct(string Title, int[] reward_challenge,int condition_challenge, string Exp_news)
        {
            this.Title = Title;
            this.reward_challenge = reward_challenge;
            this.condition_challenge = condition_challenge;
            this.Exp_news = Exp_news;
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
        
        newsExplanation = GameObject.FindGameObjectWithTag("NewsExplanation");
        medalText = GameObject.FindGameObjectWithTag("medalCount");

        medalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();//�޴� ��Ȳ �ؽ�Ʈ�� ����
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

            //�������� �Ϸ�(���� �������� ������==�������� �޼� ��ġ)
            if (DataController.instance.gameData.challengeGauge[prefabnum] == Info[prefabnum].condition_challenge)
            {                
                Btn_Challenge.GetComponent<Image>().sprite = BtnImage_Challenge[1];//�������� ��ư �̹��� ����
                bangMark_Challenge.SetActive(true);//����ǥ!! ���� (ȹ�� ������ �������� �ִ�)
            }

            //�������� �Ϸ��ؼ� ������� ���� ��
            if (DataController.instance.gameData.challengeEnd[prefabnum] == true)
            {
                doneButton_Challenge.SetActive(true);//���̻� ��ư ���������ϰ��ϱ� ���� �Ϸ� ��ư �̹��� �߰�
                bangMark_Challenge.SetActive(false);//����ǥ �����
            }

            //�������� ������ �޼� ���� ����
            gaugeConditionText_Challenge.GetComponent<Text>().text = "/"+Info[prefabnum].condition_challenge.ToString();
            
            //�������� ������ ����
            Gauge_Challenge.GetComponent<Image>().fillAmount 
                = (float)DataController.instance.gameData.challengeGauge[prefabnum] / Info[prefabnum].condition_challenge;
            
            //�������� ������ ���� ����
            gaugeText_Challenge.GetComponent<Text>().text = DataController.instance.gameData.challengeGauge[prefabnum].ToString();
            

        }
        else //NEWS=======================================================
        {
            //���� lock�����̸� ������
            if (DataController.instance.gameData.isNewsUnlock[prefabnum] == false)
            { lock_News.SetActive(true); }
            //���� unlock�����̰� �������� �ʾ�����
            else if (DataController.instance.gameData.NewsEnd[prefabnum]==false) { unlock_News.SetActive(true); }

            
            //���� ����
            countText_News.GetComponent<Text>().text = "0" + (prefabnum+1);
        }



        Prefabcount++;

    }

    //�������� �޼��� �Ϸ� ��ư ������
    public void ChallengeSuccess() {
        if (DataController.instance.gameData.challengeGauge[prefabnum] == Info[prefabnum].condition_challenge)
        {
            //�޴�==================================================
            DataController.instance.gameData.medal += Info[prefabnum].reward_challenge[0];//���� �߰�
            medalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();//�޴� ��Ȳ �ؽ�Ʈ�� ����
            //��Ʈ==================================================
            DataController.instance.gameData.heart += Info[prefabnum].reward_challenge[1];//���� �߰�



            DataController.instance.gameData.challengeEnd[prefabnum] = true;//����޾Ҵٴ°� ǥ��
            doneButton_Challenge.SetActive(true);//���̻� ��ư ���������ϰ��ϱ�(���� �Ϸ� ��ư �̹��� �־)

            bangMark_Challenge.SetActive(false);//����ǥ �����
        }
    }






    public void UnlockNews() 
    {
        //�޴� ����
        DataController.instance.gameData.medal--;
        DataController.instance.gameData.NewsEnd[prefabnum] = true;
        Destroy(unlock_News); 
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
