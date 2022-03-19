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
        public string Exp_news;


        public ChallengeNewsStruct(string Title, int[] reward, bool isUnlock_n, string Exp_news, int Gauge_c)
        {
            this.Title = Title;
            this.reward = reward;
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
            //�������� �Ϸ��ؼ� ������� �޾�����
            if (DataController.instance.gameData.challengeEnd[prefabnum] == true)
            {
                doneButton_Challenge.SetActive(true);//���̻� ��ư ���������ϰ��ϱ�(���� �Ϸ� ��ư �̹��� �־)
            }

            if (DataController.instance.gameData.challengeGauge[prefabnum] == 30)//�������� �Ϸ�, 30�� ������ ���߿� �ٲܰ�
            {
                
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
            medalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();//�޴� ��Ȳ �ؽ�Ʈ�� ����
            DataController.instance.gameData.challengeEnd[prefabnum] = true;//����޾Ҵٴ°� ǥ��
            doneButton_Challenge.SetActive(true);//���̻� ��ư ���������ϰ��ϱ�(���� �Ϸ� ��ư �̹��� �־)
        }
    }









    //���� ����â ����
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
